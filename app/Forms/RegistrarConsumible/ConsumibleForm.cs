using FrbaHotel.Model;
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

namespace FrbaHotel.Forms.RegistrarConsumible
{
    public partial class ConsumibleForm : Form
    {
        private FormType type;
        private Estadia estadia;
        private Consumo consumo;

        public ConsumibleForm(FormType type, Estadia estadia, Consumo consumo)
        {
            this.type = type;
            this.estadia = estadia;
            this.consumo = consumo;

            InitializeComponent();

            this.monthCalendar1.TodayDate = Config.GetInstance().GetCurrentDate();
            this.monthCalendar1.MinDate = estadia.Fecha_Inicio.Value;
            this.monthCalendar1.MaxDate = Config.GetInstance().GetCurrentDate();

            ApplyType();
            LoadHabitaciones();
        }

        private void LoadHabitaciones()
        {
            Habitacion Dummy = new Habitacion(-1);

            this.comboBox1.Items.Add(Dummy);
            this.comboBox1.Items.AddRange(estadia.Habitaciones.ToArray());

            this.comboBox1.SelectedItem = Dummy;
        }

        private void ApplyType()
        {
            switch (type)
            {
                case FormType.Add:
                    this.Text = "Agregando consumo";
                    this.button3.Text = "Agregar consumo";
                break;
                case FormType.Modify:
                    this.Text = "Modificando consumo";
                    this.button3.Text = "Modificar consumo";
                break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ConsumibleViewerForm Form = new ConsumibleViewerForm();
            if (Form.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = Form.Consumible.ToString();
                textBox1.Tag = Form.Consumible;
            }
            Form.Close();
            Form.Dispose();
        }

        private void textBox3_Validating(object sender, CancelEventArgs e)
        {
            if (Regex.IsMatch(textBox3.Text, "[^0-9]"))
            {
                MessageBox.Show("La cantidad debe estar compuesta sólo por números!", "ERROR");
                textBox3.Focus();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.monthCalendar1.Visible = true;
        }

        private void monthCalendar1_Leave(object sender, EventArgs e)
        {
            monthCalendar1.Visible = false;
        }

        private void monthCalendar1_DateSelected(object sender, DateRangeEventArgs e)
        {
            textBox2.Text = monthCalendar1.SelectionStart.ToString("dd/MM/yyyy");
            textBox2.Tag = monthCalendar1.SelectionStart;
            monthCalendar1.Visible = false;
        }
    }
}
