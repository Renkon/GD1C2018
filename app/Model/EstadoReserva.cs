using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrbaHotel.Model
{
    public class EstadoReserva
    {
        public int Id { get; private set; }
        public string Descripción { get; set; }

        public EstadoReserva(int Id, string Descripción)
        {
            this.Id = Id;
            this.Descripción = Descripción;
        }

        public EstadoReserva(int Id)
        {
            this.Id = Id;
        }
    }
}
