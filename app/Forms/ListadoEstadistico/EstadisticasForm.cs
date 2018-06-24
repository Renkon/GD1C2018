using FrbaHotel.Model.Custom;
using FrbaHotel.Model.Custom.DAO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FrbaHotel.Forms.ListadoEstadistico
{
    public partial class EstadisticasForm : Form
    {
        public EstadisticasForm()
        {
            InitializeComponent();

            LoadTrimestres();
            LoadListados();
        }

        private void LoadContent()
        {
            Listado listado = (Listado)comboBox2.SelectedItem;
            Trimestre trimestre = (Trimestre)comboBox1.SelectedItem;

            List<string[]> datos = new ListadoDAO().ObtenerListado(listado, trimestre);

            // Cargo las columnas
            foreach (string column in listado.Columns)
                dataGridView1.Columns.Add(column, column);

            // Y las rows
            foreach (string[] row in datos)
                dataGridView1.Rows.Add(row);
        }

        private void LoadListados()
        {
            comboBox2.Items.Add(new Listado(-1));

            comboBox2.Items.Add(new Listado(1, "hoteles con mayor cantidad de reservas canceladas",
                "TOP5_HOTELES_RESERVAS_CANCELADAS", 
                new string[] { "Nombre de hotel", "Reservas canceladas" },
                new string[] { "nombre_hotel", "cancelaciones" },
                false));

            comboBox2.Items.Add(new Listado(2, "hoteles con mayor cantidad de consumibles facturados",
                "TOP5_HOTELES_MAYOR_CANTIDAD_CONSUMOS", 
                new string[] { "Nombre de hotel", "Consumibles facturados" }, 
                new string[] { "nombre_hotel", "cantidad_consumibles_total" },
                false));

            comboBox2.Items.Add(new Listado(3, "hoteles con mayor cantidad de días fuera de servicio",
                "TOP5_HOTELES_DIAS_CERRADO", 
                new string[] { "Nombre de hotel", "Cantidad de días que estuvo cerrado" }, 
                new string[] { "nombre_hotel", "dias_cerrado" },
                false));

            comboBox2.Items.Add(new Listado(4, "habitaciones con mayor cantidad de días "
                + "y veces que fueron ocupadas",
                "TOP5_HABITACIONES_MAYOR_CANTIDAD_DIAS_ESTADIAS", 
                new string[] { "Número de habitación", "Nombre de hotel", "Días (ocupados)", "Cantidad de estadías" }, 
                new string[] { "numero_habitacion", "nombre_hotel", "cantidad_dias", "cantidad_estadias" },
                true));

            comboBox2.Items.Add(new Listado(5, "clientes con mayor cantidad de puntos",
                "LISTADO_TOP5_NOSEQPIJA", new string[] { }, new string[] { },
                false));

            comboBox2.SelectedIndex = 0;
        }

        private void LoadTrimestres()
        {
            comboBox1.Items.Add(new Trimestre(2015, -1));
            comboBox1.Items.Add(new Trimestre(2015, 1));
            comboBox1.Items.Add(new Trimestre(2015, 2));
            comboBox1.Items.Add(new Trimestre(2015, 3));
            comboBox1.Items.Add(new Trimestre(2015, 4));

            comboBox1.SelectedIndex = 0;
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            foreach (Trimestre trimestre in comboBox1.Items)
            {
                trimestre.Año = dateTimePicker1.Value.Year;
                trimestre.onYearChanged();
            }

            RefreshCombo(comboBox1);
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
        }

        // Gracias .NET por tener RefreshItems protected...
        private void RefreshCombo(ComboBox cb)
        {
            var selectedIndex = cb.SelectedIndex;
            cb.SelectedIndex = -1;            
            MethodInfo dynMethod = cb.GetType().GetMethod("RefreshItems", BindingFlags.NonPublic | BindingFlags.Instance);
            dynMethod.Invoke(cb, null);
            cb.SelectedIndex = selectedIndex;

        }

        private bool InputValido()
        {
            string ErrMsg = "";

            if (((Trimestre)comboBox1.SelectedItem).Qn == -1)
                ErrMsg += "Debe seleccionar un trimestre\n";
            if (((Listado)comboBox2.SelectedItem).Id == -1)
                ErrMsg += "Debe seleccionar un listado\n";

            bool Valido = ErrMsg.Equals("");
            if (!Valido)
                MessageBox.Show(ErrMsg, "ERROR");
            return Valido;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!InputValido())
                return;

            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            LoadContent();
        }
    }
}
