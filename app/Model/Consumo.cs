using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrbaHotel.Model
{
    public class Consumo
    {
        public int? Id { get; set; }
        public Consumible Consumible { get; set; }
        public Estadia Estadia { get; set; }
        public Habitacion Habitacion { get; set; }
        public DateTime Fecha { get; set; }
        public int Cantidad { get; set; }

        public Consumo(int? Id, Consumible Consumible, Estadia Estadia, Habitacion Habitacio, DateTime Fecha, int Cantidad)
        {
            this.Id = Id;
            this.Consumible = Consumible;
            this.Estadia = Estadia;
            this.Habitacion = Habitacion;
            this.Fecha = Fecha;
            this.Cantidad = Cantidad;
        }
    }
}
