using FrbaHotel.Database;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrbaHotel.Model.Custom.DAO
{
    public class ListadoDAO
    {
        public List<string[]> ObtenerListado(Listado listado, Trimestre trimestre)
        {
            List<string[]> content = new List<string[]>();

            foreach (var row in DatabaseConnection.GetInstance().ExecuteProcedure(listado.Procedure,
                GetParams(listado, trimestre)))
            {
                string[] contentRow = new string[listado.Columns.Length];
                int i = 0;
                foreach (string dbCol in listado.DbCols)
                    contentRow[i++] = Convert.ToString(row[dbCol]);

                content.Add(contentRow);
            }

            return content;
        }

        public SqlParameter[] GetParams(Listado listado, Trimestre trimestre)
        {
            List<SqlParameter> Params = new List<SqlParameter>();

            Params.Add(new SqlParameter("@inicio", trimestre.Inicio_Trimestre));
            Params.Add(new SqlParameter("@fin", trimestre.Fin_Trimestre));

            if (listado.WithTodayDate)
                Params.Add(new SqlParameter("@fechaHoy",
                    Config.GetInstance().GetCurrentDate()));

            return Params.ToArray();
        }
    }
}
