using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrbaHotel.Model
{
    public class Estadia
    {
        public int? Id { get; set; }
        public DateTime? Fecha_Inicio { get; set; }
        public DateTime? Fecha_Fin { get; set; }
        public Usuario Usuario_Inicio { get; set; }
        public Usuario Usuario_Fin { get; set; }
        public List<Cliente> Clientes { get; set; }
        public List<Habitacion> Habitaciones { get; set; }
        public bool Consumos_Cerrados { get; set; }

        public Estadia(int? Id, DateTime? Fecha_Inicio, DateTime? Fecha_Fin)
        {
            this.Id = Id;
            this.Fecha_Inicio = Fecha_Inicio;
            this.Fecha_Fin = Fecha_Fin;
        }

        public Estadia(int? Id, DateTime? Fecha_Inicio, DateTime? Fecha_Fin, bool Consumos_Cerrados)
        {
            this.Id = Id;
            this.Fecha_Inicio = Fecha_Inicio;
            this.Fecha_Fin = Fecha_Fin;
            this.Consumos_Cerrados = Consumos_Cerrados;
        }
    }
}
