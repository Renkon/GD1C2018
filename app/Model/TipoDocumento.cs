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
    }
}
