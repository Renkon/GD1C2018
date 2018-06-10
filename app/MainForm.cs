using FrbaHotel.AbmRol;
using FrbaHotel.Database;
using FrbaHotel.Forms;
using FrbaHotel.Forms.AbmRol;
using FrbaHotel.Forms.AbmUsuario;
using FrbaHotel.Login;
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
            new RolForm(FormType.Add, null, null).ShowDialog();
        }

        private void modificarRolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ViewerRolForm(FormType.Modify).ShowDialog();
        }

        private void borrarRolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ViewerRolForm(FormType.Delete).ShowDialog();
        }

        private void nuevoUsuarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new UsuarioForm(FormType.Add, null, null).ShowDialog();
        }

        private void modificarUsuarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ViewerUsuarioForm(FormType.Modify).ShowDialog();
        }

        private void borrarUsuarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ViewerUsuarioForm(FormType.Delete).ShowDialog();
        }
    }
}
