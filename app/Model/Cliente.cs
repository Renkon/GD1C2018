using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrbaHotel.Model
{
    public class Cliente
    {
        public int? Id { get; private set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public TipoDocumento TipoDocumento { get; set; }
        public long Documento { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string Calle { get; set; }
        public long Nro { get; set; }
        public long Piso { get; set; }
        public string Departamento { get; set; }
        public string Ciudad { get; set; }
        public int Pais { get; set; }
        public string Nacionalidad { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public bool Estado { get; set; }

    

        public Cliente(int? Id, string Nombre, string Apellido, TipoDocumento TipoDocumento, long Documento, string Correo,
                        string Telefono, string Calle, long Nro, long Piso, string Departamento,
                        string Ciudad, int Pais, string Nacionalidad, DateTime FechaNacimiento, bool Estado)
        {
            this.Id = Id;
            this.Nombre = Nombre;
            this.Apellido = Apellido;
            this.TipoDocumento = TipoDocumento;
            this.Documento = Documento;
            this.Correo = Correo;
            if (Telefono != null)
            this.Telefono = Telefono;
            this.Calle = Calle;
            if (Nro != null)
            this.Nro = Nro;
            if (Piso != null)
            this.Piso = Piso;
            this.Departamento = Departamento;
            this.Ciudad = Ciudad;
            if(Pais != null)
                this.Pais = Pais;
            this.Nacionalidad = Nacionalidad;
            if (FechaNacimiento != null)
                this.FechaNacimiento = FechaNacimiento;
            if (Estado != null)
                this.Estado = Estado;
            else
                this.Estado = false;
        }

    }

}
