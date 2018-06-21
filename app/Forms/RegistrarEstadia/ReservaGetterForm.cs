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

namespace FrbaHotel.Forms.RegistrarEstadia
{
    public partial class ReservaGetterForm : Form
    {
        public Reserva Reserva { get; private set; }

        public ReservaGetterForm()
        {
            InitializeComponent();
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            if (Regex.IsMatch(textBox1.Text, "[^0-9]"))
            {
                MessageBox.Show("El código de reserva debe estar compuesto sólo por números!", "ERROR");
                textBox1.Focus();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Equals(""))
            {
                MessageBox.Show("Debe ingresar un número de reserva", "ERROR");
                return;
            }

            Reserva reserva = new ReservaDAO().ObtenerReservaAptaEstadia(Convert.ToInt32(textBox1.Text));

            if (reserva.Id == -1)
            {
                MessageBox.Show("Lo sentimos, es posible que el código de reserva sea inválido o "
                    + "que ya haya finalizado su estadía", "INFO");
                return;
            }

            this.Reserva = reserva;
            this.DialogResult = DialogResult.OK;
        }
    }
}
