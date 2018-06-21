using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrbaHotel.Model
{
    public class Cliente
    {
        public int? Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public TipoDocumento TipoDocumento { get; set; }
        public long Documento { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string Calle { get; set; }
        public int Nro { get; set; }
        public int Piso { get; set; }
        public string Departamento { get; set; }
        public string Ciudad { get; set; }
        public Pais Pais { get; set; }
        public string Nacionalidad { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public bool Estado { get; set; }

        public Cliente(int? Id, string Nombre, string Apellido, TipoDocumento TipoDocumento, long Documento, string Correo,
                        string Telefono, string Calle, int Nro, int Piso, string Departamento,
                        string Ciudad, Pais Pais, string Nacionalidad, DateTime FechaNacimiento, bool Estado)
        {
            this.Id = Id;
            this.Nombre = Nombre;
            this.Apellido = Apellido;
            this.TipoDocumento = TipoDocumento;
            this.Documento = Documento;
            this.Correo = Correo;
            this.Telefono = Telefono;
            this.Calle = Calle;
            this.Nro = Nro;
            this.Piso = Piso;
            this.Departamento = Departamento;
            this.Ciudad = Ciudad;
            this.Pais = Pais;
            this.Nacionalidad = Nacionalidad;
            this.FechaNacimiento = FechaNacimiento;
            this.Estado = Estado;
        }

        public Cliente(int Id, string Nombre, string Apellido, TipoDocumento TipoDoc,
            long Documento, string Correo)
        {
            this.Id = Id;
            this.Nombre = Nombre;
            this.Apellido = Apellido;
            this.TipoDocumento = TipoDoc;
            this.Documento = Documento;
            this.Correo = Correo;
        }

        public override bool Equals(object obj)
        {
            Cliente cli = obj as Cliente;

            if (cli == null)
                return false;

            return this.Id == cli.Id;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }

}
