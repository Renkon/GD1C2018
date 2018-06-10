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

namespace FrbaHotel.Forms.AbmUsuario
{
    public partial class ViewerUsuarioForm : Form
    {
        private FormType type;
        private bool SoloActivos;
        private string ButtonText;

        public ViewerUsuarioForm(FormType type)
        {
            this.type = type;

            InitializeComponent();

            ApplyType();
            LoadComboboxes();

            PopulateDataGrid();
        }

        private void LoadComboboxes()
        {
            List<Rol> Roles = new RolDAO().ObtenerRoles();
            Rol dummyRole = new Rol(-1, "- ninguna seleccionada -", true, null);
            comboBox1.Items.Add(dummyRole);
            comboBox1.Items.AddRange(Roles.ToArray());

            comboBox1.SelectedIndex = 0;

            List<TipoDocumento> TiposDoc = new TipoDocumentoDAO().ObtenerTiposDocumento();
            TipoDocumento dummyType = new TipoDocumento(-1, "", " - vacío - ");

            comboBox2.Items.Add(dummyType);
            comboBox2.Items.AddRange(TiposDoc.ToArray());

            comboBox2.SelectedIndex = 0;

            List<Hotel> Hotels = new HotelDAO().ObtenerHoteles();
            Hotel dummyHotel = new Hotel(-1, " - ninguno seleccionado - ");

            comboBox3.Items.Add(dummyHotel);
            comboBox3.Items.AddRange(Hotels.ToArray());

            comboBox3.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;

            button2_Click(null, null);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PopulateDataGrid();
        }

        private void PopulateDataGrid()
        {
            List<Tuple<Usuario, Cuenta>> Users = new UsuarioDAO().ObtenerUsuariosFiltrado(textBox1.Text, (Rol)comboBox1.SelectedItem,
                textBox2.Text, textBox3.Text, (TipoDocumento)comboBox2.SelectedItem, Convert.ToInt64("0" + textBox4.Text), 
                textBox5.Text, (Hotel)comboBox3.SelectedItem, SoloActivos);

            dataGridView1.Rows.Clear();

            foreach (var tuple in Users)
            {
                var c = tuple.Item2;
                var u = tuple.Item1;
                dataGridView1.Rows.Add(c.Usuario, u.Nombre, u.Apellido, u.TipoDocumento.Sigla, u.Documento,
                    u.Correo, u.Teléfono, u.Dirección, u.FechaNacimiento.ToString("dd/MM/yyyy"), u.Estado ? "Activo" : "Deshabilitado");
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Tag = tuple;
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[10].Value = ButtonText;
            }
        }

        private void ApplyType()
        {
            switch (type)
            {
                case FormType.Modify:
                    SoloActivos = false;
                    ButtonText = "Modificar";
                    this.Text = "Visualizar usuarios para modificar";
                break;
                case FormType.Delete:
                    SoloActivos = true;
                    ButtonText = "Borrar";
                    this.Text = "Visualizar usuarios para borrar";
                break;
            }        
        }

        private void textBox4_Validating(object sender, CancelEventArgs e)
        {
            if (Regex.IsMatch(textBox4.Text, "[^0-9]"))
            {
                MessageBox.Show("El documento debe estar compuesto sólo por números!", "ERROR");
                textBox4.Focus();
            }
        }
    }
}
