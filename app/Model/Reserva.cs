using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrbaHotel.Model
{
    public class Reserva
    {
        public int? Id { get; set; }
        public DateTime Fecha_Realización { get; set; }
        public DateTime Fecha_Inicio { get; set; }
        public DateTime Fecha_Fin { get; set; }
        public Cliente Cliente { get; set; }
        public Regimen Regimen { get; set; }
        public EstadoReserva EstadoReserva { get; set; }
        public List<Habitacion> Habitaciones { get; set; }

        public Reserva(int? Id, DateTime Fecha_Realización, DateTime Fecha_Inicio,
            DateTime Fecha_Fin, Cliente Cliente, Regimen Regimen, EstadoReserva EstadoReserva,
            List<Habitacion> Habitaciones)
        {
            this.Id = Id;
            this.Fecha_Realización = Fecha_Realización;
            this.Fecha_Inicio = Fecha_Inicio;
            this.Fecha_Fin = Fecha_Fin;
            this.Cliente = Cliente;
            this.Regimen = Regimen;
            this.EstadoReserva = EstadoReserva;
            this.Habitaciones = Habitaciones;
        }
    }
}
