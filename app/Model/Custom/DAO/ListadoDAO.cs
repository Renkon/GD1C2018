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
                new SqlParameter("@inicio", trimestre.Inicio_Trimestre),
                new SqlParameter("@fin", trimestre.Fin_Trimestre)))
            {
                string[] contentRow = new string[listado.Columns.Length];
                int i = 0;
                foreach (string dbCol in listado.DbCols)
                    contentRow[i++] = Convert.ToString(row[dbCol]);

                content.Add(contentRow);
            }

            return content;
        }
    }
}
