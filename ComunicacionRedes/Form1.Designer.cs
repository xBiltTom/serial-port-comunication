namespace ComunicacionRedes
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.cbxPuertos = new System.Windows.Forms.ComboBox();
            this.cbxVelocidad = new System.Windows.Forms.ComboBox();
            this.rtbMensajes = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnActualizar = new System.Windows.Forms.Button();
            this.btnConectar = new System.Windows.Forms.Button();
            this.txtMensaje = new System.Windows.Forms.TextBox();
            this.btnEnviar = new System.Windows.Forms.Button();
            this.grpConexion = new System.Windows.Forms.Panel();
            this.btnDesconectar = new System.Windows.Forms.Button();
            this.grpConversacion = new System.Windows.Forms.Panel();
            this.grpMensaje = new System.Windows.Forms.Panel();
            this.btnLimpiar = new System.Windows.Forms.Button();
            this.lblEstadoTitulo = new System.Windows.Forms.Label();
            this.lblEstado = new System.Windows.Forms.Label();
            this.lblInfoTrama = new System.Windows.Forms.Label();
            this.btnEnviarArchivo = new System.Windows.Forms.Button();
            this.grpConexion.SuspendLayout();
            this.grpConversacion.SuspendLayout();
            this.grpMensaje.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbxPuertos
            // 
            this.cbxPuertos.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cbxPuertos.FormattingEnabled = true;
            this.cbxPuertos.Location = new System.Drawing.Point(56, 16);
            this.cbxPuertos.Name = "cbxPuertos";
            this.cbxPuertos.Size = new System.Drawing.Size(78, 23);
            this.cbxPuertos.TabIndex = 0;
            this.cbxPuertos.SelectedIndexChanged += new System.EventHandler(this.cbxPuertos_SelectedIndexChanged);
            // 
            // cbxVelocidad
            // 
            this.cbxVelocidad.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cbxVelocidad.FormattingEnabled = true;
            this.cbxVelocidad.Location = new System.Drawing.Point(201, 16);
            this.cbxVelocidad.Name = "cbxVelocidad";
            this.cbxVelocidad.Size = new System.Drawing.Size(78, 23);
            this.cbxVelocidad.TabIndex = 1;
            this.cbxVelocidad.SelectedIndexChanged += new System.EventHandler(this.cbxVelocidad_SelectedIndexChanged);
            // 
            // rtbMensajes
            // 
            this.rtbMensajes.BackColor = System.Drawing.Color.White;
            this.rtbMensajes.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbMensajes.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.rtbMensajes.Location = new System.Drawing.Point(9, 9);
            this.rtbMensajes.Name = "rtbMensajes";
            this.rtbMensajes.ReadOnly = true;
            this.rtbMensajes.Size = new System.Drawing.Size(557, 234);
            this.rtbMensajes.TabIndex = 2;
            this.rtbMensajes.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.label1.Location = new System.Drawing.Point(13, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "Puerto";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.label2.Location = new System.Drawing.Point(141, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "Velocidad";
            // 
            // btnActualizar
            // 
            this.btnActualizar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(117)))), ((int)(((byte)(117)))));
            this.btnActualizar.FlatAppearance.BorderSize = 0;
            this.btnActualizar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnActualizar.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.btnActualizar.ForeColor = System.Drawing.Color.White;
            this.btnActualizar.Location = new System.Drawing.Point(291, 13);
            this.btnActualizar.Name = "btnActualizar";
            this.btnActualizar.Size = new System.Drawing.Size(77, 26);
            this.btnActualizar.TabIndex = 5;
            this.btnActualizar.Text = "ACTUALIZAR";
            this.btnActualizar.UseVisualStyleBackColor = false;
            this.btnActualizar.Click += new System.EventHandler(this.btnActualizar_Click);
            // 
            // btnConectar
            // 
            this.btnConectar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(144)))), ((int)(((byte)(236)))));
            this.btnConectar.FlatAppearance.BorderSize = 0;
            this.btnConectar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConectar.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.btnConectar.ForeColor = System.Drawing.Color.White;
            this.btnConectar.Location = new System.Drawing.Point(377, 13);
            this.btnConectar.Name = "btnConectar";
            this.btnConectar.Size = new System.Drawing.Size(77, 26);
            this.btnConectar.TabIndex = 6;
            this.btnConectar.Text = "CONECTAR";
            this.btnConectar.UseVisualStyleBackColor = false;
            this.btnConectar.Click += new System.EventHandler(this.btnConectar_Click);
            // 
            // txtMensaje
            // 
            this.txtMensaje.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtMensaje.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtMensaje.Location = new System.Drawing.Point(9, 16);
            this.txtMensaje.Name = "txtMensaje";
            this.txtMensaje.Size = new System.Drawing.Size(394, 20);
            this.txtMensaje.TabIndex = 7;
            this.txtMensaje.TextChanged += new System.EventHandler(this.txtMensaje_TextChanged);
            this.txtMensaje.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtMensaje_KeyDown);
            // 
            // btnEnviar
            // 
            this.btnEnviar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(144)))), ((int)(((byte)(236)))));
            this.btnEnviar.FlatAppearance.BorderSize = 0;
            this.btnEnviar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEnviar.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.btnEnviar.ForeColor = System.Drawing.Color.White;
            this.btnEnviar.Location = new System.Drawing.Point(416, 13);
            this.btnEnviar.Name = "btnEnviar";
            this.btnEnviar.Size = new System.Drawing.Size(69, 26);
            this.btnEnviar.TabIndex = 8;
            this.btnEnviar.Text = "ENVIAR";
            this.btnEnviar.UseVisualStyleBackColor = false;
            this.btnEnviar.Click += new System.EventHandler(this.btnEnviar_Click);
            // 
            // grpConexion
            // 
            this.grpConexion.BackColor = System.Drawing.Color.White;
            this.grpConexion.Controls.Add(this.btnDesconectar);
            this.grpConexion.Controls.Add(this.btnConectar);
            this.grpConexion.Controls.Add(this.btnActualizar);
            this.grpConexion.Controls.Add(this.label2);
            this.grpConexion.Controls.Add(this.label1);
            this.grpConexion.Controls.Add(this.cbxVelocidad);
            this.grpConexion.Controls.Add(this.cbxPuertos);
            this.grpConexion.Location = new System.Drawing.Point(17, 9);
            this.grpConexion.Name = "grpConexion";
            this.grpConexion.Size = new System.Drawing.Size(574, 52);
            this.grpConexion.TabIndex = 9;
            // 
            // btnDesconectar
            // 
            this.btnDesconectar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(57)))), ((int)(((byte)(53)))));
            this.btnDesconectar.Enabled = false;
            this.btnDesconectar.FlatAppearance.BorderSize = 0;
            this.btnDesconectar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDesconectar.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.btnDesconectar.ForeColor = System.Drawing.Color.White;
            this.btnDesconectar.Location = new System.Drawing.Point(463, 13);
            this.btnDesconectar.Name = "btnDesconectar";
            this.btnDesconectar.Size = new System.Drawing.Size(94, 26);
            this.btnDesconectar.TabIndex = 7;
            this.btnDesconectar.Text = "DESCONECTAR";
            this.btnDesconectar.UseVisualStyleBackColor = false;
            this.btnDesconectar.Click += new System.EventHandler(this.btnDesconectar_Click);
            // 
            // grpConversacion
            // 
            this.grpConversacion.BackColor = System.Drawing.Color.White;
            this.grpConversacion.Controls.Add(this.rtbMensajes);
            this.grpConversacion.Location = new System.Drawing.Point(17, 69);
            this.grpConversacion.Name = "grpConversacion";
            this.grpConversacion.Size = new System.Drawing.Size(574, 251);
            this.grpConversacion.TabIndex = 10;
            // 
            // grpMensaje
            // 
            this.grpMensaje.BackColor = System.Drawing.Color.White;
            this.grpMensaje.Controls.Add(this.btnEnviarArchivo);
            this.grpMensaje.Controls.Add(this.btnLimpiar);
            this.grpMensaje.Controls.Add(this.btnEnviar);
            this.grpMensaje.Controls.Add(this.txtMensaje);
            this.grpMensaje.Location = new System.Drawing.Point(17, 329);
            this.grpMensaje.Name = "grpMensaje";
            this.grpMensaje.Size = new System.Drawing.Size(574, 90);
            this.grpMensaje.TabIndex = 11;
            // 
            // btnLimpiar
            // 
            this.btnLimpiar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(117)))), ((int)(((byte)(117)))));
            this.btnLimpiar.FlatAppearance.BorderSize = 0;
            this.btnLimpiar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLimpiar.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.btnLimpiar.ForeColor = System.Drawing.Color.White;
            this.btnLimpiar.Location = new System.Drawing.Point(493, 13);
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.Size = new System.Drawing.Size(64, 26);
            this.btnLimpiar.TabIndex = 9;
            this.btnLimpiar.Text = "LIMPIAR";
            this.btnLimpiar.UseVisualStyleBackColor = false;
            this.btnLimpiar.Click += new System.EventHandler(this.btnLimpiar_Click);
            // 
            // lblEstadoTitulo
            // 
            this.lblEstadoTitulo.AutoSize = true;
            this.lblEstadoTitulo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblEstadoTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.lblEstadoTitulo.Location = new System.Drawing.Point(22, 439);
            this.lblEstadoTitulo.Name = "lblEstadoTitulo";
            this.lblEstadoTitulo.Size = new System.Drawing.Size(45, 15);
            this.lblEstadoTitulo.TabIndex = 12;
            this.lblEstadoTitulo.Text = "Estado:";
            // 
            // lblEstado
            // 
            this.lblEstado.AutoSize = true;
            this.lblEstado.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblEstado.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(57)))), ((int)(((byte)(53)))));
            this.lblEstado.Location = new System.Drawing.Point(65, 439);
            this.lblEstado.Name = "lblEstado";
            this.lblEstado.Size = new System.Drawing.Size(86, 15);
            this.lblEstado.TabIndex = 13;
            this.lblEstado.Text = "Desconectado";
            // 
            // lblInfoTrama
            // 
            this.lblInfoTrama.AutoSize = true;
            this.lblInfoTrama.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblInfoTrama.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.lblInfoTrama.Location = new System.Drawing.Point(305, 439);
            this.lblInfoTrama.Name = "lblInfoTrama";
            this.lblInfoTrama.Size = new System.Drawing.Size(28, 15);
            this.lblInfoTrama.TabIndex = 14;
            this.lblInfoTrama.Text = "Info";
            this.lblInfoTrama.Click += new System.EventHandler(this.lblInfoTrama_Click);
            // 
            // btnEnviarArchivo
            // 
            this.btnEnviarArchivo.Location = new System.Drawing.Point(416, 55);
            this.btnEnviarArchivo.Name = "btnEnviarArchivo";
            this.btnEnviarArchivo.Size = new System.Drawing.Size(141, 23);
            this.btnEnviarArchivo.TabIndex = 10;
            this.btnEnviarArchivo.Text = "ENVIAR ARCHIVO";
            this.btnEnviarArchivo.UseVisualStyleBackColor = true;
            this.btnEnviarArchivo.Click += new System.EventHandler(this.btnEnviarArchivo_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(242)))), ((int)(((byte)(245)))));
            this.ClientSize = new System.Drawing.Size(609, 463);
            this.Controls.Add(this.lblInfoTrama);
            this.Controls.Add(this.lblEstado);
            this.Controls.Add(this.lblEstadoTitulo);
            this.Controls.Add(this.grpMensaje);
            this.Controls.Add(this.grpConversacion);
            this.Controls.Add(this.grpConexion);
            this.Name = "Form1";
            this.Text = "Chat Serial";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.grpConexion.ResumeLayout(false);
            this.grpConexion.PerformLayout();
            this.grpConversacion.ResumeLayout(false);
            this.grpMensaje.ResumeLayout(false);
            this.grpMensaje.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbxPuertos;
        private System.Windows.Forms.ComboBox cbxVelocidad;
        private System.Windows.Forms.RichTextBox rtbMensajes;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnActualizar;
        private System.Windows.Forms.Button btnConectar;
        private System.Windows.Forms.TextBox txtMensaje;
        private System.Windows.Forms.Button btnEnviar;
        private System.Windows.Forms.Panel grpConexion;
        private System.Windows.Forms.Panel grpConversacion;
        private System.Windows.Forms.Panel grpMensaje;
        private System.Windows.Forms.Label lblEstadoTitulo;
        private System.Windows.Forms.Label lblEstado;
        private System.Windows.Forms.Button btnDesconectar;
        private System.Windows.Forms.Label lblInfoTrama;
        private System.Windows.Forms.Button btnLimpiar;
        private System.Windows.Forms.Button btnEnviarArchivo;
    }
}

