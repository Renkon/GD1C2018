using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrbaHotel.Model
{
    public class Consumible
    {
        public int Id { get; private set; }
        public string Descripción { get; set; }
        public double Precio { get; set; }

        public Consumible(int Id, string Descripción, double Precio)
        {
            this.Id = Id;
            this.Descripción = Descripción;
            this.Precio = Precio;
        }

        public override string ToString()
        {
            return Descripción + " - Precio: USD " + Precio;
        }
    }
}
