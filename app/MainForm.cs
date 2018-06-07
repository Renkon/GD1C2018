using FrbaHotel.AbmRol;
using FrbaHotel.Database;
using FrbaHotel.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FrbaHotel
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            DatabaseConnection.GetInstance().TestConnection();

            // Seteamos la sesión de guest
            Session.InitGuest();

            InitializeComponent();
        }

        private void nuevoRolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new RolForm(FormType.Add, null).ShowDialog();
        }

        private void modificarRolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new RolForm(FormType.Modify, null).ShowDialog();
        }

        private void borrarRolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new RolForm(FormType.Delete, null).ShowDialog();
        }
    }
}
