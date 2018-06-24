namespace FrbaHotel.AbmCliente
{
    partial class ClienteForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBoxNacionalidad = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.monthCalendarFechaNacimiento = new System.Windows.Forms.MonthCalendar();
            this.buttonFechaNacimiento = new System.Windows.Forms.Button();
            this.textBoxFechaNacimiento = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.comboBoxDni = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxTelefono = new System.Windows.Forms.TextBox();
            this.textBoxCorreo = new System.Windows.Forms.TextBox();
            this.textBoxDni = new System.Windows.Forms.TextBox();
            this.textBoxApellido = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxNombre = new System.Windows.Forms.TextBox();
            this.textBoxDireccionPiso = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.textBoxDireccionCalle = new System.Windows.Forms.TextBox();
            this.textBoxDireccionNro = new System.Windows.Forms.TextBox();
            this.textBoxDireccionDepartamento = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.comboBoxPais = new System.Windows.Forms.ComboBox();
            this.textBoxCiudad = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBoxEstado = new System.Windows.Forms.CheckBox();
            this.buttonAccion = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBoxNacionalidad);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.monthCalendarFechaNacimiento);
            this.groupBox1.Controls.Add(this.buttonFechaNacimiento);
            this.groupBox1.Controls.Add(this.textBoxFechaNacimiento);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.comboBoxDni);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.textBoxTelefono);
            this.groupBox1.Controls.Add(this.textBoxCorreo);
            this.groupBox1.Controls.Add(this.textBoxDni);
            this.groupBox1.Controls.Add(this.textBoxApellido);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBoxNombre);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(760, 235);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Datos del cliente";
            // 
            // textBoxNacionalidad
            // 
            this.textBoxNacionalidad.Location = new System.Drawing.Point(124, 201);
            this.textBoxNacionalidad.MaxLength = 255;
            this.textBoxNacionalidad.Name = "textBoxNacionalidad";
            this.textBoxNacionalidad.Size = new System.Drawing.Size(230, 20);
            this.textBoxNacionalidad.TabIndex = 29;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(46, 204);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(72, 13);
            this.label14.TabIndex = 28;
            this.label14.Text = "Nacionalidad:";
            // 
            // monthCalendarFechaNacimiento
            // 
            this.monthCalendarFechaNacimiento.Location = new System.Drawing.Point(327, 29);
            this.monthCalendarFechaNacimiento.MaxSelectionCount = 1;
            this.monthCalendarFechaNacimiento.Name = "monthCalendarFechaNacimiento";
            this.monthCalendarFechaNacimiento.TabIndex = 27;
            this.monthCalendarFechaNacimiento.Visible = false;
            this.monthCalendarFechaNacimiento.DateSelected += new System.Windows.Forms.DateRangeEventHandler(this.monthCalendarFechaNacimiento_DateSelected);
            this.monthCalendarFechaNacimiento.Leave += new System.EventHandler(this.monthCalendarFechaNacimiento_Leave);
            // 
            // buttonFechaNacimiento
            // 
            this.buttonFechaNacimiento.Location = new System.Drawing.Point(240, 165);
            this.buttonFechaNacimiento.Name = "buttonFechaNacimiento";
            this.buttonFechaNacimiento.Size = new System.Drawing.Size(75, 23);
            this.buttonFechaNacimiento.TabIndex = 26;
            this.buttonFechaNacimiento.Text = "Seleccionar";
            this.buttonFechaNacimiento.UseVisualStyleBackColor = true;
            this.buttonFechaNacimiento.Click += new System.EventHandler(this.buttonFechaNacimiento_Click);
            // 
            // textBoxFechaNacimiento
            // 
            this.textBoxFechaNacimiento.Enabled = false;
            this.textBoxFechaNacimiento.Location = new System.Drawing.Point(124, 167);
            this.textBoxFechaNacimiento.Name = "textBoxFechaNacimiento";
            this.textBoxFechaNacimiento.Size = new System.Drawing.Size(105, 20);
            this.textBoxFechaNacimiento.TabIndex = 25;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(9, 170);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(109, 13);
            this.label13.TabIndex = 22;
            this.label13.Text = "Fecha de nacimiento:";
            // 
            // comboBoxDni
            // 
            this.comboBoxDni.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDni.FormattingEnabled = true;
            this.comboBoxDni.Location = new System.Drawing.Point(124, 61);
            this.comboBoxDni.Name = "comboBoxDni";
            this.comboBoxDni.Size = new System.Drawing.Size(58, 21);
            this.comboBoxDni.TabIndex = 12;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(66, 134);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Telefono:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(89, 100);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Mail:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(53, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Documento:";
            // 
            // textBoxTelefono
            // 
            this.textBoxTelefono.Location = new System.Drawing.Point(124, 131);
            this.textBoxTelefono.MaxLength = 100;
            this.textBoxTelefono.Name = "textBoxTelefono";
            this.textBoxTelefono.Size = new System.Drawing.Size(579, 20);
            this.textBoxTelefono.TabIndex = 6;
            // 
            // textBoxCorreo
            // 
            this.textBoxCorreo.Location = new System.Drawing.Point(124, 97);
            this.textBoxCorreo.MaxLength = 255;
            this.textBoxCorreo.Name = "textBoxCorreo";
            this.textBoxCorreo.Size = new System.Drawing.Size(579, 20);
            this.textBoxCorreo.TabIndex = 5;
            // 
            // textBoxDni
            // 
            this.textBoxDni.Location = new System.Drawing.Point(188, 62);
            this.textBoxDni.MaxLength = 18;
            this.textBoxDni.Name = "textBoxDni";
            this.textBoxDni.Size = new System.Drawing.Size(166, 20);
            this.textBoxDni.TabIndex = 4;
            this.textBoxDni.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxDni_Validating);
            // 
            // textBoxApellido
            // 
            this.textBoxApellido.Location = new System.Drawing.Point(473, 26);
            this.textBoxApellido.MaxLength = 255;
            this.textBoxApellido.Name = "textBoxApellido";
            this.textBoxApellido.Size = new System.Drawing.Size(230, 20);
            this.textBoxApellido.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(420, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Apellido:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(71, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Nombre:";
            // 
            // textBoxNombre
            // 
            this.textBoxNombre.Location = new System.Drawing.Point(124, 26);
            this.textBoxNombre.MaxLength = 255;
            this.textBoxNombre.Name = "textBoxNombre";
            this.textBoxNombre.Size = new System.Drawing.Size(230, 20);
            this.textBoxNombre.TabIndex = 0;
            // 
            // textBoxDireccionPiso
            // 
            this.textBoxDireccionPiso.Location = new System.Drawing.Point(333, 60);
            this.textBoxDireccionPiso.MaxLength = 18;
            this.textBoxDireccionPiso.Name = "textBoxDireccionPiso";
            this.textBoxDireccionPiso.Size = new System.Drawing.Size(100, 20);
            this.textBoxDireccionPiso.TabIndex = 7;
            this.textBoxDireccionPiso.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxDireccionPiso_Validating);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(81, 34);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(33, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "Calle:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(67, 63);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(47, 13);
            this.label8.TabIndex = 14;
            this.label8.Text = "Número:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(297, 63);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(30, 13);
            this.label9.TabIndex = 15;
            this.label9.Text = "Piso:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(517, 63);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(77, 13);
            this.label10.TabIndex = 16;
            this.label10.Text = "Departamento:";
            // 
            // textBoxDireccionCalle
            // 
            this.textBoxDireccionCalle.Location = new System.Drawing.Point(124, 31);
            this.textBoxDireccionCalle.MaxLength = 255;
            this.textBoxDireccionCalle.Name = "textBoxDireccionCalle";
            this.textBoxDireccionCalle.Size = new System.Drawing.Size(576, 20);
            this.textBoxDireccionCalle.TabIndex = 17;
            // 
            // textBoxDireccionNro
            // 
            this.textBoxDireccionNro.Location = new System.Drawing.Point(124, 60);
            this.textBoxDireccionNro.MaxLength = 18;
            this.textBoxDireccionNro.Name = "textBoxDireccionNro";
            this.textBoxDireccionNro.Size = new System.Drawing.Size(100, 20);
            this.textBoxDireccionNro.TabIndex = 18;
            this.textBoxDireccionNro.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxDireccionNro_Validating);
            // 
            // textBoxDireccionDepartamento
            // 
            this.textBoxDireccionDepartamento.Location = new System.Drawing.Point(600, 60);
            this.textBoxDireccionDepartamento.MaxLength = 50;
            this.textBoxDireccionDepartamento.Name = "textBoxDireccionDepartamento";
            this.textBoxDireccionDepartamento.Size = new System.Drawing.Size(100, 20);
            this.textBoxDireccionDepartamento.TabIndex = 19;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(71, 93);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(43, 13);
            this.label11.TabIndex = 20;
            this.label11.Text = "Ciudad:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(81, 126);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(30, 13);
            this.label12.TabIndex = 21;
            this.label12.Text = "Pais:";
            // 
            // comboBoxPais
            // 
            this.comboBoxPais.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxPais.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxPais.FormattingEnabled = true;
            this.comboBoxPais.Location = new System.Drawing.Point(124, 123);
            this.comboBoxPais.Name = "comboBoxPais";
            this.comboBoxPais.Size = new System.Drawing.Size(576, 21);
            this.comboBoxPais.TabIndex = 23;
            this.comboBoxPais.Validating += new System.ComponentModel.CancelEventHandler(this.comboBoxPais_Validating);
            // 
            // textBoxCiudad
            // 
            this.textBoxCiudad.Location = new System.Drawing.Point(124, 90);
            this.textBoxCiudad.MaxLength = 255;
            this.textBoxCiudad.Name = "textBoxCiudad";
            this.textBoxCiudad.Size = new System.Drawing.Size(576, 20);
            this.textBoxCiudad.TabIndex = 24;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBoxDireccionPiso);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.textBoxCiudad);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.comboBoxPais);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.textBoxDireccionCalle);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.textBoxDireccionNro);
            this.groupBox2.Controls.Add(this.textBoxDireccionDepartamento);
            this.groupBox2.Location = new System.Drawing.Point(12, 273);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(760, 167);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Datos de residencia";
            // 
            // checkBoxEstado
            // 
            this.checkBoxEstado.AutoSize = true;
            this.checkBoxEstado.Location = new System.Drawing.Point(354, 463);
            this.checkBoxEstado.Name = "checkBoxEstado";
            this.checkBoxEstado.Size = new System.Drawing.Size(106, 17);
            this.checkBoxEstado.TabIndex = 25;
            this.checkBoxEstado.Text = "Cliente habilitado";
            this.checkBoxEstado.UseVisualStyleBackColor = true;
            // 
            // buttonAccion
            // 
            this.buttonAccion.Location = new System.Drawing.Point(370, 516);
            this.buttonAccion.Name = "buttonAccion";
            this.buttonAccion.Size = new System.Drawing.Size(75, 23);
            this.buttonAccion.TabIndex = 2;
            this.buttonAccion.UseVisualStyleBackColor = true;
            this.buttonAccion.Click += new System.EventHandler(this.buttonAccion_Click);
            // 
            // ClienteForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.checkBoxEstado);
            this.Controls.Add(this.buttonAccion);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ClienteForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxNombre;
        private System.Windows.Forms.TextBox textBoxApellido;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxDireccionPiso;
        private System.Windows.Forms.TextBox textBoxTelefono;
        private System.Windows.Forms.TextBox textBoxCorreo;
        private System.Windows.Forms.TextBox textBoxDni;
        private System.Windows.Forms.TextBox textBoxCiudad;
        private System.Windows.Forms.ComboBox comboBoxPais;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBoxDireccionDepartamento;
        private System.Windows.Forms.TextBox textBoxDireccionNro;
        private System.Windows.Forms.TextBox textBoxDireccionCalle;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBoxDni;
        private System.Windows.Forms.MonthCalendar monthCalendarFechaNacimiento;
        private System.Windows.Forms.Button buttonFechaNacimiento;
        private System.Windows.Forms.TextBox textBoxFechaNacimiento;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBoxNacionalidad;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.CheckBox checkBoxEstado;
        private System.Windows.Forms.Button buttonAccion;
    }
}