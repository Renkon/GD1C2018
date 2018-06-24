using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrbaHotel.Model.Custom
{
    public class Listado
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Procedure { get; set; }
        public string[] Columns { get; set; }
        public string[] DbCols { get; set; }

        public Listado(int Id, string Nombre, string Procedure, string[] Columns, string[] DbCols)
        {
            this.Id = Id;
            this.Nombre = Nombre;
            this.Procedure = Procedure;
            this.Columns = Columns;
            this.DbCols = DbCols;
        }

        public Listado(int Id)
        {
            this.Id = Id;
        }

        public override string ToString()
        {
            if (Id == -1) return " - Seleccione un listado - ";

            return "Generar listado de " + Nombre;
        }
    }
}
