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

namespace FrbaHotel.Forms.RegistrarConsumible
{
    public partial class ConsumibleForm : Form
    {
        private FormType type;
        private Estadia estadia;

        public Consumo Consumo { get; set; }

        public ConsumibleForm(FormType type, Estadia estadia, Consumo consumo)
        {
            this.type = type;
            this.estadia = estadia;
            this.Consumo = consumo;

            InitializeComponent();

            this.monthCalendar1.TodayDate = Config.GetInstance().GetCurrentDate();
            this.monthCalendar1.MinDate = estadia.Fecha_Inicio.Value;
            this.monthCalendar1.MaxDate = Config.GetInstance().GetCurrentDate();
            this.textBox2.Tag = Config.GetInstance().GetCurrentDate();
            this.textBox2.Text = Config.GetInstance().GetCurrentDate().ToString("dd/MM/yyyy");

            ApplyType();
            LoadHabitaciones();

            LoadContent();
        }

        private void LoadContent()
        {
            if (Consumo != null)
            {
                this.comboBox1.SelectedItem = Consumo.Habitacion;
                this.textBox1.Text = Consumo.Consumible.ToString();
                this.textBox1.Tag = Consumo.Consumible;
                this.textBox2.Tag = Consumo.Fecha;
                this.textBox2.Text = Consumo.Fecha.ToString("dd/MM/yyyy");
                this.monthCalendar1.SelectionStart = Consumo.Fecha;
                this.textBox3.Text = Consumo.Cantidad.ToString();
            }
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
            ConsumibleViewerForm Form = new ConsumibleViewerForm((Consumible) textBox1.Tag);
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

        private bool InputValido()
        {
            string ErrMsg = "";
            if (((Habitacion)comboBox1.SelectedItem).Id == -1)
                ErrMsg += "Debe seleccionar la habitación del consumo\n";
            if (textBox1.Text.Equals(""))
                ErrMsg += "Debe seleccionar un consumible\n";
            if (textBox3.Text.Equals(""))
                ErrMsg += "Debe insertar la cantidad del consumible\n";
            if (!textBox3.Text.Equals("") && Convert.ToInt32(textBox3.Text) < 1)
                ErrMsg += "Debe ingresar un número positivo en la cantidad\n";

            bool Valido = ErrMsg.Equals("");
            if (!Valido)
                MessageBox.Show(ErrMsg, "ERROR");
            return Valido;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!InputValido())
                return;

            Consumible consumible = (Consumible)textBox1.Tag;
            Habitacion habitacion = (Habitacion)comboBox1.SelectedItem;
            DateTime fecha = (DateTime)textBox2.Tag;
            int cantidad = Convert.ToInt32(textBox3.Text);

            switch (type)
            {
                case FormType.Add:
                    Consumo insertado = new ConsumoDAO().InsertarConsumo(consumible, estadia,
                        habitacion, fecha, cantidad);
                    if (insertado != null && insertado.Id != null)
                    {
                        this.Consumo = insertado;
                        this.DialogResult = DialogResult.OK;
                    }
                break;
                case FormType.Modify:
                    this.Consumo.Consumible = consumible;
                    this.Consumo.Habitacion = habitacion;
                    this.Consumo.Fecha = fecha;
                    this.Consumo.Cantidad = cantidad;

                    if (new ConsumoDAO().ModificarConsumo(this.Consumo))
                        this.DialogResult = DialogResult.OK;
                break;
            }
        }
    }
}
