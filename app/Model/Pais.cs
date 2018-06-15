using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrbaHotel.Model
{
    public class Pais
    {
        public int Id { get; private set; }
        public string Nombre { get; set; }

        public Pais(int Id, string Nombre)
        {
            this.Id = Id;
            this.Nombre = Nombre;
        }

        public override string ToString()
        {
            return Nombre;
        }

        public override bool Equals(object obj)
        {
            var pais = obj as Pais;
            if (pais == null)
                return false;

            return this.Id == pais.Id &&
                    this.Nombre.Equals(pais.Nombre);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
