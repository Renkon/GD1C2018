using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrbaHotel.Model
{
    public class Usuario
    {
        public int? Id { get; private set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public TipoDocumento TipoDocumento { get; set; }
        public long Documento { get; set; }
        public string Correo { get; set; }
        public string Teléfono { get; set; }
        public string Dirección { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public bool Estado { get; set; }
        public List<Rol> Roles { get; set; }
        public List<Hotel> Hoteles { get; set; }

        // Usado para dummy user
        public Usuario(int? Id)
        {
            this.Id = Id;
        }

        // Usado normalmente
        public Usuario(int? Id, string Nombre, string Apellido, List<Rol> Roles, List<Hotel> Hoteles,
            TipoDocumento TipoDocumento, long Documento, string Correo, string Teléfono, string Dirección,
            DateTime FechaNacimiento, bool Estado)
        {
            this.Id = Id;
            this.Nombre = Nombre;
            this.Apellido = Apellido;
            this.Roles = Roles;
            this.Hoteles = Hoteles;
            this.TipoDocumento = TipoDocumento;
            this.Documento = Documento;
            this.Correo = Correo;
            this.Teléfono = Teléfono;
            this.Dirección = Dirección;
            this.FechaNacimiento = FechaNacimiento;
            this.Estado = Estado;
        }
    }
}
