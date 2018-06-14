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
<<<<<<< HEAD

        public List<Pais> ObtenerPaises()
        {
            List<Pais> Paises = new List<Pais>();

            foreach (var row in DatabaseConnection.GetInstance()
                .ExecuteProcedure("OBTENER_PAISES"))
            {
                Pais p = new Pais(
                    Convert.ToInt32(row["id_pais"]),
                    Convert.ToString(row["nombre_pais"])
                );

                Paises.Add(p);
            }

            return Paises;
        }
=======
>>>>>>> 14f9c716a57e47a597a427b60a7e7d200e192d0b
    }
}
