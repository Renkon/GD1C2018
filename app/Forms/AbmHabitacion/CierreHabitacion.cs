using FrbaHotel.Model;
using FrbaHotel.Model.DAO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FrbaHotel.Forms.AbmHabitacion
{
    public partial class CierreHabitacion : Form
    {
        private Habitacion habitacion;

        public CierreHabitacion(Habitacion habitacion)
        {
            this.habitacion = habitacion;

            InitializeComponent();

            textBox1.Text = habitacion.Número.ToString();
            textBox5.Text = Session.Hotel.Nombre;
            monthCalendar1.MinDate = Config.GetInstance().GetCurrentDate();
            monthCalendar1.TodayDate = Config.GetInstance().GetCurrentDate();
            monthCalendar2.TodayDate = Config.GetInstance().GetCurrentDate();
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
            textBox2.Text = monthCalendar1.SelectionStart.ToString("dd/MM/yyyy");
            monthCalendar1.Visible = false;
            monthCalendar2.MinDate = monthCalendar1.SelectionStart;
        }

        private void monthCalendar2_DateSelected(object sender, DateRangeEventArgs e)
        {
            textBox3.Text = monthCalendar2.SelectionStart.ToString("dd/MM/yyyy");
            monthCalendar2.Visible = false;
            monthCalendar1.MaxDate = monthCalendar2.SelectionStart;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string Inicio = textBox2.Text;
            string Fin = textBox3.Text;
            string Descripción = textBox4.Text;
            bool ReservadaEnPeriodo;
            string ErrMsg = "";

            if (Inicio.Equals(""))
                ErrMsg += "Debe ingresar la fecha de inicio del cierre temporal\n";
            if (Fin.Equals(""))
                ErrMsg += "Debe ingresar la fecha de fin del cierre temporal\n";
            if (Descripción.Equals(""))
                ErrMsg += "Debe ingresar el motivo del cierre temporal\n";
            if (!Inicio.Equals("") && !Fin.Equals(""))
            {
                ReservadaEnPeriodo = new HabitacionDAO().IsHabitacionReservadaEnPeriodo(
                    DateTime.ParseExact(Inicio, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    DateTime.ParseExact(Fin, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    habitacion);
                if (ReservadaEnPeriodo)
                    ErrMsg += "La habitación tiene una reserva en ese período!";
            }

            if (!ErrMsg.Equals("")) // hay error
                MessageBox.Show(ErrMsg, "ERROR");
            else
            {
                // procesamos el ingreso
                CierreTemporalHabitacion Cierre = new CierreTemporalHabitacion(null,
                    DateTime.ParseExact(Inicio, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    DateTime.ParseExact(Fin, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    habitacion, Descripción);

                if (new CierreTemporalHabitacionDAO().InsertarCierreTemporalHabitacion(Cierre))
                    this.Close();
            }
        }
    }
}
