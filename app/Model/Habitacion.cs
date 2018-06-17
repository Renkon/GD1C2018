using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrbaHotel.Model
{
    public class Habitacion
    {
        public int? Id { get; private set; }
        public Hotel Hotel { get; set; }
        public int Número { get; set; }
        public int Piso { get; set; }
        public string Ubicación { get; set; }
        public TipoHabitacion TipoHabitación { get; set; }
        public string Descripción { get; set; }

        public Habitacion(int? Id, Hotel Hotel, int Número, int Piso,
            string Ubicación, TipoHabitacion TipoHabitación, string Descripción)
        {
            this.Id = Id;
            this.Hotel = Hotel;
            this.Número = Número;
            this.Piso = Piso;
            this.Ubicación = Ubicación;
            this.TipoHabitación = TipoHabitación;
            this.Descripción = Descripción;
        }
    }
}
