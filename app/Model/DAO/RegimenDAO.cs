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
        public Regimen ObtenerRegimenDeEstadia(Estadia estadia)
        {
            Regimen r = null;

            foreach (var row in DatabaseConnection.GetInstance().ExecuteProcedure("OBTENER_REGIMEN_DE_ESTADIA",
                new SqlParameter("@id_estadia", estadia.Id)))
            {
                r = new Regimen(
                    Convert.ToInt32(row["id_regimen"]),
                    Convert.ToString(row["descripcion_regimen"]),
                    Convert.ToDouble(row["precio_base_regimen"]),
                    Convert.ToBoolean(row["estado_regimen"])
                );
            }

            return r;
        }

        public List<Regimen> ObtenerRegimenesEnUsoHotel(Hotel Hotel)
        {
            List<Regimen> Regimenes = new List<Regimen>();

            foreach (var row in DatabaseConnection.GetInstance().ExecuteProcedure("REGIMENES_USADOS_POR_RESERVAS_DE_HOTEL",
                new SqlParameter("@fecha_hoy", Config.GetInstance().GetCurrentDate()),
                new SqlParameter("@id_hotel", Hotel.Id)))
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

        public Regimen ObtenerRegimenAllInclusive()
        {
            return new Regimen(Convert.ToInt32(DatabaseConnection.GetInstance().
                ExecuteProcedureScalar("OBTENER_REGIMEN_ALL_INCLUSIVE")));
        }

        public Regimen ObtenerRegimenAllInclusiveModerado()
        {
            return new Regimen(Convert.ToInt32(DatabaseConnection.GetInstance().
                ExecuteProcedureScalar("OBTENER_REGIMEN_ALL_INCLUSIVE_MODERADO")));
        }

        public List<Regimen> ObtenerRegimenesActivosDeHotel(Hotel Hotel)
        {
            List<Regimen> Regimenes = new List<Regimen>();

            foreach (var row in DatabaseConnection.GetInstance()
                .ExecuteProcedure("OBTENER_REGIMENES_ACTIVOS_DE_HOTEL", new SqlParameter("@id_hotel", Hotel.Id)))
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
