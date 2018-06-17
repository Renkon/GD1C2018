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

namespace FrbaHotel.Forms.AbmHabitacion
{
    public partial class ViewerHabitacionForm : Form
    {
        private FormType type;
        private string ButtonText;

        public ViewerHabitacionForm(FormType type)
        {
            this.type = type;

            InitializeComponent();
            ApplyType();

            LoadComboboxes();

            label4.Text = label4.Text.Replace("${hotel}", Session.Hotel.Nombre);

            PopulateDataGrid();
        }

        private void LoadComboboxes()
        {
            List<TipoHabitacion> Tipos = new TipoHabitacionDAO().ObtenerTiposHabitacion();
            TipoHabitacion Dummy = new TipoHabitacion(-1, " - seleccione - ", 0, 0);

            comboBox1.Items.Add(Dummy);
            comboBox1.Items.AddRange(Tipos.ToArray());

            comboBox1.SelectedItem = Dummy;
        }

        private void PopulateDataGrid()
        {
            List<Habitacion> Habitaciones = new HabitacionDAO().ObtenerHabitacionesFiltradas(Session.Hotel,
                textBox1.Text, textBox2.Text, (TipoHabitacion) comboBox1.SelectedItem);

            dataGridView1.Rows.Clear();

            foreach (Habitacion habitacion in Habitaciones)
            {
                dataGridView1.Rows.Add(habitacion.Número, habitacion.Piso, habitacion.Ubicación,
                    habitacion.TipoHabitación, habitacion.Descripción);
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Tag = habitacion;
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[5].Value = ButtonText;
            }
        }

        private void ApplyType()
        {
            switch (type)
            {
                case FormType.Modify:
                    ButtonText = "Modificar";
                    this.Text = "Modificación de habitación";
                break;
                case FormType.Delete:
                    ButtonText = "Deshabilitar";
                    this.Text = "Deshabilitación de habitación";
                break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            comboBox1.SelectedIndex = 0;

            button2_Click(null, null);
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            if (Regex.IsMatch(textBox1.Text, "[^0-9]"))
            {
                MessageBox.Show("El número de habitación debe estar compuesto sólo por números!", "ERROR");
                textBox1.Focus();
            }
        }

        private void textBox2_Validating(object sender, CancelEventArgs e)
        {
            if (Regex.IsMatch(textBox1.Text, "[^0-9]"))
            {
                MessageBox.Show("El piso de la habitación debe estar compuesto sólo por números!", "ERROR");
                textBox1.Focus();
            }
        }

        public void RefreshGrid()
        {
            PopulateDataGrid();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PopulateDataGrid();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0) // Clickeó el botón?
            {
                if (type == FormType.Modify)
                    new HabitacionForm(type, (Habitacion)dataGridView1.SelectedRows[0].Tag, this).ShowDialog();
                else
                    new CierreHabitacion((Habitacion)dataGridView1.SelectedRows[0].Tag).ShowDialog();
            }
        }
    }
}
