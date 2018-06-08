using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrbaHotel.Model
{
    public class Hotel
    {
        public int? Id { get; private set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Teléfono { get; set; }
        public string Ciudad { get; set; }
        public string Domicilio_Calle { get; set; }
        public int Domicilio_Número { get; set; }
        public int Cantidad_Estrellas { get; set; }
        public Pais País { get; set; }
        public DateTime Fecha_Creación { get; set; }
        public int Recarga_Por_Estrellas { get; set; }

        public Hotel(int Id)
        {
            this.Id = Id;
        }

        public Hotel(int Id, string Nombre, string Correo, string Teléfono, string Ciudad, string Domicilio_Calle,
            int Domicilio_Número, int Cantidad_Estrellas, Pais País, DateTime Fecha_Creación, int Recarga_Por_Estrellas)
        {
            this.Id = Id;
            this.Nombre = Nombre;
            this.Correo = Correo;
            this.Teléfono = Teléfono;
            this.Ciudad = Ciudad;
            this.Domicilio_Calle = Domicilio_Calle;
            this.Domicilio_Número = Domicilio_Número;
            this.Cantidad_Estrellas = Cantidad_Estrellas;
            this.País = País;
            this.Fecha_Creación = Fecha_Creación;
            this.Recarga_Por_Estrellas = Recarga_Por_Estrellas;
        }

        public override string ToString()
        {
            return Nombre;
        }
    }
}
