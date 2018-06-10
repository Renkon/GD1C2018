using FrbaHotel.Model;
using FrbaHotel.Model.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrbaHotel
{
    public static class Session
    {
        public static Usuario User { set; get; }
        public static Rol Rol { set; get; }
        public static Hotel Hotel { set; get; }

        public static void InitGuest()
        {
            User = new UsuarioDAO().ObtenerUsuarioDummy();
            Rol = new RolDAO().ObtenerRolGuest();
        }
    }
}
