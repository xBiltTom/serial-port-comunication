using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Threading;

namespace ComunicacionRedes
{
    internal class Comunicacion
    {
        private SerialPort sPuerto;
        private Trama gestorTrama;

        // ── HILO ÚNICO DE ENVÍO ───────────────────────────────────────────
        private Thread hiloEnvio;
        private volatile bool detenerHilo;
        private readonly object bloqueoEnvio = new object();

        // ── COLA DE ALTA PRIORIDAD ────────────────────────────────────────
        // Contiene tramas ya construidas listas para enviar:
        // mensajes ('M'), inicio de archivo ('I') y fin de archivo ('F').
        private readonly Queue<byte[]> colaAltaPrioridad = new Queue<byte[]>();

        // ── FUENTES ROUND-ROBIN (BAJA PRIORIDAD) ─────────────────────────
        // Cada entrada representa un archivo en proceso de envío.
        // Clave: idCanal (1-5). Valor: contexto del archivo activo.
        private readonly Dictionary<int, ArchivoEnvio> archivosActivos
            = new Dictionary<int, ArchivoEnvio>();

        // Índice circular para el Round-Robin entre canales activos
        private int indiceRoundRobin = 0;

        // ── RECEPCIÓN: FileStreams de escritura en disco ──────────────────
        // Clave: idCanal. Valor: contexto del archivo que se está recibiendo.
        private readonly Dictionary<int, ArchivoRecepcion> archivosRecibiendo
            = new Dictionary<int, ArchivoRecepcion>();

        // ── EVENTOS PÚBLICOS ──────────────────────────────────────────────
        /// <summary>Se dispara cuando llega un mensaje de chat completo.</summary>
        public event Action<string> llegoMensaje;

        /// <summary>
        /// Se dispara cuando un archivo fue recibido y guardado completamente.
        /// Devuelve la ruta final donde quedó guardado.
        /// </summary>
        public event Action<string> llegoArchivo;

        // ─────────────────────────────────────────────────────────────────
        //  CONSTRUCTOR
        // ─────────────────────────────────────────────────────────────────
        public Comunicacion()
        {
            gestorTrama = new Trama();
            sPuerto = new SerialPort();
            sPuerto.DataReceived += SPuerto_DataReceived;
        }

        // ─────────────────────────────────────────────────────────────────
        //  PUERTOS DISPONIBLES
        // ─────────────────────────────────────────────────────────────────
        public string[] ObtenerPuertosDisponibles()
        {
            return SerialPort.GetPortNames();
        }

        // ─────────────────────────────────────────────────────────────────
        //  CONEXIÓN / DESCONEXIÓN
        // ─────────────────────────────────────────────────────────────────
        public void inicializaPuerto(string nombrePuerto, int velocidad)
        {
            cerrarPuerto();

            sPuerto.PortName = nombrePuerto;
            sPuerto.BaudRate = velocidad;
            sPuerto.DataBits = 8;
            sPuerto.Parity = Parity.Odd;
            sPuerto.StopBits = StopBits.Two;
            sPuerto.ReadBufferSize = 1024 * 64; // 64 KB — margen para ráfagas
            sPuerto.WriteBufferSize = 1024 * 64;

            sPuerto.Open();
            IniciarHebraEnvio();
        }

        public void cerrarPuerto()
        {
            DetenerHebraEnvio();
            LimpiarEstadoEnvio();

            if (sPuerto.IsOpen)
            {
                sPuerto.DiscardInBuffer();
                sPuerto.DiscardOutBuffer();
                sPuerto.Close();
            }
        }

        public bool estaConectado() => sPuerto.IsOpen;

        // ─────────────────────────────────────────────────────────────────
        //  API PÚBLICA DE ENVÍO
        // ─────────────────────────────────────────────────────────────────

        /// <summary>
        /// Encola un mensaje de chat para envío inmediato (alta prioridad).
        /// </summary>
        public void enviarMensaje(string mensaje)
        {
            if (!sPuerto.IsOpen)
                throw new Exception("El puerto no está abierto.");

            byte[] trama = gestorTrama.crearTramaMensaje(mensaje);
            EnqueueAltaPrioridad(trama);
        }

        /// <summary>
        /// Inicia el envío de un archivo en el canal indicado (1-5).
        /// Encola la trama 'I' (metadatos) y registra el FileStream
        /// para que el Round-Robin lo atienda de forma entrelazada.
        /// </summary>
        public void iniciarEnvioArchivo(int idCanal, string rutaArchivo)
        {
            if (!sPuerto.IsOpen)
                throw new Exception("El puerto no está abierto.");

            if (idCanal < 1 || idCanal > 5)
                throw new ArgumentException("El ID de canal debe estar entre 1 y 5.");

            lock (bloqueoEnvio)
            {
                if (archivosActivos.ContainsKey(idCanal))
                    throw new Exception($"El canal {idCanal} ya tiene un archivo en curso.");

                // Abrir stream de lectura del archivo
                FileStream fs = new FileStream(
                    rutaArchivo, FileMode.Open, FileAccess.Read, FileShare.Read);

                archivosActivos[idCanal] = new ArchivoEnvio
                {
                    Stream = fs,
                    IdCanal = idCanal,
                    Terminado = false
                };
            }

            // Encolar trama de inicio con el nombre del archivo (alta prioridad)
            string nombreArchivo = Path.GetFileName(rutaArchivo);
            byte[] tramaInicio = gestorTrama.crearTramaInicio(idCanal, nombreArchivo);
            EnqueueAltaPrioridad(tramaInicio);
        }

        // ─────────────────────────────────────────────────────────────────
        //  HILO ÚNICO DE ENVÍO — ARRANQUE Y PARADA
        // ─────────────────────────────────────────────────────────────────
        private void IniciarHebraEnvio()
        {
            detenerHilo = false;

            if (hiloEnvio == null || !hiloEnvio.IsAlive)
            {
                hiloEnvio = new Thread(BucleEnvio);
                hiloEnvio.IsBackground = true;
                hiloEnvio.Start();
            }
        }

        private void DetenerHebraEnvio()
        {
            detenerHilo = true;

            if (hiloEnvio != null && hiloEnvio.IsAlive)
                hiloEnvio.Join(2000);
        }

        // ─────────────────────────────────────────────────────────────────
        //  BUCLE PRINCIPAL DEL HILO DE ENVÍO (MULTIPLEXOR ROUND-ROBIN)
        // ─────────────────────────────────────────────────────────────────
        private void BucleEnvio()
        {
            while (!detenerHilo && sPuerto != null && sPuerto.IsOpen)
            {
                byte[] tramaAEnviar = null;

                lock (bloqueoEnvio)
                {
                    // ── 1. ALTA PRIORIDAD ─────────────────────────────────
                    // Mensajes de chat, tramas 'I' y 'F' siempre van primero.
                    if (colaAltaPrioridad.Count > 0)
                    {
                        tramaAEnviar = colaAltaPrioridad.Dequeue();
                    }
                    // ── 2. BAJA PRIORIDAD: ROUND-ROBIN entre archivos ─────
                    else if (archivosActivos.Count > 0)
                    {
                        tramaAEnviar = ObtenerSiguienteFragmentoRoundRobin();
                    }
                }

                if (tramaAEnviar != null)
                {
                    EsperarYEnviar(tramaAEnviar);
                }
                else
                {
                    // Nada que enviar: ceder CPU brevemente
                    Thread.Sleep(10);
                }
            }
        }

        /// <summary>
        /// Selecciona el siguiente canal activo en modo Round-Robin,
        /// lee un fragmento de su FileStream y construye la trama 'A'.
        /// Si el archivo se agota, encola la trama 'F' y limpia el canal.
        /// Devuelve null si no hay canales activos.
        /// </summary>
        private byte[] ObtenerSiguienteFragmentoRoundRobin()
        {
            // Obtener lista de IDs activos en orden estable
            List<int> canales = new List<int>(archivosActivos.Keys);
            if (canales.Count == 0) return null;

            // Ajustar índice circular al rango actual
            indiceRoundRobin = indiceRoundRobin % canales.Count;
            int idCanal = canales[indiceRoundRobin];

            ArchivoEnvio archivo = archivosActivos[idCanal];

            // Leer hasta TAMANIO_DATOS bytes del archivo
            byte[] buffer = new byte[Trama.TAMANIO_DATOS];
            int leidos = archivo.Stream.Read(buffer, 0, Trama.TAMANIO_DATOS);

            byte[] trama;

            if (leidos > 0)
            {
                // Hay datos: construir fragmento 'A'
                trama = gestorTrama.crearTramaFragmento(idCanal, buffer, leidos);

                // Avanzar al siguiente canal en la próxima iteración
                indiceRoundRobin = (indiceRoundRobin + 1) % canales.Count;
            }
            else
            {
                // Archivo agotado: señalizar fin con trama 'F'
                trama = gestorTrama.crearTramaFin(idCanal);

                // Liberar recursos y eliminar del diccionario
                archivo.Stream.Close();
                archivosActivos.Remove(idCanal);

                // Encolar la trama 'F' como alta prioridad para que salga
                // en la próxima iteración (garantía de entrega)
                colaAltaPrioridad.Enqueue(trama);

                // No devolver trama aquí para no saltarse el orden
                trama = null;

                // Reajustar índice si quedaron menos canales
                if (archivosActivos.Count > 0)
                    indiceRoundRobin = indiceRoundRobin % archivosActivos.Count;
            }

            return trama;
        }

        // ─────────────────────────────────────────────────────────────────
        //  ENVÍO FÍSICO AL PUERTO
        // ─────────────────────────────────────────────────────────────────
        private bool HayEspacioParaTrama()
        {
            return (sPuerto.WriteBufferSize - sPuerto.BytesToWrite) >= Trama.TAMANIO_TRAMA;
        }

        private void EsperarYEnviar(byte[] trama)
        {
            // Espera activa hasta que haya espacio en el buffer de escritura
            while (!HayEspacioParaTrama())
            {
                Thread.Sleep(1);
                if (detenerHilo || !sPuerto.IsOpen) return;
            }

            try
            {
                if (sPuerto.IsOpen && !detenerHilo)
                    sPuerto.Write(trama, 0, trama.Length);
            }
            catch { /* Puerto cerrado durante el envío — se ignora */ }
        }

        private void EnqueueAltaPrioridad(byte[] trama)
        {
            lock (bloqueoEnvio)
            {
                colaAltaPrioridad.Enqueue(trama);
            }
        }

        // ─────────────────────────────────────────────────────────────────
        //  RECEPCIÓN — ENRUTADOR POR TIPO
        // ─────────────────────────────────────────────────────────────────
        private void SPuerto_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                // Procesar TODAS las tramas completas disponibles en el buffer
                while (sPuerto.BytesToRead >= Trama.TAMANIO_TRAMA)
                {
                    byte[] buffer = new byte[Trama.TAMANIO_TRAMA];
                    sPuerto.Read(buffer, 0, buffer.Length);

                    Paquete paquete = gestorTrama.extraerPaquete(buffer);

                    // Enrutar según el tipo de trama recibida
                    switch (paquete.Tipo)
                    {
                        case 'M': ProcesarMensaje(paquete); break;
                        case 'I': ProcesarInicioArchivo(paquete); break;
                        case 'A': ProcesarFragmento(paquete); break;
                        case 'F': ProcesarFinArchivo(paquete); break;
                        default:
                            // Trama desconocida: ignorar silenciosamente
                            break;
                    }
                }
            }
            catch { /* Error de lectura — se descarta silenciosamente */ }
        }

        // ── Handlers de recepción ─────────────────────────────────────────

        private void ProcesarMensaje(Paquete paquete)
        {
            string mensaje = Encoding.UTF8.GetString(paquete.Datos, 0, paquete.LongitudDatos);
            llegoMensaje?.Invoke(mensaje);
        }

        private void ProcesarInicioArchivo(Paquete paquete)
        {
            int id = paquete.ID;
            string nombreOriginal = Encoding.UTF8.GetString(
                paquete.Datos, 0, paquete.LongitudDatos);

            // Crear archivo temporal en la carpeta de descargas del usuario
            string carpeta = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            carpeta = Path.Combine(carpeta, "Downloads");
            string rutaTemp = Path.Combine(carpeta, $"canal_{id}_temp.bin");

            // Si ya había un archivo en ese canal, cerrar y descartar el anterior
            if (archivosRecibiendo.ContainsKey(id))
            {
                archivosRecibiendo[id].Stream.Close();
                archivosRecibiendo.Remove(id);
            }

            FileStream fs = new FileStream(
                rutaTemp, FileMode.Create, FileAccess.Write, FileShare.None);

            archivosRecibiendo[id] = new ArchivoRecepcion
            {
                Stream = fs,
                RutaTemp = rutaTemp,
                NombreOriginal = nombreOriginal,
                Carpeta = carpeta
            };
        }

        private void ProcesarFragmento(Paquete paquete)
        {
            int id = paquete.ID;

            if (!archivosRecibiendo.ContainsKey(id)) return; // Canal no iniciado

            archivosRecibiendo[id].Stream.Write(
                paquete.Datos, 0, paquete.LongitudDatos);
        }

        private void ProcesarFinArchivo(Paquete paquete)
        {
            int id = paquete.ID;

            if (!archivosRecibiendo.ContainsKey(id)) return;

            ArchivoRecepcion recepcion = archivosRecibiendo[id];
            recepcion.Stream.Close();
            archivosRecibiendo.Remove(id);

            // Mover de nombre temporal a nombre original (evitar colisiones)
            string rutaFinal = ObtenerRutaUnica(recepcion.Carpeta, recepcion.NombreOriginal);
            File.Move(recepcion.RutaTemp, rutaFinal);

            // Notificar a la UI
            llegoArchivo?.Invoke(rutaFinal);
        }

        // ─────────────────────────────────────────────────────────────────
        //  UTILIDADES
        // ─────────────────────────────────────────────────────────────────

        /// <summary>
        /// Genera una ruta de archivo única añadiendo un número
        /// si ya existe un archivo con el mismo nombre.
        /// </summary>
        private string ObtenerRutaUnica(string carpeta, string nombreArchivo)
        {
            string rutaCandidata = Path.Combine(carpeta, nombreArchivo);
            if (!File.Exists(rutaCandidata)) return rutaCandidata;

            string sinExtension = Path.GetFileNameWithoutExtension(nombreArchivo);
            string extension = Path.GetExtension(nombreArchivo);
            int contador = 1;

            do
            {
                rutaCandidata = Path.Combine(carpeta, $"{sinExtension}({contador}){extension}");
                contador++;
            }
            while (File.Exists(rutaCandidata));

            return rutaCandidata;
        }

        /// <summary>
        /// Cierra todos los FileStreams de envío abiertos y vacía las colas.
        /// Se llama al desconectar el puerto.
        /// </summary>
        private void LimpiarEstadoEnvio()
        {
            lock (bloqueoEnvio)
            {
                colaAltaPrioridad.Clear();

                foreach (var archivo in archivosActivos.Values)
                    archivo.Stream?.Close();

                archivosActivos.Clear();
                indiceRoundRobin = 0;
            }

            // También cerrar archivos en recepción incompleta
            foreach (var recepcion in archivosRecibiendo.Values)
                recepcion.Stream?.Close();

            archivosRecibiendo.Clear();
        }

        // ─────────────────────────────────────────────────────────────────
        //  CLASES AUXILIARES PRIVADAS
        // ─────────────────────────────────────────────────────────────────

        /// <summary>Contexto de un archivo que se está enviando.</summary>
        private class ArchivoEnvio
        {
            public FileStream Stream { get; set; }
            public int IdCanal { get; set; }
            public bool Terminado { get; set; }
        }

        /// <summary>Contexto de un archivo que se está recibiendo.</summary>
        private class ArchivoRecepcion
        {
            public FileStream Stream { get; set; }
            public string RutaTemp { get; set; }
            public string NombreOriginal { get; set; }
            public string Carpeta { get; set; }
        }
    }   
}