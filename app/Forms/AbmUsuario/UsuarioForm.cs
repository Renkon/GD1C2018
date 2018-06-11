using FrbaHotel;
using FrbaHotel.Forms;
using FrbaHotel.Forms.AbmUsuario;
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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FrbaHotel.Login
{
    public partial class UsuarioForm : Form
    {
        private FormType type;
        private Tuple<Usuario, Cuenta> usuario;
        private ViewerUsuarioForm parent;

        public UsuarioForm(FormType type, Tuple<Usuario, Cuenta> usuario, ViewerUsuarioForm parent)
        {
            this.type = type;
            this.usuario = usuario;
            this.parent = parent;

            InitializeComponent();

            this.monthCalendar1.MaxDate = Config.GetInstance().GetCurrentDate();
            this.monthCalendar1.TodayDate = Config.GetInstance().GetCurrentDate();

            ApplyType();
            PopulateLists();
            LoadContent();
        }

        // Se ejecuta cuando esta validando el textbox
        private void textBox5_Validating(object sender, CancelEventArgs e)
        {
            if (Regex.IsMatch(textBox5.Text, "[^0-9]"))
            {
                MessageBox.Show("El documento debe estar compuesto sólo por números!", "ERROR");
                textBox5.Focus();
            }
        }

        private void LoadContent()
        {
            if (usuario != null)
            {
                Cuenta c = usuario.Item2;
                Usuario u = usuario.Item1;
                this.textBox1.Text = c.Usuario;
                this.textBox3.Text = u.Nombre;
                this.textBox4.Text = u.Apellido;
                this.comboBox1.SelectedItem = u.TipoDocumento;
                this.textBox5.Text = Convert.ToString(u.Documento);
                this.textBox6.Text = u.Correo;
                this.textBox7.Text = u.Teléfono;
                this.textBox8.Text = u.Dirección;
                this.textBox9.Text = u.FechaNacimiento.ToString("dd/MM/yyyy");
                this.checkBox1.Checked = u.Estado;
                this.checkBox1.Enabled = !u.Estado;
            }
        }

        private void PopulateLists()
        {
            List<TipoDocumento> TiposDocumento = new TipoDocumentoDAO().ObtenerTiposDocumento();
            comboBox1.Items.AddRange(TiposDocumento.ToArray());

            List<Rol> Roles = new RolDAO().ObtenerRoles();
            List<int> IdsRolesUsuario = usuario != null ?
                new RolDAO().ObtenerIdsRolesUsuario(usuario.Item1) : new List<int>();
            this.listBox1.Items.AddRange(Roles
                .Where(u => IdsRolesUsuario.Contains(u.Id.Value)).ToArray());
            this.listBox2.Items.AddRange(Roles
                .Where(u => !IdsRolesUsuario.Contains(u.Id.Value)).ToArray());

            List<Hotel> Hoteles = new HotelDAO().ObtenerHoteles();
            List<int> IdsHotelesUsuario = usuario != null ?
                new HotelDAO().ObtenerIdsHotelesUsuario(usuario.Item1) : new List<int>();
            this.listBox4.Items.AddRange(Hoteles
                .Where(u => IdsHotelesUsuario.Contains(u.Id.Value)).ToArray());
            this.listBox3.Items.AddRange(Hoteles
                .Where(u => !IdsHotelesUsuario.Contains(u.Id.Value)).ToArray());
        }

        // Se usa para el calendario
        private void button5_Click(object sender, EventArgs e)
        {
            monthCalendar1.Visible = true;
            monthCalendar1.Focus();
        }

        private void monthCalendar1_Leave(object sender, EventArgs e)
        {
            monthCalendar1.Visible = false;
        }

        private void monthCalendar1_DateSelected(object sender, DateRangeEventArgs e)
        {
            textBox9.Text = monthCalendar1.SelectionStart.ToString("dd/MM/yyyy");
            monthCalendar1.Visible = false;
        }

        private void ApplyType()
        {
            switch (type)
            {
                case FormType.Add:
                    button6.Text = "Registrar usuario";
                    this.Text = "Alta de usuario";
                    checkBox1.Visible = false;
                break;
                case FormType.Modify:
                    button6.Text = "Editar usuario";
                    this.Text = "Modificar usuario";
                    checkBox1.Checked = usuario.Item1.Estado;
                break;
                case FormType.Delete:
                    button6.Text = "Bloquear usuario";
                    this.Text = "Deshabilitar usuario";
                    textBox1.Enabled = false;
                    textBox2.Enabled = false;
                    textBox3.Enabled = false;
                    textBox4.Enabled = false;
                    textBox5.Enabled = false;
                    textBox6.Enabled = false;
                    textBox7.Enabled = false;
                    textBox8.Enabled = false;
                    checkBox1.Visible = false;
                    button1.Enabled = false;
                    button2.Enabled = false;
                    button3.Enabled = false;
                    button4.Enabled = false;
                    button5.Enabled = false;
                    comboBox1.Enabled = false;
                break;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MoveGeneric<Rol>(listBox2, listBox1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MoveGeneric<Rol>(listBox1, listBox2);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MoveGeneric<Hotel>(listBox3, listBox4);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MoveGeneric<Hotel>(listBox4, listBox3);
        }

        private void MoveGeneric<T>(ListBox list1, ListBox list2)
        {
            if (list1.SelectedIndex == -1)
            {
                MessageBox.Show("Debe seleccionar al menos un elemento", "ERROR");
                return; // Nada seleccionado
            }
            List<T> elems = list1.SelectedItems.Cast<T>().ToList();

            foreach (var e in elems)
            {
                list1.Items.Remove(e);
                list2.Items.Add(e);
            }
        }

        // Se ejecuta al darle al botón de acción
        private void button6_Click(object sender, EventArgs e)
        {
            string Cuenta = textBox1.Text;
            string Password = textBox2.Text;
            List<Rol> Roles = this.listBox1.Items.Cast<Rol>().ToList();
            List<Hotel> Hoteles = this.listBox4.Items.Cast<Hotel>().ToList();
            string Nombre = textBox3.Text;
            string Apellido = textBox4.Text;
            TipoDocumento TipoDocumento = (TipoDocumento) comboBox1.SelectedItem;
            string NumeroDocumento = textBox5.Text;
            string Correo = textBox6.Text;
            string Teléfono = textBox7.Text;
            string Dirección = textBox8.Text;
            string FechaNacimiento = textBox9.Text;
            bool Estado = checkBox1.Checked;

            switch (type)
            {
                case FormType.Add:
                    if (!InputValido(Cuenta, Password, Roles, Hoteles, Nombre, Apellido, TipoDocumento,
                        NumeroDocumento, Correo, Teléfono, Dirección, FechaNacimiento))
                        return;

                    Usuario NewUser = new Usuario(null, Nombre, Apellido, Roles, Hoteles, TipoDocumento, Convert.ToInt64(NumeroDocumento), Correo,
                        Teléfono, Dirección, DateTime.ParseExact(FechaNacimiento, "dd/MM/yyyy", CultureInfo.InvariantCulture), true);
                    Cuenta NewCuenta = new Cuenta(Cuenta, Password, 0);

                    if (new UsuarioDAO().InsertarNuevoUsuario(NewUser, NewCuenta))
                        this.Close();
                break;
                case FormType.Modify:
                    if (!InputValido(Cuenta, Password, Roles, Hoteles, Nombre, Apellido, TipoDocumento,
                        NumeroDocumento, Correo, Teléfono, Dirección, FechaNacimiento))
                        return;

                    // Validamos que el hotel de la sesión corresponda a uno del usuario.
                    List<Hotel> HotelesUser = new HotelDAO().ObtenerHotelesDeUsuario(usuario.Item1);
                    if (!HotelesUser.Contains(Session.Hotel))
                    {
                        MessageBox.Show("No puedes modificar este usuario. No está en tu hotel", "ERROR");
                        return;
                    }

                    Cuenta c = usuario.Item2;
                    Usuario u = usuario.Item1;
                    c.Usuario = Cuenta;
                    c.Contraseña = Password;
                    u.Roles = Roles;
                    u.Hoteles = Hoteles;
                    u.Nombre = Nombre;
                    u.Apellido = Apellido;
                    u.TipoDocumento = TipoDocumento;
                    u.Documento = Convert.ToInt64(NumeroDocumento);
                    u.Correo = Correo;
                    u.Teléfono = Teléfono;
                    u.Dirección = Dirección;
                    u.FechaNacimiento = DateTime.ParseExact(FechaNacimiento, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    u.Estado = Estado;

                    if (new UsuarioDAO().ModificarUsuario(u, c))
                    {
                        parent.RefreshGrid();
                        this.Close();
                    }
                break;
                case FormType.Delete:
                    // Validamos que el hotel de la sesión corresponda a uno del usuario.
                    List<Hotel> HotelesUser2 = new HotelDAO().ObtenerHotelesDeUsuario(usuario.Item1);
                    if (!HotelesUser2.Contains(Session.Hotel))
                    {
                        MessageBox.Show("No puedes modificar este usuario. No está en tu hotel", "ERROR");
                        return;
                    }

                    if (new UsuarioDAO().DeshabilitarUsuario(usuario.Item1, usuario.Item2))
                    {
                        parent.RefreshGrid();
                        this.Close();
                    }
                break;
            }
        }

        private bool InputValido(string Cuenta, string Password, List<Rol> Roles, List<Hotel> Hoteles,
            string Nombre, string Apellido, TipoDocumento TipoDocumento, string NumeroDocumento, string Correo,
            string Teléfono, string Dirección, string FechaNacimiento)
        {
            // pattern q matchea un email 
            // source: https://stackoverflow.com/questions/1365407/c-sharp-code-to-validate-email-address
            const string pattern = @"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$";
            string ErrMsg = "";
            if (Cuenta.Equals(""))
                ErrMsg += "Debe ingresar el nombre de su cuenta de usuario\n";
            if (Password.Length < 6)
            {
                if (!(type == FormType.Modify && Password.Equals("")))
                    ErrMsg += "Su contraseña debe tener al menos seis caracteres de longitud\n";
            }
            if (Roles.Count < 1)
                ErrMsg += "Debe seleccionar al menos un rol\n";
            if (Hoteles.Count < 1)
                ErrMsg += "Debe seleccionar al menos un hotel\n";
            if (Nombre.Equals(""))
                ErrMsg += "Debe ingresar el/los nombre/s real/es del usuario\n";
            if (Apellido.Equals(""))
                ErrMsg += "Debe ingresar el/los apellido/s real/es del usuario\n";
            if (TipoDocumento == null)
                ErrMsg += "Debe definir el tipo de documento\n";
            if (NumeroDocumento.Equals(""))
                ErrMsg += "Debe ingresar el número de documento\n";
            if (Correo.Equals("") || !Regex.IsMatch(Correo, pattern))
                ErrMsg += "Debe ingresar una dirección del correo electrónico válida\n";
            if (Teléfono.Equals(""))
                ErrMsg += "Debe ingresar un número de teléfono\n";
            if (Dirección.Equals(""))
                ErrMsg += "Debe ingresar una dirección\n";
            if (FechaNacimiento.Equals(""))
                ErrMsg += "Debe seleccionar una fecha de nacimiento\n";

            bool Valido = ErrMsg.Equals("");
            if (!Valido)
                MessageBox.Show(ErrMsg, "ERROR");
            return Valido;
        }
    }
}
