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
    public partial class ConsumosForm : Form
    {
        private Estadia estadia;

        public ConsumosForm(Estadia estadia)
        {
            this.estadia = estadia;
            estadia.Habitaciones = new HabitacionDAO().ObtenerHabitacionesDeEstadia(estadia);

            InitializeComponent();

            LoadEstadiaData();
        }

        private void LoadEstadiaData()
        {
            textBox1.Text = estadia.Id.ToString();
            textBox2.Text = estadia.Fecha_Inicio.HasValue ? estadia.Fecha_Inicio.Value.ToString("dd/MM/yyyy") : string.Empty;
            textBox3.Text = estadia.Fecha_Fin.HasValue ? estadia.Fecha_Fin.Value.ToString("dd/MM/yyyy") : string.Empty;

            foreach (Consumo c in new ConsumoDAO().ObtenerConsumosDeEstadia(estadia))
            {
                AddConsumoToGrid(c);
            }

            if (estadia.Fecha_Fin == null)
                button1.Enabled = false;
            else
                button2.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ConsumibleForm Form = new ConsumibleForm(FormType.Add, estadia, null);
            if (Form.ShowDialog() == DialogResult.OK)
            {
                AddConsumoToGrid(Form.Consumo);
            }
            Form.Close();
            Form.Dispose();
        }

        private void AddConsumoToGrid(Consumo newConsumo)
        {
            dataGridView1.Rows.Add(newConsumo.Fecha.ToString("dd/MM/yyyy"), newConsumo.Habitacion.Número, 
                newConsumo.Consumible.Descripción, "USD " + newConsumo.Consumible.Precio, 
                newConsumo.Cantidad, "USD " + newConsumo.Consumible.Precio * newConsumo.Cantidad);
            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Tag = newConsumo;
            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[6].Value = "Modificar";
            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[7].Value = "Borrar";
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0) // Clickeó el botón?
            {
                if (e.ColumnIndex == 6) // Modificar
                {
                    ConsumibleForm Form = new ConsumibleForm(FormType.Modify, estadia, 
                        (Consumo) dataGridView1.SelectedRows[0].Tag);
                    
                    if (Form.ShowDialog() == DialogResult.OK)
                    {
                        dataGridView1.Rows[e.RowIndex].SetValues(Form.Consumo.Fecha.ToString("dd/MM/yyyy"),
                            Form.Consumo.Habitacion.Número, Form.Consumo.Consumible.Descripción,
                            "USD " + Form.Consumo.Consumible.Precio, Form.Consumo.Cantidad,
                            "USD " + Form.Consumo.Consumible.Precio * Form.Consumo.Cantidad);
                    }
                    Form.Close();
                    Form.Dispose();
                }
                else // La unica columna que queda botón es la de Borraara
                {
                    if (MessageBox.Show("¿Está seguro que desea borrar el consumo?", "INFO", MessageBoxButtons.YesNo)
                        == DialogResult.Yes)
                    {
                        if (new ConsumoDAO().BorrarConsumo((Consumo)dataGridView1.SelectedRows[0].Tag))
                            dataGridView1.Rows.RemoveAt(e.RowIndex);
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0 && MessageBox.Show("No tiene consumos registrados. ¿Es esto correcto?",
                    "INFO", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            if (new EstadiaDAO().CerrarConsumosEstadia(this.estadia))
            {
                this.button1.Text = "Consumibles cerrados";
                this.button1.Enabled = false;
                this.dataGridView1.Columns[6].Visible = false;
                this.dataGridView1.Columns[7].Visible = false;
            }
        }
    }
}
