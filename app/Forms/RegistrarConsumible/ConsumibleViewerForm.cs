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

namespace FrbaHotel.Forms.RegistrarConsumible
{
    public partial class ConsumibleViewerForm : Form
    {
        public Consumible Consumible { get; private set; }

        public ConsumibleViewerForm()
        {
            InitializeComponent();

            LoadContent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            button2_Click(null, null);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoadContent();
        }

        private void LoadContent()
        {
            List<Consumible> consumibles = new ConsumibleDAO().ObtenerConsumiblesFiltrados(textBox1.Text);
            
            dataGridView1.Rows.Clear();

            foreach (Consumible c in consumibles)
            {
                dataGridView1.Rows.Add(c.Descripción, "USD " + c.Precio);
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Tag = c;
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[2].Value = "Seleccionar";
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0) // Clickeó el botón?
            {
                this.Consumible = (Consumible)dataGridView1.SelectedRows[0].Tag;
                this.DialogResult = DialogResult.OK;
            }
        }
    }
}
