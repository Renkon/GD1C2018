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

namespace FrbaHotel.Forms.GenerarModificacionReserva
{
    public partial class ActualizarClienteMigrado : Form
    {
        private Cliente cliente;

        public ActualizarClienteMigrado(Cliente cliente)
        {
            this.cliente = cliente;

            InitializeComponent();

            LoadComboBoxes();

            textBox1.Text = cliente.Documento.ToString();
            textBox2.Text = cliente.Correo;
            textBox3.Text = cliente.Ciudad;
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            if (Regex.IsMatch(textBox1.Text, "[^0-9]"))
            {
                MessageBox.Show("El número de documento debe estar representado sólo por números!", "ERROR");
                textBox1.Focus();
            }
        }

        private void LoadComboBoxes()
        {
            List<TipoDocumento> Tipos = new TipoDocumentoDAO().ObtenerTiposDocumento();
            comboBox1.Items.AddRange(Tipos.ToArray());
            comboBox1.SelectedItem = cliente.TipoDocumento;

            List<Pais> Paises = new PaisDAO().ObtenerPaises();
            comboBox2.Items.AddRange(Paises.ToArray());
            comboBox2.SelectedItem = cliente.Pais;
        }

        private void comboBox2_Validating(object sender, CancelEventArgs e)
        {
            if (comboBox2.SelectedIndex == -1)
            {
                MessageBox.Show("El pais no es valido!", "ERROR");
                comboBox2.Focus();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!InputValido())
                return;

            // No deberían haber problemas para hacer el insert del cliente
            cliente.Documento = Convert.ToInt64(textBox1.Text);
            cliente.TipoDocumento = (TipoDocumento)comboBox1.SelectedItem;
            cliente.Correo = textBox2.Text;
            cliente.Ciudad = textBox3.Text;
            cliente.Pais = (Pais)comboBox2.SelectedItem;

            if (new ClienteDAO().InsertarClientePreexistente(cliente))
            {
                cliente.Estado = true;
                this.DialogResult = DialogResult.OK;
                this.Close();
                this.Dispose();
            }
        }

        private bool InputValido()
        {
            // pattern q matchea un email 
            // source: https://stackoverflow.com/questions/1365407/c-sharp-code-to-validate-email-address
            const string pattern = @"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$";
            string ErrMsg = "";
            ClienteDAO cDAO = new ClienteDAO();

            if (textBox1.Text.Equals(""))
                ErrMsg += "Debe ingresar el número de documento\n";
            if (textBox2.Text.Equals("") || !Regex.IsMatch(textBox2.Text.ToLower(), pattern))
                ErrMsg += "Debe ingresar una dirección del correo electrónico válida\n";
            if (textBox3.Text.Equals(""))
                ErrMsg += "Debe ingresar una ciudad\n";
            if (comboBox2.SelectedItem == null)
                ErrMsg += "Debe seleccionar un país\n";
            if (!textBox2.Text.Equals("") && !cDAO.isCorreoUnico(textBox2.Text))
                ErrMsg += "El correo que ingresó ya se encuentra registrado\n";
            if (!textBox1.Text.Equals("") && !cDAO.isDocumentoUnico((TipoDocumento) comboBox1.SelectedItem,
                Convert.ToInt64("0" + textBox1.Text)))
                ErrMsg += "El tipo y documento que ingresó ya se encuentra registrado\n";

            bool Valido = ErrMsg.Equals("");
            if (!Valido)
                MessageBox.Show(ErrMsg, "ERROR");
            return Valido;
        }

    }
}
