using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrbaHotel.Model
{
    class CierreTemporalHotel
    {
        public int? Id { get; private set; }
        public DateTime Inicio { get; set; }
        public DateTime Fin { get; set; }
        public Hotel Hotel { get; set; }
        public string Descripción { get; set; }

        public CierreTemporalHotel(int? Id, DateTime Inicio, DateTime Fin, Hotel Hotel, string Descripción)
        {
            this.Id = Id;
            this.Inicio = Inicio;
            this.Fin = Fin;
            this.Hotel = Hotel;
            this.Descripción = Descripción;
        }
    }
}
