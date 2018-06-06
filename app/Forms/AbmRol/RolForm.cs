using FrbaHotel.Database;
using FrbaHotel.Forms;
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

namespace FrbaHotel.AbmRol
{
    public partial class RolForm
    {
        private FuncionalidadDAO FuncionalidadDAO;

        public RolForm(FormType type, Rol rol)
        {
            this.components = new System.ComponentModel.Container();
            InitializeComponent();
            ApplyType(type, rol);

            FuncionalidadDAO = new FuncionalidadDAO();

            PopulateLists();
        }

        public void PopulateLists()
        {
            List<Funcionalidad> Funcionalidades = FuncionalidadDAO.ObtenerFuncionalidades();
            this.listBox2.Items.AddRange(Funcionalidades.ToArray());
        }

        // Ejecuta el proceso que corresponda (según sea alta, modificación o borrado)
        private void button1_Click(object sender, EventArgs e)
        {
            string Nombre = textBox1.Text;
            bool Enabled = checkBox1.Checked;
            List<Funcionalidad> Funcionalidades = this.listBox1.Items.Cast<Funcionalidad>().ToList();
            if (Funcionalidades.Count < 1)
            {
                MessageBox.Show("Debes seleccionar al menos una funcionalidad!", "ERROR!");
                return;
            }
            Rol NewRol = new Rol(null, Nombre, Enabled, Funcionalidades);
            if (new RolDAO().InsertarNuevoRol(NewRol)) // creó el rol?
            {
                this.Close();
            }
        }

        // Mover objeto a la lista izquierda
        private void button3_Click(object sender, EventArgs e)
        {
            MoveFuncionalidades(listBox2, listBox1);
        }

        // Mover objeto a la lista derecha
        private void button2_Click(object sender, EventArgs e)
        {
            MoveFuncionalidades(listBox1, listBox2);
        }

        // Evito que se superpongan selecciones en las listBoxes
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            CleanSelection(listBox1, listBox2);
        }

        // Evito que se superpongan selecciones en las listBoxes
        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            CleanSelection(listBox2, listBox1);
        }


        private void MoveFuncionalidades(ListBox list1, ListBox list2)
        {
            if (list1.SelectedIndex == -1)
            {
                MessageBox.Show("Debe seleccionar al menos un elemento", "ERROR");
                return; // Nada seleccionado
            }
            List<Funcionalidad> fs = list1.SelectedItems.Cast<Funcionalidad>().ToList();
            foreach (var f in fs)
            {
                list1.Items.Remove(f);
                list2.Items.Add(f);
            }
        }

        private void CleanSelection(ListBox list1, ListBox list2)
        {
            if (list1.SelectedIndex != -1)
                list2.SelectedIndex = -1;
        }

        private void ApplyType(FormType type, Rol rol)
        {
            switch (type)
            {
                case FormType.Add:
                    this.button1.Text = "Agregar rol";
                    this.Text = "Agregar rol";
                    this.checkBox1.Visible = false;
                    break;
                case FormType.Modify:
                    this.button1.Text = "Modificar rol";
                    this.Text = "Modificar rol";
                    this.checkBox1.Visible = rol.Estado;
                    break;
                case FormType.Delete:
                    this.button1.Text = "Eliminar rol";
                    this.Text = "Eliminar rol";
                    this.checkBox1.Visible = false;
                    break;
            }
        }
    }
}
