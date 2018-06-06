using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrbaHotel.Model
{
    public class Funcionalidad
    {
        public int Id { get; private set; }
        public string Descripción { get; private set; }

        public Funcionalidad(int Id, string Descripción)
        {
            this.Id = Id;
            this.Descripción = Descripción;
        }

        public override string ToString()
        {
            return Descripción;
        }
    }
}
