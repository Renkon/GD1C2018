using FrbaHotel.Database;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FrbaHotel.Model.DAO
{
    public class FuncionalidadDAO
    {
        public List<Funcionalidad> ObtenerFuncionalidades()
        {
            List<Funcionalidad> Funcionalidades = new List<Funcionalidad>();

            foreach (var row in DatabaseConnection.GetInstance()
                .ExecuteProcedure("OBTENER_FUNCIONALIDADES"))
            {
                Funcionalidad f = new Funcionalidad(
                    Convert.ToInt32(row["id_funcionalidad"]),
                    Convert.ToString(row["descripcion_funcionalidad"])
                    );

                Funcionalidades.Add(f);
            }

            return Funcionalidades;
        }

        public List<int> ObtenerIdsFuncionalidadesDeRol(Rol Rol)
        {
            List<int> Ids = new List<int>();

            SqlParameter param = new SqlParameter("@id_rol", Rol.Id);

            foreach (var row in DatabaseConnection.GetInstance()
                .ExecuteProcedure("OBTENER_FUNCIONALIDADES_DE_ROL", param))
            {
                Ids.Add(Convert.ToInt32(row["id_funcionalidad"]));
            }

            return Ids;
        }
    }
}
