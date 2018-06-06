using FrbaHotel.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FrbaHotel.Model.DAO
{
    class FuncionalidadDAO
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
    }
}
