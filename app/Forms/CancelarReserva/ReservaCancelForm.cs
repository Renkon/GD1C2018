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

namespace FrbaHotel.Forms.CancelarReserva
{
    public partial class ReservaCancelForm : Form
    {
        private Reserva reserva;

        public ReservaCancelForm(Reserva r)
        {
            reserva = r;

            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string Motivo = textBox1.Text;

            if (new ReservaDAO().CancelarReseva(reserva, Motivo))
                this.Close();
        }
    }
}
