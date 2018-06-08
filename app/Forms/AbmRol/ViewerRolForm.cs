using FrbaHotel.AbmRol;
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

namespace FrbaHotel.Forms.AbmRol
{
    public partial class ViewerRolForm : Form
    {

        private FormType type;
        private bool SoloActivos;
        private string ButtonText;

        public ViewerRolForm(FormType type)
        {
            this.type = type;

            InitializeComponent();

            ApplyType();
            LoadComboBox();
            RefreshGrid();
        }

        public void RefreshGrid()
        {
            PopulateDataGrid(SoloActivos, ButtonText);
        }

        private void LoadComboBox()
        {
            List<Funcionalidad> Funcionalidades = new FuncionalidadDAO().ObtenerFuncionalidades();
            Funcionalidad dummy = new Funcionalidad(-1, "- ninguna seleccionada -");
            comboBox1.Items.Add(dummy);
            comboBox1.Items.AddRange(Funcionalidades.ToArray());

            comboBox1.SelectedItem = dummy;
        }

        private void PopulateDataGrid(bool SoloActivos, string ButtonText)
        {
            List<Rol> Roles = new RolDAO().ObtenerRolesFiltrado(textBox1.Text, (Funcionalidad) comboBox1.SelectedItem, SoloActivos);

            dataGridView1.Rows.Clear();
            
            foreach (var rol in Roles)
            {
                dataGridView1.Rows.Add(rol.Nombre, rol.Estado ? "Activo" : "Deshabilitado");
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Tag = rol;
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[2].Value = ButtonText;
            }
        }

        private void ApplyType()
        {
            switch (type)
            {
                case FormType.Modify:
                    SoloActivos = false;
                    ButtonText = "Modificar";
                    this.Text = "Visualizar roles para modificar";
                break;
                case FormType.Delete:
                    SoloActivos = true;
                    ButtonText = "Borrar";
                    this.Text = "Visualizar roles para borrar";
                break;
            }        
        }

        // Ejecutado al aplicar el filtro
        private void button2_Click(object sender, EventArgs e)
        {
            PopulateDataGrid(SoloActivos, ButtonText);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            comboBox1.SelectedIndex = 0;
            button2_Click(null, null); // invocamos el efecto de 'buscar'
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView) sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0) // Clickeó el botón?
            {
                new RolForm(type, (Rol)dataGridView1.SelectedRows[0].Tag, this).ShowDialog();
            }
        }
    }
}
