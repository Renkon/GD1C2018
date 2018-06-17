using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrbaHotel.Model
{
    public class TipoHabitacion
    {
        public int Id { get; private set; }
        public string Descripción { get; set; }
        public double Porcentual { get; set; }
        public int Huéspedes { get; set; }

        public TipoHabitacion(int Id, string Descripción, double Porcentual, int Huéspedes)
        {
            this.Id = Id;
            this.Descripción = Descripción;
            this.Porcentual = Porcentual;
            this.Huéspedes = Huéspedes;
        }

        public override string ToString()
        {
            return Descripción;
        }

        public override bool Equals(object obj)
        {
            var hab = obj as TipoHabitacion;
            if (hab == null)
                return false;

            return this.Id == hab.Id &&
                    this.Descripción.Equals(hab.Descripción) &&
                    this.Porcentual.Equals(hab.Porcentual) &&
                    this.Huéspedes.Equals(hab.Huéspedes);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
