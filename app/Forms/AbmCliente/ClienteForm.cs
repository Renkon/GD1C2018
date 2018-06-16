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
using FrbaHotel.Forms.AbmCliente;
using FrbaHotel.Model;
using System.Text.RegularExpressions;
using System.Globalization;
using FrbaHotel.Model.DAO;

namespace FrbaHotel.AbmCliente
{
    public partial class ClienteForm : Form
    {
        private FormType type;
        private Cliente cliente;
        private ViewerClienteForm parent;

        public ClienteForm(FormType type, Cliente cliente, ViewerClienteForm parent)
        {
            this.type = type;
            this.cliente = cliente;
            this.parent = parent;

            InitializeComponent();

            this.monthCalendarFechaNacimiento.MaxDate = Config.GetInstance().GetCurrentDate();
            this.monthCalendarFechaNacimiento.TodayDate = Config.GetInstance().GetCurrentDate();

            if (cliente != null)
                this.monthCalendarFechaNacimiento.SelectionStart = cliente.FechaNacimiento;

            ApplyType();
            PopulateLists();
            LoadContent();
        }

        private void PopulateLists()
        {
            List<TipoDocumento> TiposDocumento = new TipoDocumentoDAO().ObtenerTiposDocumento();
            comboBoxDni.Items.AddRange(TiposDocumento.ToArray());
            List<Pais> Paises = new PaisDAO().ObtenerPaises();
            comboBoxPais.Items.AddRange(Paises.ToArray());
        }

        // Se ejecuta cuando esta validando el textbox
        private void textBoxDni_Validating(object sender, CancelEventArgs e)
        {
            if (Regex.IsMatch(textBoxDni.Text, "[^0-9]"))
            {
                MessageBox.Show("El documento debe estar compuesto sólo por números!", "ERROR");
                textBoxDni.Focus();
            }
        }

        private void textBoxDireccionNro_Validating(object sender, CancelEventArgs e)
        {
            if (Regex.IsMatch(textBoxDireccionNro.Text, "[^0-9]"))
            {
                MessageBox.Show("La altura de la calle debe estar compuesta sólo por números!", "ERROR");
                textBoxDireccionNro.Focus();
            }
        }

        private void textBoxDireccionPiso_Validating(object sender, CancelEventArgs e)
        {
            if (Regex.IsMatch(textBoxDireccionPiso.Text, "[^0-9]"))
            {
                MessageBox.Show("El piso debe estar compuesto sólo por números!", "ERROR");
                textBoxDireccionPiso.Focus();
            }
        }

        private void LoadContent()
        {
            if (cliente != null)
            {
                this.textBoxNombre.Text = cliente.Nombre;
                this.textBoxApellido.Text = cliente.Apellido;
                this.comboBoxDni.SelectedItem = cliente.TipoDocumento;
                this.textBoxDni.Text = Convert.ToString(cliente.Documento);
                this.textBoxCorreo.Text = cliente.Correo;
                this.textBoxTelefono.Text = cliente.Telefono;
                this.textBoxDireccionCalle.Text = cliente.Calle;
                this.textBoxDireccionNro.Text = Convert.ToString(cliente.Nro);
                this.textBoxDireccionPiso.Text = cliente.Piso != 0 ? Convert.ToString(cliente.Piso) : "";
                this.textBoxDireccionDepartamento.Text = cliente.Departamento;
                this.textBoxCiudad.Text = cliente.Ciudad;
                this.textBoxNacionalidad.Text = cliente.Nacionalidad;
                this.textBoxFechaNacimiento.Text = cliente.FechaNacimiento.ToString("dd/MM/yyyy");
                this.comboBoxPais.SelectedItem = cliente.Pais;
                this.checkBoxEstado.Checked = cliente.Estado;
                this.checkBoxEstado.Enabled = !cliente.Estado;
            }
        }

        // Se usa para el calendario
        private void buttonFechaNacimiento_Click(object sender, EventArgs e)
        {
            monthCalendarFechaNacimiento.Visible = true;
            monthCalendarFechaNacimiento.Focus();
        }

        private void monthCalendarFechaNacimiento_Leave(object sender, EventArgs e)
        {
            monthCalendarFechaNacimiento.Visible = false;
        }

        private void monthCalendarFechaNacimiento_DateSelected(object sender, DateRangeEventArgs e)
        {
            textBoxFechaNacimiento.Text = monthCalendarFechaNacimiento.SelectionStart.ToString("dd/MM/yyyy");
            monthCalendarFechaNacimiento.Visible = false;
        }

        private void ApplyType()
        {
            switch (type)
            {
                case FormType.Add:
                    buttonAccion.Text = "Registrar cliente";
                    this.Text = "Alta de cliente";
                    checkBoxEstado.Visible = false;
                    break;
                case FormType.Modify:
                    buttonAccion.Text = "Editar cliente";
                    this.Text = "Modificar cliente";
                    checkBoxEstado.Checked = cliente.Estado;
                    break;
                case FormType.Delete:
                    buttonAccion.Text = "Bloquear cliente";
                    this.Text = "Deshabilitar cliente";
                    textBoxNombre.Enabled = false;
                    textBoxApellido.Enabled = false;
                    textBoxDni.Enabled = false;
                    textBoxCorreo.Enabled = false;
                    textBoxTelefono.Enabled = false;
                    textBoxDireccionCalle.Enabled = false;
                    textBoxDireccionNro.Enabled = false;
                    textBoxDireccionPiso.Enabled = false;
                    textBoxDireccionDepartamento.Enabled = false;
                    textBoxCiudad.Enabled = false;
                    textBoxNacionalidad.Enabled = false;
                    textBoxFechaNacimiento.Enabled = false;
                    checkBoxEstado.Visible = false;
                    buttonFechaNacimiento.Enabled = false;
                    comboBoxDni.Enabled = false;
                    comboBoxPais.Enabled = false;
                    break;
            }
        }

        // Se ejecuta al darle al botón de acción
        private void buttonAccion_Click(object sender, EventArgs e)
        {
            string Nombre = textBoxNombre.Text;
            string Apellido = textBoxApellido.Text;
            TipoDocumento TipoDocumento = (TipoDocumento)comboBoxDni.SelectedItem;
            string NumeroDocumento = textBoxDni.Text;
            string Correo = textBoxCorreo.Text;
            string Telefono = textBoxTelefono.Text;
            string Calle = textBoxDireccionCalle.Text;
            string Nro = textBoxDireccionNro.Text;
            string Piso = textBoxDireccionPiso.Text;
            string Departamento = textBoxDireccionDepartamento.Text;
            string Ciudad = textBoxCiudad.Text;
            Pais Pais = (Pais) comboBoxPais.SelectedItem;
            string Nacionalidad = textBoxNacionalidad.Text;
            string FechaNacimiento = textBoxFechaNacimiento.Text;
            bool Estado = checkBoxEstado.Checked;

            switch (type)
            {
                case FormType.Add:
                    if (!InputValido(Nombre, Apellido, TipoDocumento,
                        NumeroDocumento, Correo, Telefono, Calle, Nro ,Piso, Departamento, Ciudad, Pais, Nacionalidad, FechaNacimiento))
                        return;

                    Cliente NewUser = new Cliente(null, Nombre, Apellido, TipoDocumento, Convert.ToInt64(NumeroDocumento), Correo,
                        Telefono, Calle, Convert.ToInt32(Nro), Convert.ToInt32("0" + Piso), Departamento, Ciudad, Pais, Nacionalidad, DateTime.ParseExact(FechaNacimiento, "dd/MM/yyyy", CultureInfo.InvariantCulture), true);

                    if (new ClienteDAO().InsertarNuevoUsuario(NewUser))
                        this.Close();
                    break;
                case FormType.Modify:
                    if (!InputValido(Nombre, Apellido, TipoDocumento,
                        NumeroDocumento, Correo, Telefono, Calle, Nro ,Piso, Departamento, Ciudad, Pais, Nacionalidad, FechaNacimiento))
                        return;
                    
                    cliente.Nombre = Nombre;
                    cliente.Apellido = Apellido;
                    cliente.TipoDocumento = TipoDocumento;
                    cliente.Documento = Convert.ToInt64(NumeroDocumento);
                    cliente.Correo = Correo;
                    cliente.Telefono = Telefono;
                    cliente.Calle = Calle;
                    cliente.Nro = Convert.ToInt32(Nro);
                    cliente.Piso = Convert.ToInt32("0" + Piso);
                    cliente.Departamento = Departamento;
                    cliente.Ciudad = Ciudad;
                    cliente.Pais = Pais;
                    cliente.Nacionalidad = Nacionalidad;
                    cliente.FechaNacimiento = DateTime.ParseExact(FechaNacimiento, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    cliente.Estado = Estado;

                    if (new ClienteDAO().ModificarUsuario(cliente))
                    {
                        parent.RefreshGrid();
                        this.Close();
                    }
                    break;
                case FormType.Delete:
                    
                    if (new ClienteDAO().DeshabilitarUsuario(cliente))
                    {
                        parent.RefreshGrid();
                        this.Close();
                    }
                    break;
            }
        }

        private bool InputValido(string Nombre, string Apellido, TipoDocumento TipoDocumento, string NumeroDocumento, string Correo,
            string Telefono, string Calle, string Nro, string Piso, string Departamento, string Ciudad, Pais Pais, string Nacionalidad, string FechaNacimiento)
        {
            // pattern q matchea un email 
            // source: https://stackoverflow.com/questions/1365407/c-sharp-code-to-validate-email-address
            const string pattern = @"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$";
            string ErrMsg = "";
            if (Nombre.Equals(""))
                ErrMsg += "Debe ingresar el/los nombre/s real/es del usuario\n";
            if (Apellido.Equals(""))
                ErrMsg += "Debe ingresar el/los apellido/s real/es del usuario\n";
            if (TipoDocumento == null)
                ErrMsg += "Debe definir el tipo de documento\n";
            if (NumeroDocumento.Equals(""))
                ErrMsg += "Debe ingresar el número de documento\n";
            if (Correo.Equals("") || !Regex.IsMatch(Correo.ToLower(), pattern))
                ErrMsg += "Debe ingresar una dirección del correo electrónico válida\n";
            if (Telefono.Equals(""))
                ErrMsg += "Debe ingresar un número de teléfono\n";
            if (Calle.Equals(""))
                ErrMsg += "Debe ingresar una calle\n";
            if (Nro.Equals(""))
                ErrMsg += "Debe ingresar un nro\n";
            if (Ciudad.Equals(""))
                ErrMsg += "Debe ingresar una ciudad\n";
            if (Pais == null || Pais.Id == -1)
                ErrMsg += "Debe ingresar un pais\n";
            if (Nacionalidad.Equals(""))
                ErrMsg += "Debe ingresar una nacionalidad\n";
            if (FechaNacimiento.Equals(""))
                ErrMsg += "Debe seleccionar una fecha de nacimiento\n";

            bool Valido = ErrMsg.Equals("");
            if (!Valido)
                MessageBox.Show(ErrMsg, "ERROR");
            return Valido;
        }

        private void comboBoxPais_Validating(object sender, CancelEventArgs e)
        {
            if (comboBoxPais.SelectedIndex == -1)
            {
                MessageBox.Show("El pais no es valido!", "ERROR");
                comboBoxPais.Focus();
            }
        }

    }


    
}
