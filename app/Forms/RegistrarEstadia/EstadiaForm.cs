using FrbaHotel.Forms.GenerarModificacionReserva;
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

namespace FrbaHotel.Forms.RegistrarEstadia
{
    public partial class EstadiaForm : Form
    {
        private Reserva reserva;
        private Estadia estadia;

        private int huéspedesPosibles = 0;

        public EstadiaForm(Reserva reserva)
        {
            this.reserva = reserva;
            this.estadia = new EstadiaDAO().ObtenerEstadiaDeReserva(reserva);

            InitializeComponent();

            LoadInfoReserva();
            LoadInfoEstadia();
        }

        private void LoadInfoEstadia()
        {
            if (estadia.Id != null)
            {
                if (estadia.Clientes == null)
                {
                    estadia.Clientes = new ClienteDAO().ObtenerClientesDeEstadia(estadia);
                    foreach (var cliente in estadia.Clientes)
                    {
                        dataGridView1.Rows.Add(cliente.Nombre, cliente.Apellido,
                            cliente.TipoDocumento.Sigla, cliente.Documento,
                            cliente.Correo);
                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].Tag = cliente;
                    }
                }

                textBox1.Text = estadia.Fecha_Inicio.HasValue ? estadia.Fecha_Inicio.Value.ToString("dd/MM/yyyy") : string.Empty;
                textBox2.Text = estadia.Fecha_Fin.HasValue ? estadia.Fecha_Fin.Value.ToString("dd/MM/yyyy") : string.Empty;
                button1.Enabled = false;
                button2.Text = "Finalizar estadía";
                dataGridView1.Columns[5].Visible = false;
            }
            else
            {
                Cliente clienteOriginal = new ClienteDAO().ObtenerClienteDeReserva(reserva);
                button2.Text = "Iniciar estadía";
                // Estadía es null, ingresamos al primer huésped (el cliente que reservó)
                dataGridView1.Rows.Add(clienteOriginal.Nombre, clienteOriginal.Apellido,
                    clienteOriginal.TipoDocumento.Sigla, clienteOriginal.Documento,
                    clienteOriginal.Correo);
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Tag = clienteOriginal;
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[5].Value = "Eliminar";
            }
        }

        private void LoadInfoReserva()
        {
            textBox3.Text = reserva.Fecha_Inicio.ToString("dd/MM/yyyy");
            textBox4.Text = reserva.Fecha_Fin.ToString("dd/MM/yyyy");
            textBox5.Text = reserva.Regimen.Descripción;
            textBox6.Text = reserva.Habitaciones.First(h => h.Hotel != null).Hotel.Nombre;

            foreach (var hab in reserva.Habitaciones)
            {
                dataGridView2.Rows.Add(hab.Número, hab.Piso, hab.Ubicación, 
                    hab.TipoHabitación.Descripción, hab.Descripción);
                dataGridView2.Rows[dataGridView2.Rows.Count - 1].Tag = hab;

                huéspedesPosibles += hab.TipoHabitación.Huéspedes;
            }

            label4.Text = label4.Text.Replace("${huespedes}", huéspedesPosibles.ToString());
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0) // Clickeó el botón?
            {
                dataGridView1.Rows.RemoveAt(e.RowIndex);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == huéspedesPosibles)
            {
                MessageBox.Show("No puedes agregar mas huéspedes. Si deseas agregar uno "
                    + "tendrás que eliminar a uno que ya esté en la lista", "INFO");
                return;
            }

            Cliente clienteNuevo;
            
            ReservaFormPaso2 Form = new ReservaFormPaso2();
            if (Form.ShowDialog() == DialogResult.OK)
            {
                clienteNuevo = Form.Cliente;
                Form.Close();
                Form.Dispose();
            }
            else
            {
                Form.Close();
                Form.Dispose();
                return;
            }

            if (!clienteNuevo.Estado)
            {
                MessageBox.Show("Lo sentimos. El huésped se encuentra deshabilitado", "ERROR");
                return;
            }

            if (GetClientesHuéspedes().Contains(clienteNuevo))
            {
                MessageBox.Show("Ese cliente ya está en la lista de huéspedes!", "ERROR");
                return;
            }

            // Lo inserto al dataGridView de clientes
            dataGridView1.Rows.Add(clienteNuevo.Nombre, clienteNuevo.Apellido,
                clienteNuevo.TipoDocumento.Sigla, clienteNuevo.Documento,
                clienteNuevo.Correo);
            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Tag = clienteNuevo;
            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[5].Value = "Eliminar";
        }

        private List<Cliente> GetClientesHuéspedes()
        {
            List<Cliente> Clientes = new List<Cliente>();

            foreach (DataGridViewRow row in dataGridView1.Rows)
                Clientes.Add((Cliente) row.Tag);

            return Clientes;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (GetClientesHuéspedes().Count < dataGridView2.Rows.Count)
            {
                // Esto quiere decir que hay menos huéspedes que habitaciones
                MessageBox.Show("Debe tener al menos igual cantidad de huéspedes que de habitaciones!", "ERROR");
                return;
            }

            if (estadia.Id == null) // nueva estadia
            {
                if (new EstadiaDAO().InsertarEstadia(estadia, reserva, GetClientesHuéspedes()))
                    LoadInfoEstadia();
            }
            else
            {
                if (new EstadiaDAO().CerrarEstadia(estadia))
                {
                    textBox2.Text = estadia.Fecha_Fin.Value.ToString("dd/MM/yyyy");
                    button2.Enabled = false;
                    button2.Text = "Estadía cerrada";
                }

            }
        }
    }
}
