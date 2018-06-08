using FrbaHotel.Database;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrbaHotel.Model.DAO
{
    public class PaisDAO
    {
        public string ObtenerNombrePais(int IdPais)
        {
            var NombrePais = Convert.ToString(DatabaseConnection.GetInstance()
                .ExecuteProcedureScalar("OBTENER_PAIS",
                    new SqlParameter("@id_pais", IdPais)));

            return NombrePais;
        }
    }
}
