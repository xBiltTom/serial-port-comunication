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
            this.grpConexion = new System.Windows.Forms.GroupBox();
            this.btnDesconectar = new System.Windows.Forms.Button();
            this.grpConversacion = new System.Windows.Forms.GroupBox();
            this.grpMensaje = new System.Windows.Forms.GroupBox();
            this.lblEstadoTitulo = new System.Windows.Forms.Label();
            this.lblEstado = new System.Windows.Forms.Label();
            this.lblInfoTrama = new System.Windows.Forms.Label();
            this.btnLimpiar = new System.Windows.Forms.Button();
            this.grpConexion.SuspendLayout();
            this.grpConversacion.SuspendLayout();
            this.grpMensaje.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbxPuertos
            // 
            this.cbxPuertos.FormattingEnabled = true;
            this.cbxPuertos.Location = new System.Drawing.Point(60, 20);
            this.cbxPuertos.Name = "cbxPuertos";
            this.cbxPuertos.Size = new System.Drawing.Size(90, 21);
            this.cbxPuertos.TabIndex = 0;
            this.cbxPuertos.SelectedIndexChanged += new System.EventHandler(this.cbxPuertos_SelectedIndexChanged);
            // 
            // cbxVelocidad
            // 
            this.cbxVelocidad.FormattingEnabled = true;
            this.cbxVelocidad.Location = new System.Drawing.Point(227, 20);
            this.cbxVelocidad.Name = "cbxVelocidad";
            this.cbxVelocidad.Size = new System.Drawing.Size(90, 21);
            this.cbxVelocidad.TabIndex = 1;
            this.cbxVelocidad.SelectedIndexChanged += new System.EventHandler(this.cbxVelocidad_SelectedIndexChanged);
            // 
            // rtbMensajes
            // 
            this.rtbMensajes.Location = new System.Drawing.Point(19, 19);
            this.rtbMensajes.Name = "rtbMensajes";
            this.rtbMensajes.ReadOnly = true;
            this.rtbMensajes.Size = new System.Drawing.Size(592, 207);
            this.rtbMensajes.TabIndex = 2;
            this.rtbMensajes.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Puerto";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(167, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Velocidad";
            // 
            // btnActualizar
            // 
            this.btnActualizar.Location = new System.Drawing.Point(339, 18);
            this.btnActualizar.Name = "btnActualizar";
            this.btnActualizar.Size = new System.Drawing.Size(92, 23);
            this.btnActualizar.TabIndex = 5;
            this.btnActualizar.Text = "ACTUALIZAR";
            this.btnActualizar.UseVisualStyleBackColor = true;
            this.btnActualizar.Click += new System.EventHandler(this.btnActualizar_Click);
            // 
            // btnConectar
            // 
            this.btnConectar.Location = new System.Drawing.Point(446, 18);
            this.btnConectar.Name = "btnConectar";
            this.btnConectar.Size = new System.Drawing.Size(75, 23);
            this.btnConectar.TabIndex = 6;
            this.btnConectar.Text = "CONECTAR";
            this.btnConectar.UseVisualStyleBackColor = true;
            this.btnConectar.Click += new System.EventHandler(this.btnConectar_Click);
            // 
            // txtMensaje
            // 
            this.txtMensaje.Location = new System.Drawing.Point(16, 20);
            this.txtMensaje.Name = "txtMensaje";
            this.txtMensaje.Size = new System.Drawing.Size(461, 20);
            this.txtMensaje.TabIndex = 7;
            this.txtMensaje.TextChanged += new System.EventHandler(this.txtMensaje_TextChanged);
            this.txtMensaje.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtMensaje_KeyDown);
            // 
            // btnEnviar
            // 
            this.btnEnviar.Location = new System.Drawing.Point(493, 19);
            this.btnEnviar.Name = "btnEnviar";
            this.btnEnviar.Size = new System.Drawing.Size(60, 23);
            this.btnEnviar.TabIndex = 8;
            this.btnEnviar.Text = "ENVIAR";
            this.btnEnviar.UseVisualStyleBackColor = true;
            this.btnEnviar.Click += new System.EventHandler(this.btnEnviar_Click);
            // 
            // grpConexion
            // 
            this.grpConexion.Controls.Add(this.btnDesconectar);
            this.grpConexion.Controls.Add(this.btnConectar);
            this.grpConexion.Controls.Add(this.btnActualizar);
            this.grpConexion.Controls.Add(this.label2);
            this.grpConexion.Controls.Add(this.label1);
            this.grpConexion.Controls.Add(this.cbxVelocidad);
            this.grpConexion.Controls.Add(this.cbxPuertos);
            this.grpConexion.Location = new System.Drawing.Point(22, 12);
            this.grpConexion.Name = "grpConexion";
            this.grpConexion.Size = new System.Drawing.Size(648, 64);
            this.grpConexion.TabIndex = 9;
            this.grpConexion.TabStop = false;
            this.grpConexion.Text = "Conexión";
            // 
            // btnDesconectar
            // 
            this.btnDesconectar.Enabled = false;
            this.btnDesconectar.Location = new System.Drawing.Point(533, 19);
            this.btnDesconectar.Name = "btnDesconectar";
            this.btnDesconectar.Size = new System.Drawing.Size(96, 23);
            this.btnDesconectar.TabIndex = 7;
            this.btnDesconectar.Text = "DESCONECTAR";
            this.btnDesconectar.UseVisualStyleBackColor = true;
            this.btnDesconectar.Click += new System.EventHandler(this.btnDesconectar_Click);
            // 
            // grpConversacion
            // 
            this.grpConversacion.Controls.Add(this.rtbMensajes);
            this.grpConversacion.Location = new System.Drawing.Point(22, 88);
            this.grpConversacion.Name = "grpConversacion";
            this.grpConversacion.Size = new System.Drawing.Size(647, 250);
            this.grpConversacion.TabIndex = 10;
            this.grpConversacion.TabStop = false;
            this.grpConversacion.Text = "Conversación";
            // 
            // grpMensaje
            // 
            this.grpMensaje.Controls.Add(this.btnLimpiar);
            this.grpMensaje.Controls.Add(this.btnEnviar);
            this.grpMensaje.Controls.Add(this.txtMensaje);
            this.grpMensaje.Location = new System.Drawing.Point(22, 351);
            this.grpMensaje.Name = "grpMensaje";
            this.grpMensaje.Size = new System.Drawing.Size(646, 57);
            this.grpMensaje.TabIndex = 11;
            this.grpMensaje.TabStop = false;
            this.grpMensaje.Text = "Mensaje a enviar";
            // 
            // lblEstadoTitulo
            // 
            this.lblEstadoTitulo.AutoSize = true;
            this.lblEstadoTitulo.Location = new System.Drawing.Point(19, 420);
            this.lblEstadoTitulo.Name = "lblEstadoTitulo";
            this.lblEstadoTitulo.Size = new System.Drawing.Size(43, 13);
            this.lblEstadoTitulo.TabIndex = 12;
            this.lblEstadoTitulo.Text = "Estado:";
            // 
            // lblEstado
            // 
            this.lblEstado.AutoSize = true;
            this.lblEstado.ForeColor = System.Drawing.Color.Red;
            this.lblEstado.Location = new System.Drawing.Point(79, 420);
            this.lblEstado.Name = "lblEstado";
            this.lblEstado.Size = new System.Drawing.Size(77, 13);
            this.lblEstado.TabIndex = 13;
            this.lblEstado.Text = "Desconectado";
            // 
            // lblInfoTrama
            // 
            this.lblInfoTrama.AutoSize = true;
            this.lblInfoTrama.Location = new System.Drawing.Point(377, 420);
            this.lblInfoTrama.Name = "lblInfoTrama";
            this.lblInfoTrama.Size = new System.Drawing.Size(25, 13);
            this.lblInfoTrama.TabIndex = 14;
            this.lblInfoTrama.Text = "Info";
            this.lblInfoTrama.Click += new System.EventHandler(this.lblInfoTrama_Click);
            // 
            // btnLimpiar
            // 
            this.btnLimpiar.Location = new System.Drawing.Point(559, 20);
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.Size = new System.Drawing.Size(70, 20);
            this.btnLimpiar.TabIndex = 9;
            this.btnLimpiar.Text = "LIMPIAR";
            this.btnLimpiar.UseVisualStyleBackColor = true;
            this.btnLimpiar.Click += new System.EventHandler(this.btnLimpiar_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(718, 501);
            this.Controls.Add(this.lblInfoTrama);
            this.Controls.Add(this.lblEstado);
            this.Controls.Add(this.lblEstadoTitulo);
            this.Controls.Add(this.grpMensaje);
            this.Controls.Add(this.grpConversacion);
            this.Controls.Add(this.grpConexion);
            this.Name = "Form1";
            this.Text = "Form1";
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
        private System.Windows.Forms.GroupBox grpConexion;
        private System.Windows.Forms.GroupBox grpConversacion;
        private System.Windows.Forms.GroupBox grpMensaje;
        private System.Windows.Forms.Label lblEstadoTitulo;
        private System.Windows.Forms.Label lblEstado;
        private System.Windows.Forms.Button btnDesconectar;
        private System.Windows.Forms.Label lblInfoTrama;
        private System.Windows.Forms.Button btnLimpiar;
    }
}

