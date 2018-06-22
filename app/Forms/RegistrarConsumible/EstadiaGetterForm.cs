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
    public partial class EstadiaGetterForm : Form
    {
        public Estadia Estadia { get; private set; }

        public EstadiaGetterForm()
        {
            InitializeComponent();
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            if (Regex.IsMatch(textBox1.Text, "[^0-9]"))
            {
                MessageBox.Show("La altura de la calle debe estar compuesta sólo por números!", "ERROR");
                textBox1.Focus();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Equals(""))
            {
                MessageBox.Show("Debe ingresar un código de estadía!", "ERROR");
                return;
            }

            this.Estadia = new EstadiaDAO().ObtenerEstadia(Convert.ToInt32(textBox1.Text));

            if (this.Estadia == null)
            {
                MessageBox.Show("El código de estadía ingresado es inválido", "ERROR");
                return;
            }

            if (this.Estadia.Consumos_Cerrados)
            {
                MessageBox.Show("Los consumos de esta estadía ya han sido cerrados", "INFO");
                return;
            }

            this.DialogResult = DialogResult.OK;
        }
    }
}
