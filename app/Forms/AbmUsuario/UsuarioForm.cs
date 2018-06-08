using FrbaHotel.Forms;
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

namespace FrbaHotel.Login
{
    public partial class UsuarioForm : Form
    {
        private FormType type;
        private Usuario usuario;

        public UsuarioForm(FormType type, Usuario usuario, Form parent)
        {
            this.type = type;
            this.usuario = usuario;

            InitializeComponent();

            PopulateLists();
            ApplyType(type);
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

        private void PopulateLists()
        {
            List<TipoDocumento> TiposDocumento = new TipoDocumentoDAO().ObtenerTiposDocumento();
            comboBox1.Items.AddRange(TiposDocumento.ToArray());

            List<Rol> Roles = new RolDAO().ObtenerRoles();
            List<int> IdsRolesUsuario = usuario != null ?
                new RolDAO().ObtenerIdsRolesUsuario(usuario) : new List<int>();
            this.listBox1.Items.AddRange(Roles
                .Where(u => IdsRolesUsuario.Contains(u.Id.Value)).ToArray());
            this.listBox2.Items.AddRange(Roles
                .Where(u => !IdsRolesUsuario.Contains(u.Id.Value)).ToArray());

            List<Hotel> Hoteles = new HotelDAO().ObtenerHoteles();
            List<int> IdsHotelesUsuario = usuario != null ?
                new HotelDAO().ObtenerIdsHotelesUsuario(usuario) : new List<int>();
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

        private void ApplyType(FormType type)
        {
            switch (type)
            {
                case FormType.Add:
                    button6.Text = "Registrar usuario";
                    checkBox1.Visible = false;
                break;
                case FormType.Modify:
                    button6.Text = "Editar usuario";
                    checkBox1.Checked = usuario.Estado;
                break;
                case FormType.Delete:
                    button6.Text = "Bloquear usuario";
                    textBox1.Enabled = false;
                    textBox2.Enabled = false;
                    textBox3.Enabled = false;
                    textBox4.Enabled = false;
                    textBox5.Enabled = false;
                    textBox6.Enabled = false;
                    textBox7.Enabled = false;
                    textBox8.Enabled = false;
                    checkBox1.Visible = false;
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
    }
}
