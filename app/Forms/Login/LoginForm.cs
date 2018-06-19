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

namespace FrbaHotel.Forms.Login
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Usuario User = new UsuarioDAO().ObtenerUsuarioLogin(textBox1.Text, textBox2.Text);
            Hotel Hotel = null;
            Rol Rol = null;

            if (User.Id == -3)
                MessageBox.Show("El usuario NO existe en el sistema", "ERROR");
            else if (User.Id == -2)
                MessageBox.Show("El usuario se encuentra DESHABILITADO", "ERROR");
            else if (User.Id == -1)
                MessageBox.Show("Las credenciales ingresadas son INVÁLIDAS", "ERROR");
            else // Sesión Iniciada
            {
                User.Roles = User.Roles.Where(rol => rol.Estado == true).ToList();

                if (User.Hoteles.Count == 0 || User.Roles.Count == 0)
                    MessageBox.Show("Su usuario no tiene un hotel o un rol habilitados. Solicite ayuda con el administrador", "ERROR");
                else // Caso feliz.
                {
                    if (User.Roles.Count == 1)
                        Rol = User.Roles[0];
                    else
                    {
                        LoginRol Form = new LoginRol(User.Roles);
                        Form.ShowDialog();
                        Rol = Form.Rol;
                        Form.Close();
                        Form.Dispose();
                    }

                    if (User.Hoteles.Count == 1)
                        Hotel = User.Hoteles[0];
                    else
                    {
                        LoginHotel Form = new LoginHotel(User.Hoteles);
                        Form.ShowDialog();
                        Hotel = Form.Hotel;
                        Form.Close();
                        Form.Dispose();
                    }

                    Session.Set(User, Rol, Hotel);
                    MessageBox.Show("Bienvenido " + User.Nombre + " " + User.Apellido + "\n"
                        + "Rol de sesión: " + Rol.Id + ". " + Rol.Nombre + "\n"
                        + "Hotel: " + Hotel.Id + ". " + Hotel.Nombre, "INFO");
                    this.Close();
                }
                
            }
        }
    }
}
