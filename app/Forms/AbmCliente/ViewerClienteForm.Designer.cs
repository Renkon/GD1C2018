namespace FrbaHotel.Forms.AbmCliente
{
    partial class ViewerClienteForm
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
            this.buttonBuscar = new System.Windows.Forms.Button();
            this.buttonLimpiar = new System.Windows.Forms.Button();
            this.textBoxCorreo = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxDni = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBoxDni = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxApellidos = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxNombres = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ColumnNombre = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnApellido = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnTipoDni = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnDni = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnCorreo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnTelefono = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnCalle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnNro = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnPiso = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnDepartamento = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnCiudad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnPais = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnNacionalidad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnFechaNacimiento = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnEstado = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnBoton = new System.Windows.Forms.DataGridViewButtonColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonBuscar);
            this.groupBox1.Controls.Add(this.buttonLimpiar);
            this.groupBox1.Controls.Add(this.textBoxCorreo);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.textBoxDni);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.comboBoxDni);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.textBoxApellidos);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.textBoxNombres);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(13, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(759, 164);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filtros de búsqueda";
            // 
            // buttonBuscar
            // 
            this.buttonBuscar.Location = new System.Drawing.Point(652, 134);
            this.buttonBuscar.Name = "buttonBuscar";
            this.buttonBuscar.Size = new System.Drawing.Size(100, 23);
            this.buttonBuscar.TabIndex = 15;
            this.buttonBuscar.Text = "Buscar";
            this.buttonBuscar.UseVisualStyleBackColor = true;
            this.buttonBuscar.Click += new System.EventHandler(this.buttonBuscar_Click);
            // 
            // buttonLimpiar
            // 
            this.buttonLimpiar.Location = new System.Drawing.Point(5, 134);
            this.buttonLimpiar.Name = "buttonLimpiar";
            this.buttonLimpiar.Size = new System.Drawing.Size(100, 23);
            this.buttonLimpiar.TabIndex = 14;
            this.buttonLimpiar.Text = "Limpiar";
            this.buttonLimpiar.UseVisualStyleBackColor = true;
            this.buttonLimpiar.Click += new System.EventHandler(this.buttonLimpiar_Click);
            // 
            // textBoxCorreo
            // 
            this.textBoxCorreo.Location = new System.Drawing.Point(165, 102);
            this.textBoxCorreo.MaxLength = 255;
            this.textBoxCorreo.Name = "textBoxCorreo";
            this.textBoxCorreo.Size = new System.Drawing.Size(559, 20);
            this.textBoxCorreo.TabIndex = 13;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(74, 105);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(86, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "Filtrar por correo:";
            // 
            // textBoxDni
            // 
            this.textBoxDni.Location = new System.Drawing.Point(523, 76);
            this.textBoxDni.MaxLength = 18;
            this.textBoxDni.Name = "textBoxDni";
            this.textBoxDni.Size = new System.Drawing.Size(201, 20);
            this.textBoxDni.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(355, 79);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(162, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Filtrar por número de documento:";
            // 
            // comboBoxDni
            // 
            this.comboBoxDni.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDni.FormattingEnabled = true;
            this.comboBoxDni.Location = new System.Drawing.Point(165, 76);
            this.comboBoxDni.Name = "comboBoxDni";
            this.comboBoxDni.Size = new System.Drawing.Size(77, 21);
            this.comboBoxDni.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 79);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(144, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Filtrar por tipo de documento:";
            // 
            // textBoxApellidos
            // 
            this.textBoxApellidos.Location = new System.Drawing.Point(165, 50);
            this.textBoxApellidos.MaxLength = 255;
            this.textBoxApellidos.Name = "textBoxApellidos";
            this.textBoxApellidos.Size = new System.Drawing.Size(559, 20);
            this.textBoxApellidos.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(63, 53);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(97, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Filtrar por apellidos:";
            // 
            // textBoxNombres
            // 
            this.textBoxNombres.Location = new System.Drawing.Point(165, 24);
            this.textBoxNombres.MaxLength = 255;
            this.textBoxNombres.Name = "textBoxNombres";
            this.textBoxNombres.Size = new System.Drawing.Size(559, 20);
            this.textBoxNombres.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(63, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Filtrar por nombres:";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnNombre,
            this.ColumnApellido,
            this.ColumnTipoDni,
            this.ColumnDni,
            this.ColumnCorreo,
            this.ColumnTelefono,
            this.ColumnCalle,
            this.ColumnNro,
            this.ColumnPiso,
            this.ColumnDepartamento,
            this.ColumnCiudad,
            this.ColumnPais,
            this.ColumnNacionalidad,
            this.ColumnFechaNacimiento,
            this.ColumnEstado,
            this.ColumnBoton});
            this.dataGridView1.Location = new System.Drawing.Point(13, 201);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(759, 347);
            this.dataGridView1.TabIndex = 2;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // ColumnNombre
            // 
            this.ColumnNombre.HeaderText = "Nombre";
            this.ColumnNombre.Name = "ColumnNombre";
            this.ColumnNombre.ReadOnly = true;
            // 
            // ColumnApellido
            // 
            this.ColumnApellido.HeaderText = "Apellido";
            this.ColumnApellido.Name = "ColumnApellido";
            this.ColumnApellido.ReadOnly = true;
            // 
            // ColumnTipoDni
            // 
            this.ColumnTipoDni.HeaderText = "Tipo Documento";
            this.ColumnTipoDni.Name = "ColumnTipoDni";
            this.ColumnTipoDni.ReadOnly = true;
            // 
            // ColumnDni
            // 
            this.ColumnDni.HeaderText = "Número de Documento";
            this.ColumnDni.Name = "ColumnDni";
            this.ColumnDni.ReadOnly = true;
            // 
            // ColumnCorreo
            // 
            this.ColumnCorreo.HeaderText = "Correo";
            this.ColumnCorreo.Name = "ColumnCorreo";
            this.ColumnCorreo.ReadOnly = true;
            // 
            // ColumnTelefono
            // 
            this.ColumnTelefono.HeaderText = "Teléfono";
            this.ColumnTelefono.Name = "ColumnTelefono";
            this.ColumnTelefono.ReadOnly = true;
            // 
            // ColumnCalle
            // 
            this.ColumnCalle.HeaderText = "Calle";
            this.ColumnCalle.Name = "ColumnCalle";
            this.ColumnCalle.ReadOnly = true;
            // 
            // ColumnNro
            // 
            this.ColumnNro.HeaderText = "Nro";
            this.ColumnNro.Name = "ColumnNro";
            this.ColumnNro.ReadOnly = true;
            // 
            // ColumnPiso
            // 
            this.ColumnPiso.HeaderText = "Piso";
            this.ColumnPiso.Name = "ColumnPiso";
            this.ColumnPiso.ReadOnly = true;
            // 
            // ColumnDepartamento
            // 
            this.ColumnDepartamento.HeaderText = "Departamento";
            this.ColumnDepartamento.Name = "ColumnDepartamento";
            this.ColumnDepartamento.ReadOnly = true;
            // 
            // ColumnCiudad
            // 
            this.ColumnCiudad.HeaderText = "Ciudad";
            this.ColumnCiudad.Name = "ColumnCiudad";
            this.ColumnCiudad.ReadOnly = true;
            // 
            // ColumnPais
            // 
            this.ColumnPais.HeaderText = "Pais";
            this.ColumnPais.Name = "ColumnPais";
            this.ColumnPais.ReadOnly = true;
            // 
            // ColumnNacionalidad
            // 
            this.ColumnNacionalidad.HeaderText = "Nacionalidad";
            this.ColumnNacionalidad.Name = "ColumnNacionalidad";
            this.ColumnNacionalidad.ReadOnly = true;
            // 
            // ColumnFechaNacimiento
            // 
            this.ColumnFechaNacimiento.HeaderText = "Fecha de Nacimiento";
            this.ColumnFechaNacimiento.Name = "ColumnFechaNacimiento";
            this.ColumnFechaNacimiento.ReadOnly = true;
            // 
            // ColumnEstado
            // 
            this.ColumnEstado.HeaderText = "Estado";
            this.ColumnEstado.Name = "ColumnEstado";
            this.ColumnEstado.ReadOnly = true;
            // 
            // ColumnBoton
            // 
            this.ColumnBoton.HeaderText = "Seleccionar";
            this.ColumnBoton.Name = "ColumnBoton";
            this.ColumnBoton.ReadOnly = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 179);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(725, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "ATENCIÓN! - La lista muestra las primeras cien ocurrencias con los filtros aplica" +
    "dos. Si no encuentra lo que busca, debe ser más específico con su filtro.";
            // 
            // ViewerClienteForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.groupBox1);
            this.Name = "ViewerClienteForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ViewerClienteForm";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonBuscar;
        private System.Windows.Forms.Button buttonLimpiar;
        private System.Windows.Forms.TextBox textBoxCorreo;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxDni;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboBoxDni;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxApellidos;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxNombres;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnNombre;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnApellido;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnTipoDni;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDni;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnCorreo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnTelefono;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnCalle;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnNro;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnPiso;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDepartamento;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnCiudad;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnPais;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnNacionalidad;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnFechaNacimiento;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnEstado;
        private System.Windows.Forms.DataGridViewButtonColumn ColumnBoton;
        private System.Windows.Forms.Label label1;
    }
}