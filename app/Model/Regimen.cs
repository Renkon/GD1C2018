using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrbaHotel.Model
{
    public class Regimen
    {
        public int Id { get; private set; }
        public string Descripción { get; set; }
        public double PrecioBase { get; set; }
        public bool Estado { get; set; }

        public Regimen(int Id, string Descripción, double PrecioBase, bool Estado)
        {
            this.Id = Id;
            this.Descripción = Descripción;
            this.PrecioBase = PrecioBase;
            this.Estado = Estado;
        }

        public override string ToString()
        {
            return Descripción;
        }

        public override bool Equals(object obj)
        {
            var reg = obj as Regimen;
            if (reg == null)
                return false;

            return this.Id == reg.Id;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
