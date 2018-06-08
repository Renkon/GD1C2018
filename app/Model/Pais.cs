using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrbaHotel.Model
{
    public class Pais
    {
        public int Id { get; private set; }
        public string Nombre { get; set; }

        public Pais(int Id, string Nombre)
        {
            this.Id = Id;
            this.Nombre = Nombre;
        }
    }
}
