using FrbaHotel.AbmCliente;
using FrbaHotel.AbmRol;
using FrbaHotel.Database;
using FrbaHotel.Forms;
using FrbaHotel.Forms.AbmCliente;
using FrbaHotel.Forms.AbmHabitacion;
using FrbaHotel.Forms.AbmHotel;
using FrbaHotel.Forms.AbmRol;
using FrbaHotel.Forms.AbmUsuario;
using FrbaHotel.Forms.CancelarReserva;
using FrbaHotel.Forms.FacturarEstadia;
using FrbaHotel.Forms.GenerarModificacionReserva;
using FrbaHotel.Forms.ListadoEstadistico;
using FrbaHotel.Forms.Login;
using FrbaHotel.Forms.RegistrarConsumible;
using FrbaHotel.Forms.RegistrarEstadia;
using FrbaHotel.Login;
using FrbaHotel.Model;
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
        private Dictionary<int, ToolStripMenuItem> menu = new Dictionary<int, ToolStripMenuItem>();
        private List<ToolStripMenuItem> menuParent = new List<ToolStripMenuItem>();

        public MainForm()
        {
            DatabaseConnection.GetInstance().TestConnection();

            InitializeComponent();

            LoadMenues();

            Session.MainForm = this;
            // Seteamos la sesión de guest
            Session.InitGuest();

            this.label1.Text = this.label1.Text.Replace("${fecha}",
                Config.GetInstance().GetCurrentDate().ToString("dd/MM/yyyy"));
        }

        public void HideAll()
        {
            // Oculto todos las funcionalidades
            foreach (var entry in menu)
                entry.Value.Visible = false;

            // Y todos los menues padres
            foreach (var parent in menuParent)
                parent.Visible = false;

        }

        private void mostrarMenu(int key)
        {
            // Si tienen lo de regimenes lo ignoramos
            if (new int[] { 19, 20, 21 }.Contains(key))
                return;

            ToolStripMenuItem item = menu[key];
            ToolStripItem parent = item.OwnerItem;

            if (!parent.Visible)
                parent.Visible = true;
            item.Visible = true;
        }

        public void SetFuncionalidadesVisibles(List<int> funcionalidades)
        {
            HideAll();

            if (Session.LoggedIn)
                mostrarMenu(-1);
            else
                mostrarMenu(0);

            funcionalidades.ForEach(f => mostrarMenu(f));
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

        private void iniciarSesiónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new LoginForm().ShowDialog();
        }

        private void cerrarSesiónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Hasta luego " + Session.User.Nombre + " " + Session.User.Apellido, "INFO");
            Session.Reset();
        }

        private void nuevoClienteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ClienteForm(FormType.Add, null, null).ShowDialog();
        }

        private void modificarClienteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ViewerClienteForm(FormType.Modify).ShowDialog();
        }

        private void borrarClienteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ViewerClienteForm(FormType.Delete).ShowDialog();
        }

        private void nuevoHotelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new HotelForm(FormType.Add, null, null).ShowDialog();
        }

        private void modificarHotelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ViewerHotelForm(FormType.Modify).ShowDialog();
        }

        private void cerrarTemporalmenteHotelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ViewerHotelForm(FormType.Delete).ShowDialog();
        }

        private void nuevaHabitaciónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new HabitacionForm(FormType.Add, null, null).ShowDialog();
        }

        private void modificarHabitaciónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ViewerHabitacionForm(FormType.Modify).ShowDialog();
        }

        private void eliminarHabitaciónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ViewerHabitacionForm(FormType.Delete).ShowDialog();
        }

        private void nuevaReservaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ReservaFormPaso1(null, FormType.Add).ShowDialog();
        }

        private void modificarReservaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReservaModDel FormTmp = new ReservaModDel(FormType.Modify);
            if (FormTmp.ShowDialog() == DialogResult.OK)
            {
                Reserva r = FormTmp.Reserva;

                FormTmp.Close();
                FormTmp.Dispose();

                new ReservaFormPaso1(r, FormType.Modify).ShowDialog();
            }
        }

        private void cancelarReservaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReservaModDel FormTmp = new ReservaModDel(FormType.Delete);
            if (FormTmp.ShowDialog() == DialogResult.OK)
            {
                Reserva r = FormTmp.Reserva;

                FormTmp.Close();
                FormTmp.Dispose();

                new ReservaCancelForm(r).ShowDialog();
            }
        }

        private void registrarEstadíaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReservaGetterForm Form = new ReservaGetterForm();
            if (Form.ShowDialog() == DialogResult.OK)
            {
                Reserva reservaRelacionada = Form.Reserva;

                Form.Close();
                Form.Dispose();

                new EstadiaForm(reservaRelacionada).ShowDialog();
            }
        }

        private void registrarConsumiblesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EstadiaGetterForm Form = new EstadiaGetterForm();
            if (Form.ShowDialog() == DialogResult.OK)
            {
                Estadia estadiaRelacionada = Form.Estadia;

                Form.Close();
                Form.Dispose();

                new ConsumosForm(estadiaRelacionada).ShowDialog();
            }
        }

        private void facturarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FacturacionForm().ShowDialog();
        }

        private void generarListadoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new EstadisticasForm().ShowDialog();
        }

        private void LoadMenues()
        {
            menuParent.Add(statusToolStripMenuItem);
            menuParent.Add(reservasToolStripMenuItem);
            menuParent.Add(rolesToolStripMenuItem);
            menuParent.Add(usuariosToolStripMenuItem);
            menuParent.Add(clientesToolStripMenuItem);
            menuParent.Add(hotelesToolStripMenuItem);
            menuParent.Add(habitacionesToolStripMenuItem);
            menuParent.Add(estadíaToolStripMenuItem);
            menuParent.Add(consumiblesToolStripMenuItem);
            menuParent.Add(facturaciónToolStripMenuItem);
            menuParent.Add(estadísticasToolStripMenuItem);

            menu.Add(-1, cerrarSesiónToolStripMenuItem);
            menu.Add(0, iniciarSesiónToolStripMenuItem);
            menu.Add(1, nuevaReservaToolStripMenuItem);
            menu.Add(2, modificarReservaToolStripMenuItem);
            menu.Add(3, cancelarReservaToolStripMenuItem);
            menu.Add(4, nuevoRolToolStripMenuItem);
            menu.Add(5, modificarRolToolStripMenuItem);
            menu.Add(6, borrarRolToolStripMenuItem);
            menu.Add(7, nuevoUsuarioToolStripMenuItem);
            menu.Add(8, modificarUsuarioToolStripMenuItem);
            menu.Add(9, borrarUsuarioToolStripMenuItem);
            menu.Add(10, nuevoClienteToolStripMenuItem);
            menu.Add(11, modificarClienteToolStripMenuItem);
            menu.Add(12, borrarClienteToolStripMenuItem);
            menu.Add(13, nuevoHotelToolStripMenuItem);
            menu.Add(14, modificarHotelToolStripMenuItem);
            menu.Add(15, cerrarTemporalmenteHotelToolStripMenuItem);
            menu.Add(16, nuevaHabitaciónToolStripMenuItem);
            menu.Add(17, modificarHabitaciónToolStripMenuItem);
            menu.Add(18, eliminarHabitaciónToolStripMenuItem);
            menu.Add(22, registrarEstadíaToolStripMenuItem);
            menu.Add(23, registrarConsumiblesToolStripMenuItem);
            menu.Add(24, facturarToolStripMenuItem);
            menu.Add(25, generarListadoToolStripMenuItem);
        }
    }
}
