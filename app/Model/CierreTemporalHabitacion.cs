using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrbaHotel.Model
{
    public class CierreTemporalHabitacion
    {
        public int? Id { get; private set; }
        public DateTime Inicio { get; set; }
        public DateTime Fin { get; set; }
        public Habitacion Habitacion { get; set; }
        public string Descripción { get; set; }

        public CierreTemporalHabitacion(int? Id, DateTime Inicio, DateTime Fin, Habitacion Habitacion, string Descripción)
        {
            this.Id = Id;
            this.Inicio = Inicio;
            this.Fin = Fin;
            this.Habitacion = Habitacion;
            this.Descripción = Descripción;
        }
    }
}
