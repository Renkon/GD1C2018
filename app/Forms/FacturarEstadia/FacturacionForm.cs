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

namespace FrbaHotel.Forms.FacturarEstadia
{
    public partial class FacturacionForm : Form
    {
        private Estadia estadia;
        private Reserva reserva;

        public FacturacionForm()
        {
            InitializeComponent();
            LoadFormasDePago();
        }

        private void LoadFormasDePago()
        {
            FormaDePago Dummy = new FormaDePago(-1, " - Seleccione la forma de pago - ");
            List<FormaDePago> FormasDePago = new FormaDePagoDAO().ObtenerFormasDePago();

            comboBox1.Items.Add(Dummy);
            comboBox1.Items.AddRange(FormasDePago.ToArray());
            comboBox1.SelectedItem = Dummy;
        }

        private void PopularItemsFacturas()
        {
            List<Consumo> consumos = new ConsumoDAO().ObtenerConsumosDeEstadia(estadia);

            dataGridView1.Rows.Clear();

            double costoDiario = new EstadiaDAO().ObtenerCostoDiarioEstadia(estadia);
            int diasReserva = reserva.Fecha_Fin.Subtract(reserva.Fecha_Inicio).Days;
            int diasEstadia = estadia.Fecha_Fin.Value.Subtract(estadia.Fecha_Inicio.Value).Days;

            // Agrego el item relacionado a los días en estadía
            AgregarItemFactura(new ItemFactura(costoDiario, "Días de alojamiento", diasEstadia), true);
            if (diasReserva != diasEstadia)
                AgregarItemFactura(new ItemFactura(costoDiario, 
                    "Días de alojamiento no utilizados", diasReserva - diasEstadia), true);

            foreach (Consumo c in consumos)
            {
                AgregarItemFactura(new ItemFactura(c), true);
            }

            RegimenDAO rDAO = new RegimenDAO();
            Regimen regimen = rDAO.ObtenerRegimenDeEstadia(estadia);
            if (regimen.Equals(rDAO.ObtenerRegimenAllInclusive())) // es all inclusive? descuento todo
                AgregarItemFactura(new ItemFactura(
                    -consumos.Select(c => c.Cantidad * c.Consumible.Precio).Sum(), 
                    "Descuento por régimen All Inclusive", 1), true);
            else if (regimen.Equals(rDAO.ObtenerRegimenAllInclusiveModerado())) // es moderado? 50% off
                AgregarItemFactura(new ItemFactura(
                    -0.5*consumos.Select(c => c.Cantidad * c.Consumible.Precio).Sum(),
                    "Descuento por régimen All Inclusive moderado", 1), true);

            ActualizarPrecioFinal();
        }

        private void ActualizarPrecioFinal()
        {
            textBox6.Text = GetPrecioFinal().ToString();
        }

        private double GetPrecioFinal()
        {
            return ObtenerItemsFactura().Select(i => i.Cantidad * i.Precio).Sum();
        }

        private List<ItemFactura> ObtenerItemsFactura()
        {
            List<ItemFactura> items = new List<ItemFactura>();

            foreach (DataGridViewRow row in dataGridView1.Rows)
                items.Add(((Tuple<ItemFactura, bool>)row.Tag).Item1);

            return items;
        }

        private void AgregarItemFactura(ItemFactura itemFactura, bool noModificable)
        {
            dataGridView1.Rows.Add(itemFactura.Descripción, "USD " + itemFactura.Precio,
                itemFactura.Cantidad, "USD " + itemFactura.Precio * itemFactura.Cantidad);
            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Tag = new Tuple<ItemFactura, bool>(itemFactura, noModificable);
            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[4].Value = "Modificar";
            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[5].Value = "Borrar";

            if (noModificable)
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].DefaultCellStyle.BackColor = Color.LightPink;
        }

        private void ModificarItemFactura(ItemFactura item, int rowIndex)
        {
            dataGridView1.Rows[rowIndex].SetValues(item.Descripción, "USD " + item.Precio,
                item.Cantidad, "USD " + item.Precio * item.Cantidad);
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            if (Regex.IsMatch(textBox1.Text, "[^0-9]"))
            {
                MessageBox.Show("El código de reserva sólo por números!", "ERROR");
                textBox1.Focus();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Equals(""))
            {
                MessageBox.Show("Debe ingresar un código de reserva válido para poder avanzar", "ERROR");
                return;
            }

            Tuple<Estadia, Reserva> Data = new EstadiaDAO().ObtenerDatosEstadiaFacturacion(
                Convert.ToInt32(textBox1.Text));

            if (Data.Item1 == null || Data.Item2 == null)
            {
                MessageBox.Show("No se puede iniciar el proceso de facturación. Esto puede deberse a que "
                    + "el código es inválido, la factura ya se haya realizado, la estadía esté en curso o "
                    + "que los consumos no hayan sido cerrados", "ERROR");
                return;
            }

            this.estadia = Data.Item1;
            this.reserva = Data.Item2;

            textBox5.Text = estadia.Fecha_Inicio.Value.ToString("dd/MM/yyyy");
            textBox5.Tag = estadia.Fecha_Inicio;
            textBox4.Text = estadia.Fecha_Fin.Value.ToString("dd/MM/yyyy");
            textBox4.Tag = estadia.Fecha_Fin;
            textBox2.Text = reserva.Fecha_Inicio.ToString("dd/MM/yyyy");
            textBox2.Tag = reserva.Fecha_Inicio;
            textBox3.Text = reserva.Fecha_Fin.ToString("dd/MM/yyyy");
            textBox3.Tag = reserva.Fecha_Fin;

            PopularItemsFacturas();

            button3.Enabled = true;
            button2.Enabled = true;
            comboBox1.Enabled = true;
            comboBox1.SelectedIndex = 0;
            textBox7.Enabled = true;
            textBox7.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ItemFacturaForm Form = new ItemFacturaForm(FormType.Add, null);
            if (Form.ShowDialog() == DialogResult.OK)
            {
                AgregarItemFactura(Form.ItemFactura, false);
                ActualizarPrecioFinal();
            }
            Form.Close();
            Form.Dispose();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0) // Clickeó el botón?
            {
                Tuple<ItemFactura, bool> selectedItem = (Tuple<ItemFactura, bool>)dataGridView1.SelectedRows[0].Tag;

                if (selectedItem.Item2)
                    return; 

                if (e.ColumnIndex == 4) // Modificar
                {
                    ItemFacturaForm Form = new ItemFacturaForm(FormType.Modify, selectedItem.Item1);
                    if (Form.ShowDialog() == DialogResult.OK)
                    {
                        ModificarItemFactura(Form.ItemFactura, e.RowIndex);
                    }
                    Form.Close();
                    Form.Dispose();
                }
                else // Borrar - otro único botón
                {
                    if (MessageBox.Show("¿Estás seguro que deseas eliminar este item?", "INFO", MessageBoxButtons.YesNo)
                        == DialogResult.Yes)
                    dataGridView1.Rows.RemoveAt(e.RowIndex);
                }

                ActualizarPrecioFinal();
            }
        }

        private bool InputValido()
        {
            string ErrMsg = "";
            if (textBox7.Text.Equals(""))
                ErrMsg += "Debe ingresar detalles de la forma de pago\n";
            if (((FormaDePago)comboBox1.SelectedItem).Id == -1)
                ErrMsg += "Debe seleccionar una forma de pago\n";

            bool Valido = ErrMsg.Equals("");
            if (!Valido)
                MessageBox.Show(ErrMsg, "ERROR");
            return Valido;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!InputValido())
                return;

            Factura NewFactura = new Factura(Config.GetInstance().GetCurrentDate(),
                GetPrecioFinal(), estadia, (FormaDePago)comboBox1.SelectedItem,
                textBox7.Text, ObtenerItemsFactura());

            // Se insertó la factura?
            if (new FacturaDAO().CrearFactura(NewFactura))
            {
                NewFactura.GuardarTXT();
                this.Close();
            }
        }
    }
}
