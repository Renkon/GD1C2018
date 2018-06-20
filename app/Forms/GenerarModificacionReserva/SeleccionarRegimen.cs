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

namespace FrbaHotel.Forms.GenerarModificacionReserva
{
    public partial class SeleccionarRegimen : Form
    {
        public Regimen Regimen { get; private set; }

        public SeleccionarRegimen(Hotel Hotel)
        {
            InitializeComponent();

            List<Regimen> Regimenes = new RegimenDAO().ObtenerRegimenesActivosDeHotel(Hotel);

            foreach (var reg in Regimenes)
            {
                if (reg.Estado)
                {
                    dataGridView1.Rows.Add(reg.Descripción, "USD " + reg.PrecioBase.ToString("0.00"));
                    dataGridView1.Rows[dataGridView1.Rows.Count - 1].Tag = reg;
                    dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[2].Value = "Seleccionar régimen";
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0) // Clickeó el botón?
            {
                Regimen = (Regimen)dataGridView1.SelectedRows[0].Tag;
                this.DialogResult = DialogResult.OK;
            }
        }
    }
}
