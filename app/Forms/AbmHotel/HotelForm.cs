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

namespace FrbaHotel.Forms.AbmHotel
{
    public partial class HotelForm : Form
    {
        private FormType type;
        private Hotel hotel;
        private ViewerHotelForm parent;

        public HotelForm(FormType type, Hotel hotel, ViewerHotelForm parent)
        {
            this.type = type;
            this.hotel = hotel;
            this.parent = parent;

            InitializeComponent();
            ApplyType();

            this.monthCalendar1.MaxDate = Config.GetInstance().GetCurrentDate();
            this.monthCalendar1.TodayDate = Config.GetInstance().GetCurrentDate();

            PopulateLists();
            LoadContent();
            
        }

        private void textBox5_Validating(object sender, CancelEventArgs e)
        {
            if (Regex.IsMatch(textBox5.Text, "[^0-9]"))
            {
                MessageBox.Show("El número debe estar compuesto sólo por números!", "ERROR");
                textBox5.Focus();
            }
        }

        private void LoadContent()
        {
            if (hotel != null)
            {
                textBox1.Text = hotel.Nombre;
                textBox2.Text = hotel.Correo;
                textBox3.Text = hotel.Teléfono;
                textBox4.Text = hotel.Domicilio_Calle;
                textBox5.Text = hotel.Domicilio_Número.ToString();
                textBox6.Text = hotel.Ciudad.TrimEnd();
                comboBox1.SelectedItem = hotel.País;
                numericUpDown1.Value = hotel.Cantidad_Estrellas;
                textBox7.Text = hotel.Recarga_Por_Estrellas.ToString();
                textBox8.Text = hotel.Fecha_Creación.ToString("dd/MM/yyyy");

                monthCalendar1.SelectionStart = hotel.Fecha_Creación;
            }
        }

        private void textBox7_Validating(object sender, CancelEventArgs e)
        {
            if (Regex.IsMatch(textBox7.Text, "[^0-9]"))
            {
                MessageBox.Show("El recargo por estrellas debe estar compuesta sólo por números!", "ERROR");
                textBox5.Focus();
            }
        }

        private void ApplyType()
        {
            switch (type)
            {
                case FormType.Add:
                    button1.Text = "Agregar hotel";
                    this.Text = "Alta de Hotel";
                break;
                case FormType.Modify:
                    button1.Text = "Modificar hotel";
                    this.Text = "Modificación de hotel";
                break;
            }
        }

        private void PopulateLists()
        {
            List<Regimen> Regimenes = new RegimenDAO().ObtenerRegimenes();
            List<int> IdsRegimenesHotel = hotel != null ?
                new RegimenDAO().ObtenerIdsRegimenesHotel(hotel) : new List<int>();
            this.listBox1.Items.AddRange(Regimenes
                .Where(r => IdsRegimenesHotel.Contains(r.Id)).ToArray());
            this.listBox2.Items.AddRange(Regimenes
                .Where(r => !IdsRegimenesHotel.Contains(r.Id)).ToArray());

            List<Pais> Paises = new PaisDAO().ObtenerPaises();
            comboBox1.Items.AddRange(Paises.ToArray());
        }

        // Mover objeto a la lista izquierda
        private void button3_Click(object sender, EventArgs e)
        {
            MoveFuncionalidades(listBox2, listBox1);
        }

        // Mover objeto a la lista derecha
        private void button2_Click(object sender, EventArgs e)
        {
            MoveFuncionalidades(listBox1, listBox2);
        }

        private void MoveFuncionalidades(ListBox list1, ListBox list2)
        {
            if (list1.SelectedIndex == -1)
            {
                MessageBox.Show("Debe seleccionar al menos un elemento", "ERROR");
                return; // Nada seleccionado
            }
            List<Regimen> rs = list1.SelectedItems.Cast<Regimen>().ToList();

            foreach (var r in rs)
            {
                list1.Items.Remove(r);
                list2.Items.Add(r);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string Nombre = textBox1.Text;
            string Correo = textBox2.Text;
            string Teléfono = textBox3.Text;
            string Calle = textBox4.Text;
            string Numero = textBox5.Text;
            string Ciudad = textBox6.Text;
            Pais Pais = (Pais) comboBox1.SelectedItem;
            int Estrellas = Convert.ToInt32(numericUpDown1.Value);
            string Recarga = textBox7.Text;
            List<Regimen> Regimenes = this.listBox1.Items.Cast<Regimen>().ToList();
            string Creacion = textBox8.Text;

            switch (type)
            {
                case FormType.Add:
                    if (!InputValido(Nombre, Correo, Teléfono, Calle, Numero, Ciudad, Pais, Estrellas, Recarga, Regimenes, Creacion))
                        return;

                    Hotel NewHotel = new Hotel(null, Nombre, Correo, Teléfono, Ciudad, Calle, Convert.ToInt32(Numero),
                        Estrellas, Pais, DateTime.ParseExact(Creacion, "dd/MM/yyyy", CultureInfo.InvariantCulture), Convert.ToInt32(Recarga), Regimenes);

                    if (new HotelDAO().InsertarNuevoHotel(NewHotel)) // creó el hotel?
                        this.Close();
                break;
                case FormType.Modify:
                    if (!InputValido(Nombre, Correo, Teléfono, Calle, Numero, Ciudad, Pais, Estrellas, Recarga, Regimenes, Creacion))
                        return;

                    hotel.Nombre = Nombre;
                    hotel.Correo = Correo;
                    hotel.Teléfono = Teléfono;
                    hotel.Domicilio_Calle = Calle;
                    hotel.Domicilio_Número = Convert.ToInt32(Numero);
                    hotel.Ciudad = Ciudad;
                    hotel.País = Pais;
                    hotel.Cantidad_Estrellas = Estrellas;
                    hotel.Recarga_Por_Estrellas = Convert.ToInt32(Recarga);
                    hotel.Regimenes = Regimenes;
                    hotel.Fecha_Creación = DateTime.ParseExact(Creacion, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    if (new HotelDAO().ModificarHotel(hotel)) // creó el hotel?
                    {
                        parent.RefreshGrid();
                        this.Close();
                    }
                break;
            }
        }

        private bool InputValido(string Nombre, string Correo, string Teléfono, string Calle,
            string Numero, string Ciudad, Pais Pais, int Estrellas, string Recarga, List<Regimen> Regimenes, string Creacion)
        {
            // pattern q matchea un email 
            // source: https://stackoverflow.com/questions/1365407/c-sharp-code-to-validate-email-address
            const string pattern = @"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$";
            string ErrMsg = "";
            if (Regimenes.Count < 1)
                ErrMsg += "Debe seleccionar al menos un regimen\n";
            if (Nombre.Equals(""))
                ErrMsg += "Debe ingresar el nombre del hotel\n";
            if (Correo.Equals("") || !Regex.IsMatch(Correo.ToLower(), pattern))
                ErrMsg += "Debe ingresar un correo válido del hotel\n";
            if (Teléfono.Equals(""))
                ErrMsg += "Debe ingresar un teléfono para hotel\n";
            if (Calle.Equals(""))
                ErrMsg += "Debe ingresar la calle del hotel\n";
            if (Numero.Equals(""))
                ErrMsg += "Debe ingresar el número (de la calle) del hotel\n";
            if (Ciudad.Equals(""))
                ErrMsg += "Debe ingresar la ciudad del hotel\n";
            if (Pais == null)
                ErrMsg += "Debe ingresar el país del hotel\n";
            if (Estrellas < 1 || Estrellas > 7)
                ErrMsg += "Debe ingresar una cantidad de estrellas válida (1 <= estrellas <= 7)\n";
            if (Recarga.Equals(""))
                ErrMsg += "Debe ingresar la recarga por estrella del hotel\n";
            if (Creacion.Equals(""))
                ErrMsg += "Debe ingresar una fecha de creación válida\n";

            bool Valido = ErrMsg.Equals("");
            if (!Valido)
                MessageBox.Show(ErrMsg, "ERROR");
            return Valido;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            monthCalendar1.Visible = true;
        }

        private void monthCalendar1_DateSelected(object sender, DateRangeEventArgs e)
        {
            textBox8.Text = monthCalendar1.SelectionStart.ToString("dd/MM/yyyy");
            monthCalendar1.Visible = false;
        }

        private void monthCalendar1_Leave(object sender, EventArgs e)
        {
            monthCalendar1.Visible = false;
        }
    }
}
