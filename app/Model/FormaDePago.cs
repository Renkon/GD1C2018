using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrbaHotel.Model
{
    public class FormaDePago
    {
        public int Id { get; private set; }
        public string Descripción { get; set; }

        public FormaDePago(int Id, string Descripción)
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
