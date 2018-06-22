using FrbaHotel.Database;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrbaHotel.Model.DAO
{
    class ConsumibleDAO
    {
        public List<Consumible> ObtenerConsumiblesFiltrados(string Descripción)
        {
            List<Consumible> Consumibles = new List<Consumible>();

            foreach (var row in DatabaseConnection.GetInstance().ExecuteProcedure("OBTENER_CONSUMIBLES_FILTRADOS",
                new SqlParameter("@descripcion", Descripción)))
            {
                Consumible c = new Consumible(
                    Convert.ToInt32(row["id_consumible"]),
                    Convert.ToString(row["descripcion_consumible"]),
                    Convert.ToDouble(row["precio_consumible"])
                    );

                Consumibles.Add(c);
            }

            return Consumibles;
        }
    }
}
