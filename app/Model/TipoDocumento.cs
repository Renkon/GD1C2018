using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrbaHotel.Model
{
    public class TipoDocumento
    {
        public int? Id { get; private set; }
        public string Nombre { get; set; }
        public string Sigla { get; set; }

        public TipoDocumento(int? Id, string Nombre, string Sigla)
        {
            this.Id = Id;
            this.Nombre = Nombre;
            this.Sigla = Sigla;
        }

        public override string ToString()
        {
            return Sigla;
        }

        public override bool Equals(object obj)
        {
            var doc = obj as TipoDocumento;
            if (doc == null)
                return false;

            return this.Id.Value == doc.Id.Value &&
                    this.Nombre.Equals(doc.Nombre) &&
                    this.Sigla.Equals(doc.Sigla);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
