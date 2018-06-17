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

namespace FrbaHotel.Forms.AbmHabitacion
{
    public partial class HabitacionForm : Form
    {
        private FormType type;
        private Habitacion habitacion;
        private ViewerHabitacionForm parent;

        public HabitacionForm(FormType type, Habitacion habitacion, ViewerHabitacionForm parent)
        {
            this.type = type;
            this.habitacion = habitacion;
            this.parent = parent;

            InitializeComponent();
            ApplyType();

            PopulateLists();
            LoadContent();
        }

        private void LoadContent()
        {
            if (habitacion != null)
            {
                textBox1.Text = habitacion.Número.ToString();
                textBox2.Text = habitacion.Piso.ToString();
                checkBox1.Checked = habitacion.Ubicación.Equals("S");
                comboBox1.SelectedItem = habitacion.TipoHabitación;
                textBox4.Text = habitacion.Descripción;
            }
        }

        private void PopulateLists()
        {
            List<TipoHabitacion> Tipos = new TipoHabitacionDAO().ObtenerTiposHabitacion();
            TipoHabitacion Dummy = new TipoHabitacion(-1, " - seleccione - ", 0, 0);
            comboBox1.Items.Add(Dummy);
            comboBox1.Items.AddRange(Tipos.ToArray());

            comboBox1.SelectedItem = Dummy;
        }

        private void ApplyType()
        {
            switch (type)
            {
                case FormType.Add:
                    button1.Text = "Agregar habitación";
                    this.Text = "Alta de Habitación";
                    break;
                case FormType.Modify:
                    button1.Text = "Modificar habitación";
                    this.Text = "Modificación de Habitación";
                    this.comboBox1.Enabled = false;
                    break;
            }
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            if (Regex.IsMatch(textBox1.Text, "[^0-9]"))
            {
                MessageBox.Show("El número de habitación debe estar compuesto sólo por números!", "ERROR");
                textBox1.Focus();
            }
        }

        private void textBox2_Validating(object sender, CancelEventArgs e)
        {
            if (Regex.IsMatch(textBox2.Text, "[^0-9]"))
            {
                MessageBox.Show("El piso de la habitación debe estar compuesto sólo por números!", "ERROR");
                textBox2.Focus();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string Numero = textBox1.Text;
            string Piso = textBox2.Text;
            string Ubicacion = checkBox1.Checked ? "S" : "N";
            TipoHabitacion Tipo = (TipoHabitacion) comboBox1.SelectedItem;
            string Descripcion = textBox4.Text;

            switch (type)
            {
                case FormType.Add:
                    if (!InputValido(Numero, Piso, Ubicacion, Tipo, Descripcion))
                        return;

                    Habitacion NewHab = new Habitacion(null, Session.Hotel, Convert.ToInt32(Numero),
                        Convert.ToInt32(Piso), Ubicacion, Tipo, Descripcion);

                    if (new HabitacionDAO().InsertarNuevaHabitacion(NewHab)) // creó el hotel?
                        this.Close();
                break;
                case FormType.Modify:
                    if (!InputValido(Numero, Piso, Ubicacion, Tipo, Descripcion))
                        return;

                    habitacion.Número = Convert.ToInt32(Numero);
                    habitacion.Piso = Convert.ToInt32(Piso);
                    habitacion.Ubicación = Ubicacion;
                    habitacion.TipoHabitación = Tipo;
                    habitacion.Descripción = Descripcion;

                    if (new HabitacionDAO().ModificarHabitacion(habitacion)) // creó el hotel?
                    {
                        parent.RefreshGrid();
                        this.Close();
                    }
                break;
            }
        }

        private bool InputValido(string Numero, string Piso, string Ubicacion, TipoHabitacion Tipo, string Descripcion)
        {
            string ErrMsg = "";

            if (Numero.Equals(""))
                ErrMsg += "Debe ingresar un número de habitación\n";
            if (Piso.Equals(""))
                ErrMsg += "Debe ingresar un piso de habitación\n";
            if (Tipo.Id == -1)
                ErrMsg += "Debe ingresar un tipo de habitación\n";

            bool Valido = ErrMsg.Equals("");
            if (!Valido)
                MessageBox.Show(ErrMsg, "ERROR");
            return Valido;
        }
    }
}
