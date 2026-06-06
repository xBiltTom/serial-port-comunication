using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
            cargarVelocidades();
            cargarPuertos();
            ActualizarEstadoUI(false);
        }

        private void cargarVelocidades()
        {
            cbxVelocidad.Items.Clear();
            cbxVelocidad.Items.Add("9600");
            cbxVelocidad.Items.Add("19200");
            cbxVelocidad.Items.Add("38400");
            cbxVelocidad.Items.Add("57600");
            cbxVelocidad.Items.Add("115200");
            cbxVelocidad.SelectedItem = "115200";
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
            Invoke(mostrarMensaje, "Otro: " + mensaje);
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
    }
}
