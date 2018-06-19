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
    public partial class SeleccionarHabitacion : Form
    {
        public TipoHabitacion Tipo { get; private set; }

        public SeleccionarHabitacion()
        {
            InitializeComponent();

            LoadComboBox();
        }

        private void LoadComboBox()
        {
            List<TipoHabitacion> TiposHabitación = new TipoHabitacionDAO().ObtenerTiposHabitacion();
            TipoHabitacion Dummy = new TipoHabitacion(-1, " - seleccione un tipo de habitación - ", 0, 0);

            comboBox1.Items.Add(Dummy);
            comboBox1.Items.AddRange(TiposHabitación.ToArray());

            comboBox1.SelectedItem = Dummy;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Tipo = (TipoHabitacion) comboBox1.SelectedItem;

            if (Tipo.Id != -1)
            {
                textBox1.Text = Tipo.Huéspedes.ToString();
                textBox2.Text = Tipo.Porcentual.ToString();
            }
            else
            {
                textBox1.Text = "";
                textBox2.Text = "";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null || Tipo.Id == -1)
            {
                MessageBox.Show("Debe seleccionar un tipo de habitación!", "ERROR");
                return;
            }

            this.DialogResult = DialogResult.OK;
        }
    }
}
