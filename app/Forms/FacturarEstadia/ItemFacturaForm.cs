using FrbaHotel.Model;
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

namespace FrbaHotel.Forms.FacturarEstadia
{
    public partial class ItemFacturaForm : Form
    {
        private FormType type;
        public ItemFactura ItemFactura { get; set; }

        public ItemFacturaForm(FormType type, ItemFactura item)
        {
            this.type = type;
            this.ItemFactura = item;

            InitializeComponent();
            ApplyType();

            if (item != null)
            {
                textBox1.Text = item.Precio.ToString();
                textBox2.Text = item.Descripción;
                textBox3.Text = item.Cantidad.ToString();
            }
        }

        private void ApplyType()
        {
            switch (type)
            {
                case FormType.Add:
                    this.Text = "Agregar ítem de factura";
                    this.button1.Text = "Agregar ítem";
                break;
                case FormType.Modify:
                    this.Text = "Modificar ítem de factura";
                    this.button1.Text = "Modificar ítem";
                break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string Precio = textBox1.Text;
            string Descripción = textBox2.Text;
            string Cantidad = textBox3.Text;

            if (!InputValido(Precio, Descripción, Cantidad))
                return;

            // Pongo los numeros decimales como corresponde
            var separator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            Precio = Regex.Replace(Precio, "[.,]", separator);

            switch (type)
            {
                case FormType.Add:
                    this.ItemFactura = new ItemFactura(Convert.ToDouble(Precio), Descripción, Convert.ToInt32(Cantidad));
                break;
                case FormType.Modify:
                    this.ItemFactura.Precio = Convert.ToDouble(Precio);
                    this.ItemFactura.Descripción = Descripción;
                    this.ItemFactura.Cantidad = Convert.ToInt32(Cantidad);
                break;
            }

            this.DialogResult = DialogResult.OK;
        }

        private bool InputValido(string Precio, string Descripción, string Cantidad)
        {
            string ErrMsg = "";

            if (Precio.Equals(""))
                ErrMsg += "Debe ingresar un precio del ítem\n";
            if (Descripción.Equals(""))
                ErrMsg += "Debe ingresar la descripción del ítem\n";
            if (Cantidad.Equals(""))
                ErrMsg += "Debe ingresar la cantidad del ítem\n";

            bool Valido = ErrMsg.Equals("");
            if (!Valido)
                MessageBox.Show(ErrMsg, "ERROR");
            return Valido;
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            if (!Regex.IsMatch(textBox1.Text, "^[0-9]*([.,][0-9]{1,2})?$"))
            {
                MessageBox.Show("El precio debe ser representado por un número válido con hasta dos decimales!", "ERROR");
                textBox1.Focus();
            }
        }

        private void textBox3_Validating(object sender, CancelEventArgs e)
        {
            if (Regex.IsMatch(textBox3.Text, "[^0-9]"))
            {
                MessageBox.Show("La cantidad debe ser representada sólo con números!", "ERROR");
                textBox3.Focus();
            }
        }
    }
}
