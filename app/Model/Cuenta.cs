using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrbaHotel.Model
{
    public class Cuenta
    {
        public string Usuario { get; set; }
        public string Contraseña { get; set; }
        public int CantidadIntentos { get; set; }

        public Cuenta(string Usuario, string Contraseña, int CantidadIntentos)
        {
            this.Usuario = Usuario;
            this.Contraseña = Contraseña;
            this.CantidadIntentos = CantidadIntentos;
        }
    }
}
