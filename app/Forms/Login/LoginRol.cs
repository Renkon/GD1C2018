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
    public partial class LoginRol : Form
    {
        public Rol Rol { get; set; }
            
        public LoginRol(List<Rol> Roles)
        {
            InitializeComponent();

            // Completamos el combo
            comboBox1.Items.AddRange(Roles.ToArray());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Debe seleccionar un rol!", "ERROR");
                return;
            }
            this.Rol = (Rol)comboBox1.SelectedItem;
            this.DialogResult = DialogResult.OK;
        }
    }
}
