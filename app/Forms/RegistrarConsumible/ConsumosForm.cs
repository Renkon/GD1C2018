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
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ConsumibleForm Form = new ConsumibleForm(FormType.Add, estadia, null);
            if (Form.ShowDialog() == DialogResult.OK)
            {
                
            }
            Form.Close();
            Form.Dispose();

            // 

        }
    }
}
