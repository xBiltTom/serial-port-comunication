using System;
using System.IO.Ports;
using System.Threading;

namespace ComunicacionRedes
{
    internal class Comunicacion
    {
        private SerialPort sPuerto;
        private Trama gestorTrama;

        // HEBRA Y FLAG 
        private Thread hiloEnvio;
        private bool mensajePendiente;
        private byte[] tramaPendiente;
        private readonly object bloqueoEnvio = new object();
        private volatile bool detenerHilo;

        public event Action<string> llegoMensaje;

        public Comunicacion()
        {
            gestorTrama = new Trama();
            sPuerto = new SerialPort();
            sPuerto.DataReceived += SPuerto_DataReceived;
        }

        // PUERTOS DISPONIBLES 
        public string[] ObtenerPuertosDisponibles()
        {
            return SerialPort.GetPortNames();
        }

        // CONEXIÓN / DESCONEXIÓN 
        public void inicializaPuerto(string nombrePuerto, int velocidad)
        {
            cerrarPuerto();

            sPuerto.PortName = nombrePuerto;
            sPuerto.BaudRate = velocidad;
            sPuerto.DataBits = 8;
            sPuerto.Parity = Parity.Odd;
            sPuerto.StopBits = StopBits.Two;
            sPuerto.ReadBufferSize = 2048;
            sPuerto.WriteBufferSize = 3072;

            sPuerto.Open();

            // Lanzar la hebra al abrir el puerto
            IniciarHebraEnvio();
        }

        public void cerrarPuerto()
        {
            DetenerHebraEnvio();

            if (sPuerto.IsOpen)
                sPuerto.Close();
        }

        public bool estaConectado()
        {
            return sPuerto.IsOpen;
        }

        //  ENVÍO (INTERFAZ PÚBLICA) 
        public void enviarMensaje(string mensaje)
        {
            if (!sPuerto.IsOpen)
                throw new Exception("El puerto no está abierto");

            PrepararMensajePendiente(mensaje);
        }

        // MÉTODOS SEPARADOS (INTERNOS) 

        // 1. Iniciar la hebra de envío
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

        // 2. Bucle de la hebra (corre mientras el puerto esté abierto)
        private void BucleEnvio()
        {
            while (!detenerHilo && sPuerto != null && sPuerto.IsOpen)
            {
                bool hayMensaje = false;
                byte[] tramaAEnviar = null;

                lock (bloqueoEnvio)
                {
                    if (mensajePendiente)
                    {
                        hayMensaje = true;
                        tramaAEnviar = tramaPendiente;
                        mensajePendiente = false;
                        tramaPendiente = null;
                    }
                }

                if (hayMensaje && tramaAEnviar != null)
                {
                    EnviarTramaPendiente(tramaAEnviar);
                }
                else
                {
                    // Dormir para no saturar la CPU
                    Thread.Sleep(50);
                }
            }
        }

        // 3. Verificar si hay 1024 bytes libres en el buffer de escritura
        private bool HayEspacioParaTrama()
        {
            return sPuerto.WriteBufferSize - sPuerto.BytesToWrite >= Trama.TAMANIO_TRAMA;
        }

        // 4. Preparar el mensaje (solo construye la trama y activa el flag)
        private void PrepararMensajePendiente(string mensaje)
        {
            byte[] trama = gestorTrama.crearTramaMensaje(mensaje);

            lock (bloqueoEnvio)
            {
                tramaPendiente = trama;
                mensajePendiente = true;
            }
        }

        // 5. Esperar espacio y enviar la trama al puerto
        private void EnviarTramaPendiente(byte[] trama)
        {
            // Espera activa hasta que haya espacio para una trama completa
            while (!HayEspacioParaTrama())
            {
                Thread.Sleep(1);
                if (detenerHilo || !sPuerto.IsOpen)
                    return; // Salir si se cerró el puerto mientras esperábamos
            }

            // Enviar
            if (sPuerto.IsOpen && !detenerHilo)
            {
                try
                {
                    sPuerto.Write(trama, 0, trama.Length);
                }
                catch
                {
                    // Opcional: loggear error
                }
            }
        }

        // 6. Detener la hebra de envío de forma segura
        private void DetenerHebraEnvio()
        {
            detenerHilo = true;

            if (hiloEnvio != null && hiloEnvio.IsAlive)
            {
                hiloEnvio.Join(2000); // espera hasta 2 segundos
            }
        }

        //  RECEPCIÓN 
        private void SPuerto_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                if (sPuerto.BytesToRead >= Trama.TAMANIO_TRAMA)
                {
                    byte[] buffer = new byte[Trama.TAMANIO_TRAMA];
                    sPuerto.Read(buffer, 0, buffer.Length);
                    string mensaje = gestorTrama.extraerMensaje(buffer);
                    llegoMensaje?.Invoke(mensaje);
                }
            }
            catch
            {
                // Silencioso
            }
        }
    }
}