using FrbaHotel.AbmCliente;
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
    public partial class ReservaFormPaso2 : Form
    {
        public Cliente Cliente { get; private set; }
        public ReservaFormPaso2()
        {
            InitializeComponent();

            LoadComboBox();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();

            textBox1.Text = "";
            textBox2.Text = "";
            comboBox1.SelectedIndex = 0;
        }

        private void LoadComboBox()
        {
            TipoDocumento Dummy = new TipoDocumento(-1, "", " - selecc. -");
            List<TipoDocumento> Tipos = new TipoDocumentoDAO().ObtenerTiposDocumento();

            comboBox1.Items.Add(Dummy);
            comboBox1.Items.AddRange(Tipos.ToArray());

            comboBox1.SelectedItem = Dummy;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<Cliente> Clientes = new ClienteDAO().ObtenerClientesCompletosFiltrado(
                (TipoDocumento) comboBox1.SelectedItem, Convert.ToInt64("0" + textBox1.Text), textBox2.Text);

            // Poblamos el grid
            dataGridView1.Rows.Clear();
            foreach (Cliente cli in Clientes)
            {
                dataGridView1.Rows.Add(cli.Nombre, cli.Apellido, cli.TipoDocumento.Sigla,
                    cli.Documento, cli.Correo);
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Tag = cli;
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[5].Value = "Seleccionar";
                if (cli.Id == -1)
                    dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.LightGray;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ClienteForm Form = new ClienteForm(FormType.Register, null, null);

            if (Form.ShowDialog() == DialogResult.OK)
            {
                Cliente = Form.GetCliente();
                this.DialogResult = DialogResult.OK;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0) // Clickeó el botón?
            {
                Cliente c = (Cliente)dataGridView1.SelectedRows[0].Tag;

                if (c.Id == -1) // ¿es un cliente migrado con datos erróneos?
                {
                    // Debe actualizar sus datos
                    ActualizarClienteMigrado Form = new ActualizarClienteMigrado(c);
                    if (Form.ShowDialog() == DialogResult.OK)
                    {
                        Form.Close();
                        Form.Dispose();
                    }
                    else
                    {
                        Form.Close();
                        Form.Dispose();
                        return;
                    }
                }

                Cliente = c;
                this.DialogResult = DialogResult.OK;
            }
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            if (Regex.IsMatch(textBox1.Text, "[^0-9]"))
            {
                MessageBox.Show("El número de documento debe estar representado sólo por números!", "ERROR");
                textBox1.Focus();
            }
        }
    }
}
