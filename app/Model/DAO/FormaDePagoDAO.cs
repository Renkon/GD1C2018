using FrbaHotel.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrbaHotel.Model.DAO
{
    public class FormaDePagoDAO
    {
        public List<FormaDePago> ObtenerFormasDePago()
        {
            List<FormaDePago> Formas = new List<FormaDePago>();

            foreach (var row in DatabaseConnection.GetInstance().ExecuteProcedure("OBTENER_FORMAS_DE_PAGO"))
            {
                FormaDePago f = new FormaDePago(
                    Convert.ToInt32(row["id_forma_de_pago"]),
                    Convert.ToString(row["descripcion_forma_de_pago"])
                    );

                Formas.Add(f);
            }

            return Formas;
        }
    }
}
