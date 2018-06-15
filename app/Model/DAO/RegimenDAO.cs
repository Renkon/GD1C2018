using FrbaHotel.Database;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrbaHotel.Model.DAO
{
    class RegimenDAO
    {
        public List<Regimen> ObtenerRegimenes()
        {
            List<Regimen> Regimenes = new List<Regimen>();

            foreach (var row in DatabaseConnection.GetInstance()
                .ExecuteProcedure("OBTENER_REGIMENES"))
            {
                Regimen r = new Regimen(
                    Convert.ToInt32(row["id_regimen"]),
                    Convert.ToString(row["descripcion_regimen"]),
                    Convert.ToDouble(row["precio_base_regimen"]),
                    Convert.ToBoolean(row["estado_regimen"])
                );

                Regimenes.Add(r);
            }

            return Regimenes;
        }

        public List<int> ObtenerIdsRegimenesHotel(Hotel Hotel)
        {
            List<int> Ids = new List<int>();

            SqlParameter param = new SqlParameter("@id_hotel", Hotel.Id);

            foreach (var row in DatabaseConnection.GetInstance()
                .ExecuteProcedure("OBTENER_REGIMENES_DE_UN_HOTEL", param))
            {
                Ids.Add(Convert.ToInt32(row["id_regimen"]));
            }

            return Ids;
        }
    }
}
