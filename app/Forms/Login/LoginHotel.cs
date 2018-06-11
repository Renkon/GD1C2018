using FrbaHotel.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FrbaHotel.Forms.Login
{
    public partial class LoginHotel : Form
    {
        public Hotel Hotel { get; set; }

        public LoginHotel(List<Hotel> Hoteles)
        {
            InitializeComponent();

            // Completamos el combo
            comboBox1.Items.AddRange(Hoteles.ToArray());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Debe seleccionar un hotel!", "ERROR");
                return;
            }
            this.Hotel = (Hotel)comboBox1.SelectedItem;
            this.DialogResult = DialogResult.OK;
        }
    }
}
