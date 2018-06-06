using FrbaHotel.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrbaHotel.Model.DAO
{
    class UsuarioDAO
    {
        public Usuario ObtenerUsuarioDummy()
        {
            var DummyId = Convert.ToInt32(DatabaseConnection.GetInstance()
                .ExecuteProcedureScalar("OBTENER_USUARIO_DUMMY"));
          
            return new Usuario(DummyId);
        }
    }
}
