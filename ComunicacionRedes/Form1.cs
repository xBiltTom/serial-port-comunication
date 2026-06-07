using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComunicacionRedes
{
    public partial class Form1 : Form
    {

        private Comunicacion enlace;
        private delegate void accesoControlRichTextBox(string msg);
        private accesoControlRichTextBox mostrarMensaje;

        public Form1()
        {
            InitializeComponent();

            enlace = new Comunicacion();

            mostrarMensaje = new accesoControlRichTextBox(mostrandoMensaje);
        }

        private void cbxPuertos_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            enlace.llegoMensaje += Enlace_llegoMensaje;
            enlace.llegoArchivo += Enlace_llegoArchivo;
            enlace.progresoEnvio += Enlace_progresoEnvio;
            enlace.envioCompletado += Enlace_envioCompletado;
            cargarVelocidades();
            cargarPuertos();
            ActualizarEstadoUI(false);
        }

        private void cargarVelocidades()
        {
            cbxVelocidad.Items.Clear();
            cbxVelocidad.Items.Clear();
            cbxVelocidad.Items.Add("9600");
            cbxVelocidad.Items.Add("19200");
            cbxVelocidad.Items.Add("38400");
            cbxVelocidad.Items.Add("57600");
            cbxVelocidad.Items.Add("115200");
            cbxVelocidad.Items.Add("1000000");   // NUEVO
            cbxVelocidad.Items.Add("2000000");   // NUEVO
            cbxVelocidad.SelectedItem = "1000000";
        }

        private void ActualizarInfoTrama()
        {
            string mensaje = txtMensaje.Text;
            int bytesMensaje = Encoding.UTF8.GetBytes(mensaje).Length;

            lblInfoTrama.Text = $"Trama fija: {Trama.TAMANIO_TRAMA} bytes | Mensaje: {bytesMensaje} / {Trama.TAMANIO_TRAMA - Trama.TAMANIO_CABECERA} bytes";
        }

        private void cargarPuertos()
        {
            cbxPuertos.Items.Clear();
            string[] puertos = enlace.ObtenerPuertosDisponibles();
            foreach(string puerto in puertos){
                cbxPuertos.Items.Add(puerto);
            }

            if (cbxPuertos.Items.Count > 0)
            {
                cbxPuertos.SelectedIndex = 0;
            } else
            {
                MessageBox.Show("No se encontraron puertos disponibles.");
            }

        }

        private void ActualizarEstadoUI(bool conectado, string puerto = "", int velocidad = 0)
        {
            if (conectado)
            {
                lblEstado.Text = $"Conectado a {puerto} - {velocidad} baudios";
                lblEstado.ForeColor = Color.FromArgb(76, 175, 80);
            }
            else
            {
                lblEstado.Text = "Desconectado";
                lblEstado.ForeColor = Color.FromArgb(229, 57, 53);
                lblInfoTrama.Text = $"Trama fija: {Trama.TAMANIO_TRAMA} bytes | Mensaje: 0 / {Trama.TAMANIO_TRAMA - Trama.TAMANIO_CABECERA} bytes";
            }

            cbxPuertos.Enabled = !conectado;
            cbxVelocidad.Enabled = !conectado;
            btnActualizar.Enabled = !conectado;
            btnConectar.Enabled = !conectado;

            txtMensaje.Enabled = conectado;
            btnEnviar.Enabled = conectado;
            btnDesconectar.Enabled = conectado;
        }

        private void Enlace_llegoMensaje(string mensaje)
        {
            // BeginInvoke: no bloquea el hilo DataReceived
            BeginInvoke(mostrarMensaje, "Otro: " + mensaje);
        }

        private void mostrandoMensaje(string mensaje)
        {
            AgregarLinea(mensaje, false);
        }

        private void AgregarLinea(string texto, bool esMio = false)
        {
            rtbMensajes.SelectionStart = rtbMensajes.TextLength;
            rtbMensajes.SelectionLength = 0;

            rtbMensajes.SelectionAlignment = esMio ? HorizontalAlignment.Right : HorizontalAlignment.Left;
            rtbMensajes.SelectionColor = esMio ? Color.FromArgb(51, 144, 236) : Color.FromArgb(60, 60, 60);

            rtbMensajes.AppendText(texto + Environment.NewLine);
            
            // Forzar scroll al final
            rtbMensajes.SelectionStart = rtbMensajes.Text.Length;
            rtbMensajes.ScrollToCaret();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            cargarPuertos();
        }

        private void btnConectar_Click(object sender, EventArgs e)
        {
            if (cbxPuertos.SelectedItem == null)
            {
                MessageBox.Show("Seleccione un puerto COM antes de conectar.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string puerto = cbxPuertos.SelectedItem.ToString();
            int velocidad = Convert.ToInt32(cbxVelocidad.SelectedItem);

            try
            {
                enlace.inicializaPuerto(puerto, velocidad);
                ActualizarEstadoUI(true, puerto, velocidad);
                txtMensaje.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ActualizarEstadoUI(false, puerto, velocidad);
            }
        }

        private void btnEnviar_Click(object sender, EventArgs e)
        {

            
            if (!enlace.estaConectado())
            {
                MessageBox.Show("No hay conexión activa. Conéctese primero.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string mensaje = txtMensaje.Text.Trim();
            if (mensaje.Length == 0)
            {
                MessageBox.Show("Escriba un mensaje antes de enviar.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            byte[] bytesMensaje = Encoding.UTF8.GetBytes(mensaje);
            if (bytesMensaje.Length > Trama.TAMANIO_TRAMA - Trama.TAMANIO_CABECERA)
            {
                MessageBox.Show($"El mensaje es demasiado largo. Máximo {Trama.TAMANIO_TRAMA - Trama.TAMANIO_CABECERA} bytes.",
                                "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                enlace.enviarMensaje(mensaje);
                AgregarLinea("Yo: " + mensaje, true);
                txtMensaje.Clear();
                txtMensaje.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al enviar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            enlace.cerrarPuerto();
        }

        private void btnDesconectar_Click(object sender, EventArgs e)
        {
            try
            {
                enlace.cerrarPuerto();
            }
            catch { /* ignorar */ }
            finally
            {
                ActualizarEstadoUI(false);
            }
        }

        private void txtMensaje_TextChanged(object sender, EventArgs e)
        {
            ActualizarInfoTrama();
        }

        private void txtMensaje_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                btnEnviar.PerformClick();
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            rtbMensajes.Clear();
        }

        private void lblInfoTrama_Click(object sender, EventArgs e)
        {

        }

        private void cbxVelocidad_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnEnviarArchivo_Click(object sender, EventArgs e)
        {
            if (!enlace.estaConectado())
            {
                MessageBox.Show("Conéctese primero.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (OpenFileDialog dialogo = new OpenFileDialog())
            {
                dialogo.Title = "Seleccionar archivos a enviar";
                dialogo.Filter = "Todos los archivos (*.*)|*.*";
                dialogo.Multiselect = true;

                if (dialogo.ShowDialog() != DialogResult.OK) return;

                // Validación estricta: ¿hay suficientes canales libres para TODOS los archivos?
                int canalesLibres = enlace.ObtenerCanalesDisponibles();
                int archivosSeleccionados = dialogo.FileNames.Length;

                if (archivosSeleccionados > canalesLibres)
                {
                    MessageBox.Show(
                        $"Seleccionó {archivosSeleccionados} archivo(s), pero solo quedan " +
                        $"{canalesLibres} canal(es) libre(s).\n\n" +
                        $"Reduzca la selección o espere a que terminen los envíos en curso.",
                        "Canales insuficientes", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Hay espacio: iniciar envío de cada archivo
                foreach (string ruta in dialogo.FileNames)
                {
                    try
                    {
                        enlace.iniciarEnvioArchivo(ruta);
                        AgregarLinea($"SISTEMA: Enviando [{Path.GetFileName(ruta)}]...", true);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al enviar [{Path.GetFileName(ruta)}]: " + ex.Message,
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                ActualizarLabelCanales();
            }
        }

        private void Enlace_llegoArchivo(string rutaTemp, string nombreOriginal)
        {
            // BeginInvoke: no bloquea el hilo DataReceived
            BeginInvoke(new Action(() =>
            {
                using (SaveFileDialog saveDialog = new SaveFileDialog())
                {
                    saveDialog.Title = "Guardar archivo recibido";
                    saveDialog.FileName = nombreOriginal;
                    saveDialog.Filter = "Todos los archivos (*.*)|*.*";

                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            // Si ya existe un archivo en el destino, eliminarlo primero
                            if (File.Exists(saveDialog.FileName))
                                File.Delete(saveDialog.FileName);

                            File.Move(rutaTemp, saveDialog.FileName);
                            AgregarLinea($"SISTEMA: ✔ Archivo recibido [{nombreOriginal}] → {saveDialog.FileName}");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error al guardar archivo: " + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            // Intentar limpiar el temporal
                            try { if (File.Exists(rutaTemp)) File.Delete(rutaTemp); } catch { }
                        }
                    }
                    else
                    {
                        // El usuario canceló: eliminar archivo temporal
                        try { if (File.Exists(rutaTemp)) File.Delete(rutaTemp); } catch { }
                        AgregarLinea($"SISTEMA: ✖ Archivo [{nombreOriginal}] descartado por el usuario.");
                    }
                }
            }));
        }

        // ── PROGRESO DE ENVÍO ──────────────────────────────────────────────

        private void Enlace_progresoEnvio(string nombreArchivo, int porcentaje)
        {
            if (InvokeRequired)
            {
                // BeginInvoke: no bloquea el BucleEnvio — el hilo sigue enviando tramas
                BeginInvoke(new Action(() => Enlace_progresoEnvio(nombreArchivo, porcentaje)));
                return;
            }

            string prefijo = nombreArchivo + " — ";

            // Buscar si ya existe una entrada para este archivo
            int indice = -1;
            for (int i = 0; i < lstProgreso.Items.Count; i++)
            {
                if (lstProgreso.Items[i].ToString().StartsWith(prefijo))
                {
                    indice = i;
                    break;
                }
            }

            string textoNuevo = $"{nombreArchivo} — {porcentaje}%";

            if (indice >= 0)
            {
                // Actualizar in-place sin parpadeo
                lstProgreso.Items[indice] = textoNuevo;
            }
            else
            {
                lstProgreso.Items.Add(textoNuevo);
            }

            ActualizarLabelCanales();
        }

        private void Enlace_envioCompletado(string nombreArchivo)
        {
            if (InvokeRequired)
            {
                // BeginInvoke: no bloquea el BucleEnvio al liberar un canal
                BeginInvoke(new Action(() => Enlace_envioCompletado(nombreArchivo)));
                return;
            }

            string prefijo = nombreArchivo + " — ";

            // Eliminar la entrada del ListBox
            for (int i = lstProgreso.Items.Count - 1; i >= 0; i--)
            {
                if (lstProgreso.Items[i].ToString().StartsWith(prefijo))
                {
                    lstProgreso.Items.RemoveAt(i);
                    break;
                }
            }

            AgregarLinea($"SISTEMA: ✔ Archivo enviado [{nombreArchivo}]", true);
            ActualizarLabelCanales();
        }

        private void ActualizarLabelCanales()
        {
            int libres = enlace.ObtenerCanalesDisponibles();
            lblCanalesLibres.Text = $"Envios en curso ({libres}/5 canales libres)";
        }
    }
}
