using FrbaHotel.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FrbaHotel.Model
{
    public class Rol
    {
        public int? Id { get; private set; }
        public string Nombre { get; set; }
        public bool Estado { get; set; }
        public List<Funcionalidad> Funcionalidades { get; set; }

        public Rol(int Id)
        {
            this.Id = Id;
        }

        public Rol(int? Id, string Nombre, bool Estado, List<Funcionalidad> Funcionalidades)
        {
            this.Id = Id;
            this.Nombre = Nombre;
            this.Estado = Estado;
            this.Funcionalidades = Funcionalidades;
        }

        public override String ToString()
        {
            return Nombre;
        }
    }
}
