using FrbaHotel.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrbaHotel.Model.DAO
{
    public class TipoDocumentoDAO
    {
        public List<TipoDocumento> ObtenerTiposDocumento()
        {
            List<TipoDocumento> TiposDocumento = new List<TipoDocumento>();

            foreach (var row in DatabaseConnection.GetInstance()
                .ExecuteProcedure("OBTENER_TIPOS_DOCUMENTO"))
            {
                TipoDocumento td = new TipoDocumento(
                    Convert.ToInt32(row["id_tipo_documento"]),
                    Convert.ToString(row["nombre_tipo_documento"]),
                    Convert.ToString(row["sigla_tipo_documento"])
                );

                TiposDocumento.Add(td);
            }

            return TiposDocumento;
        }
    }
}
