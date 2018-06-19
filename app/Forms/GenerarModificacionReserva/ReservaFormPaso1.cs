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

namespace FrbaHotel.Forms.GenerarModificacionReserva
{
    public partial class ReservaFormPaso1 : Form
    {

        public ReservaFormPaso1(FormType type)
        {
            InitializeComponent();

            monthCalendar1.TodayDate = Config.GetInstance().GetCurrentDate();
            monthCalendar2.TodayDate = Config.GetInstance().GetCurrentDate();
            
            LoadData();

            if (Session.Hotel != null) // hay una sesión con hotel seleccionado
            {
                // No permitimos que cambie el hotel del combo.
                comboBox1.SelectedItem = Session.Hotel;
                comboBox1.Enabled = false;
            }
        }

        private void ValidateReserva(DateTime inicio, DateTime fin,
            Regimen regimen, Hotel hotel, List<TipoHabitacion> habitaciones)
        {
            // TODO: definir una estrategia para validar la reserva!
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (monthCalendar2.Visible)
                monthCalendar2.Visible = false;

            monthCalendar1.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (monthCalendar1.Visible)
                monthCalendar1.Visible = false;

            monthCalendar2.Visible = true;
        }

        private void monthCalendar1_Leave(object sender, EventArgs e)
        {
            monthCalendar1.Visible = false;
        }

        private void monthCalendar2_Leave(object sender, EventArgs e)
        {
            monthCalendar2.Visible = false;
        }

        private void monthCalendar1_DateSelected(object sender, DateRangeEventArgs e)
        {
            textBox1.Text = monthCalendar1.SelectionStart.ToString("dd/MM/yyyy");
            monthCalendar1.Visible = false;
            monthCalendar2.MinDate = monthCalendar1.SelectionStart.AddDays(1);

            textBox1.Tag = monthCalendar1.SelectionStart;
        }

        private void monthCalendar2_DateSelected(object sender, DateRangeEventArgs e)
        {
            textBox2.Text = monthCalendar2.SelectionStart.ToString("dd/MM/yyyy");
            monthCalendar2.Visible = false;
            monthCalendar1.MaxDate = monthCalendar2.SelectionStart.AddDays(-1);

            textBox2.Tag = monthCalendar2.SelectionStart;
        }

        private void LoadData()
        {
            Regimen DummyReg = new Regimen(-1, " - Seleccione el régimen - ", 0, true);

            comboBox2.Items.Add(DummyReg);

            if (Session.Hotel != null)
            {
                List<Regimen> Regimenes = new RegimenDAO().ObtenerRegimenesActivosDeHotel(Session.Hotel);
                comboBox2.Items.AddRange(Regimenes.ToArray());
            }
            
            comboBox2.SelectedItem = DummyReg;

            List<Hotel> Hoteles = new HotelDAO().ObtenerHoteles();
            Hotel Dummy = new Hotel(-1, " - Seleccione un hotel - ");
            comboBox1.Items.Add(Dummy);
            comboBox1.Items.AddRange(Hoteles.ToArray());
            comboBox1.SelectedItem = Dummy;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SeleccionarHabitacion FormSeleccion = new SeleccionarHabitacion();

            if (FormSeleccion.ShowDialog() == DialogResult.OK)
            {
                TipoHabitacion selected = FormSeleccion.Tipo;
                dataGridView2.Rows.Add(selected.Descripción, selected.Huéspedes, selected.Porcentual + "%");
                dataGridView2.Rows[dataGridView2.Rows.Count - 1].Tag = FormSeleccion.Tipo;
                dataGridView2.Rows[dataGridView2.Rows.Count - 1].Cells[3].Value = "Eliminar habitación";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!InputValido())
                return;

            if (((Regimen)comboBox2.SelectedItem).Id == -1)
            {
                SeleccionarRegimen FormSelecc = new SeleccionarRegimen((Hotel) comboBox1.SelectedItem);
                if (FormSelecc.ShowDialog() == DialogResult.OK)
                {
                    comboBox2.SelectedItem = FormSelecc.Regimen;
                    FormSelecc.Close();
                    FormSelecc.Dispose();
                }
                else
                {
                    FormSelecc.Close();
                    FormSelecc.Dispose();
                    return;
                }
            }

            // Avanzo mostrando el precio final por día

            Regimen regimen = (Regimen)comboBox2.SelectedItem;
            Hotel hotel = (Hotel)comboBox1.SelectedItem;
            List<TipoHabitacion> habs = new List<TipoHabitacion>();

            StringBuilder Precio = new StringBuilder("Se muestra a continuación el monto diario:\n\n");
            Precio.Append("Regimen: ")
                .Append(regimen.Descripción)
                .Append("\n\n");

            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                TipoHabitacion tipo = (TipoHabitacion) row.Tag;
                habs.Add(tipo);

                double precioFinalDiario = GetPrecioFinalDiarioHabitación(regimen, hotel, tipo);
                   
                Precio.Append("US ")
                    .Append(precioFinalDiario.ToString("0.00"))
                    .Append(" final por día - ")
                    .Append(tipo.Descripción)
                    .Append(" para ")
                    .Append(tipo.Huéspedes)
                    .Append(" persona(s).\n");
            }

            Precio.Append("\n¿Desea continuar?");

            if (MessageBox.Show(Precio.ToString(), "ATENCIÓN!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                ValidateReserva((DateTime)textBox1.Tag, (DateTime)textBox2.Tag, regimen, hotel, habs);
            }
        }

        private double GetPrecioFinalDiarioHabitación(Regimen regimen, Hotel hotel, TipoHabitacion tipo)
        {
            return (regimen.PrecioBase * tipo.Huéspedes * (1 + (tipo.Porcentual / 100)))
                    + (hotel.Cantidad_Estrellas * hotel.Recarga_Por_Estrellas);
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0) // Clickeó el botón?
            {
                dataGridView2.Rows.RemoveAt(e.RowIndex);
            }
        }

        private bool InputValido()
        {
            Hotel Hotel = (Hotel) comboBox1.SelectedItem;
            string ErrMsg = "";
            if (textBox1.Text.Equals(""))
                ErrMsg += "Debe seleccionar una fecha de inicio de reserva\n";
            if (textBox2.Text.Equals(""))
                ErrMsg += "Debe seleccionar una fecha de finalización válida\n";
            if (dataGridView2.Rows.Count < 1)
                ErrMsg += "Debe seleccionar al menos una habitación\n";
            if (Hotel == null || Hotel.Id == -1)
                ErrMsg += "Debe seleccionar un hotel\n";

            bool Valido = true;
            if (!ErrMsg.Equals(""))
            {
                Valido = false;
                MessageBox.Show(ErrMsg, "ERROR");
            }
            return Valido;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();

            Regimen DummyReg = new Regimen(-1, " - Seleccione el régimen - ", 0, true);
            comboBox2.Items.Add(DummyReg);
            comboBox2.SelectedItem = DummyReg;

            if (comboBox1.SelectedIndex == 0)
                return;

            List<Regimen> Regimenes = new RegimenDAO().ObtenerRegimenesActivosDeHotel((Hotel) comboBox1.SelectedItem);
            comboBox2.Items.AddRange(Regimenes.ToArray());
        }
    }
}
