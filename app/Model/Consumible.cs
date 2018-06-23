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

        public override bool Equals(object obj)
        {
            var con = obj as Consumible;
            if (con == null)
                return false;

            return con.Id == this.Id;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
