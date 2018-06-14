using FrbaHotel.AbmCliente;
using FrbaHotel.Model;
using FrbaHotel.Model.DAO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FrbaHotel.Forms.AbmCliente
{
    public partial class ViewerClienteForm : Form
    {
        private FormType type;
        private bool SoloActivos;
        private string ButtonText;

        public ViewerClienteForm(FormType type)
        {
            this.type = type;

            InitializeComponent();

            ApplyType();
            LoadComboboxes();

            PopulateDataGrid();
        }

        private void LoadComboboxes()
        {
            List<TipoDocumento> TiposDocumento = new TipoDocumentoDAO().ObtenerTiposDocumento();
            comboBoxDni.Items.AddRange(TiposDocumento.ToArray());
            comboBoxDni.SelectedIndex = 0;
        }

        private void buttonLimpiar_Click(object sender, EventArgs e)
        {
            textBoxNombres.Text = "";
            textBoxApellidos.Text = "";
            textBoxDni.Text = "";
            textBoxCorreo.Text = "";
            comboBoxDni.SelectedIndex = 0;

            buttonBuscar_Click(null, null);
        }

        private void buttonBuscar_Click(object sender, EventArgs e)
        {
            PopulateDataGrid();
        }

        private void PopulateDataGrid()
        {
            List<Cliente> Clientes = new ClienteDAO().ObtenerClientesFiltrado(textBoxNombres.Text, textBoxApellidos.Text,
                (TipoDocumento)comboBoxDni.SelectedItem, Convert.ToInt64("0" + textBoxDni.Text), textBoxCorreo.Text, SoloActivos);

            dataGridView1.Rows.Clear();

            foreach (var cliente in Clientes)
            {
                dataGridView1.Rows.Add(cliente.Nombre, cliente.Apellido, cliente.TipoDocumento.Sigla, cliente.Documento,
                    cliente.Correo, cliente.Telefono, cliente.Calle, cliente.Nro, cliente.Piso, cliente.Departamento, cliente.Ciudad,
                    cliente.Pais, cliente.Nacionalidad, cliente.FechaNacimiento.ToString("dd/MM/yyyy"), cliente.Estado ? "Activo" : "Deshabilitado");
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Tag = cliente;
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[15].Value = ButtonText;
            }
        }

        public void RefreshGrid()
        {
            PopulateDataGrid();
        }

        private void ApplyType()
        {
            switch (type)
            {
                case FormType.Modify:
                    SoloActivos = false;
                    ButtonText = "Modificar";
                    this.Text = "Visualizar clientes para modificar";
                    break;
                case FormType.Delete:
                    SoloActivos = true;
                    ButtonText = "Borrar";
                    this.Text = "Visualizar clientes para borrar";
                    break;
            }
        }

        private void textBoxDni_Validating(object sender, CancelEventArgs e)
        {
            if (Regex.IsMatch(textBoxDni.Text, "[^0-9]"))
            {
                MessageBox.Show("El documento debe estar compuesto sólo por números!", "ERROR");
                textBoxDni.Focus();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0) // Clickeó el botón?
            {
                new ClienteForm(type, (Cliente)dataGridView1.SelectedRows[0].Tag, this).ShowDialog();
            }
        }
    }
}
