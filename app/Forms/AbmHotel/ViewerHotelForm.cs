using FrbaHotel.Model;
using FrbaHotel.Model.DAO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FrbaHotel.Forms.AbmHotel
{
    public partial class ViewerHotelForm : Form
    {
        private string ButtonText;
        private FormType type;

        public ViewerHotelForm(FormType type)
        {
            this.type = type;

            InitializeComponent();
            ApplyType();

            LoadComboboxes();

            PopulateDataGrid();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PopulateDataGrid();
        }

        private void PopulateDataGrid()
        {
            List<Hotel> Hoteles = new HotelDAO().ObtenerHotelesFiltrados(textBox1.Text,
                Convert.ToInt32(numericUpDown1.Value), textBox2.Text, (Pais) comboBox1.SelectedItem);

            dataGridView1.Rows.Clear();

            foreach (Hotel hotel in Hoteles)
            {
                dataGridView1.Rows.Add(hotel.Nombre, hotel.Correo, hotel.Teléfono, hotel.Ciudad.Trim(),
                    hotel.Domicilio_Calle + " " + hotel.Domicilio_Número, hotel.Cantidad_Estrellas,
                    hotel.País, hotel.Fecha_Creación.ToString("dd/MM/yyyy"), hotel.Recarga_Por_Estrellas);
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Tag = hotel;
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[9].Value = ButtonText;
            }
        }

        private void ApplyType()
        {
            switch (type)
            {
                case FormType.Modify:
                    ButtonText = "Modificar";
                    this.Text = "Modificación de hoteles";
                break;
                case FormType.Delete:
                    ButtonText = "Deshabilitar";
                    this.Text = "Deshabilitación de hoteles";
                break;
            }
        }

        private void LoadComboboxes()
        {
            List<Pais> paises = new PaisDAO().ObtenerPaises();
            comboBox1.Items.AddRange(paises.ToArray());
        }

        public void RefreshGrid()
        {
            PopulateDataGrid();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            comboBox1.SelectedItem = null;
            numericUpDown1.Value = 0;

            button2_Click(null, null);
        }

        private void comboBox1_Validating(object sender, CancelEventArgs e)
        {
            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Debe seleccionar un país válido!", "ERROR");
                comboBox1.Focus();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0) // Clickeó el botón?
            {
                if (type == FormType.Modify)
                    new HotelForm(type, (Hotel)dataGridView1.SelectedRows[0].Tag, this).ShowDialog();
                else
                    new CierreHotel((Hotel)dataGridView1.SelectedRows[0].Tag).ShowDialog();
            }
        }
    }
}
