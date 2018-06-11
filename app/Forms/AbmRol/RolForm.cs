using FrbaHotel.Database;
using FrbaHotel.Forms;
using FrbaHotel.Forms.AbmRol;
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
        private Rol rol;
        private FormType type;
        private ViewerRolForm parent;

        public RolForm(FormType type, Rol rol, ViewerRolForm parent)
        {
            this.components = new System.ComponentModel.Container();
            this.type = type;
            this.rol = rol;
            this.parent = parent;

            InitializeComponent();
            ApplyType();
            LoadContent();

            PopulateLists();
        }

        private void PopulateLists()
        {
            List<Funcionalidad> Funcionalidades = new FuncionalidadDAO().ObtenerFuncionalidades();
            List<int> IdsFuncionalidadesRol = rol != null ? 
                new FuncionalidadDAO().ObtenerIdsFuncionalidadesDeRol(rol) : new List<int>();
            this.listBox1.Items.AddRange(Funcionalidades
                .Where(f => IdsFuncionalidadesRol.Contains(f.Id)).ToArray());
            this.listBox2.Items.AddRange(Funcionalidades
                .Where(f => !IdsFuncionalidadesRol.Contains(f.Id)).ToArray());
        }

        private void LoadContent()
        {
            if (rol != null)
            {
                this.textBox1.Text = rol.Nombre;
                this.checkBox1.Checked = rol.Estado;
                this.checkBox1.Enabled = !rol.Estado;
            }
        }

        // Ejecuta el proceso que corresponda (según sea alta, modificación o borrado)
        private void button1_Click(object sender, EventArgs e)
        {
            string Nombre = textBox1.Text;
            bool Enabled = checkBox1.Checked;
            List<Funcionalidad> Funcionalidades = this.listBox1.Items.Cast<Funcionalidad>().ToList();

            switch (type)
            {
                case FormType.Add:
                    if (!InputValido(Funcionalidades, Nombre))
                        return;

                    Rol NewRol = new Rol(null, Nombre, Enabled, Funcionalidades);
                    if (new RolDAO().InsertarNuevoRol(NewRol)) // creó el rol?
                        this.Close();
                break;
                case FormType.Modify:
                if (!InputValido(Funcionalidades, Nombre))
                    return;

                    rol.Nombre = Nombre;
                    rol.Funcionalidades = Funcionalidades;
                    rol.Estado = Enabled;

                    if (new RolDAO().ModificarRol(rol))
                    {
                        parent.RefreshGrid();
                        this.Close();
                    }
                break;
                case FormType.Delete:
                    if (new RolDAO().DeshabilitarRol(rol))
                    {
                        parent.RefreshGrid();
                        this.Close();
                    }
                break;
            }
        }

        private bool InputValido(List<Funcionalidad> Funcionalidades, string Nombre)
        {
            string ErrMsg = "";
            if (Funcionalidades.Count < 1)
                ErrMsg += "Debe seleccionar al menos una funcionalidad\n";
            if (Nombre.Equals(""))
                ErrMsg += "Debe ingresar el nombre del rol\n";

            bool Valido = ErrMsg.Equals("");
            if (!Valido)
                MessageBox.Show(ErrMsg, "ERROR");
            return Valido;
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

            // Deniego la posibilidad de que se pueda mover un rol relac. a usuarios.
            if (fs.Select(f => f.Id).Intersect(new int[]{7, 8, 9}).Any())
            {
                MessageBox.Show("Los roles de usuario no se pueden modificar!", "ERROR");
                return;
            }
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

        private void ApplyType()
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
                    break;
                case FormType.Delete:
                    this.button1.Text = "Eliminar rol";
                    this.Text = "Eliminar rol";
                    this.checkBox1.Visible = false;
                    this.textBox1.Enabled = false;
                    this.button2.Enabled = false;
                    this.button3.Enabled = false;
                    break;
            }
        }
    }
}
