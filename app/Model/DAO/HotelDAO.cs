using FrbaHotel.Database;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrbaHotel.Model.DAO
{
    public class HotelDAO
    {
        public List<Hotel> ObtenerHoteles()
        {
            List<Hotel> Hoteles = new List<Hotel>();

            foreach (var row in DatabaseConnection.GetInstance().
                ExecuteProcedure("OBTENER_HOTELES"))
            {
                Hotel h = new Hotel(
                    Convert.ToInt32(row["id_hotel"]) ,
                    Convert.ToString(row["nombre_hotel"]),
                    Convert.ToString(row["correo_hotel"]),
                    Convert.ToString(row["telefono_hotel"]),
                    Convert.ToString(row["ciudad_hotel"]),
                    Convert.ToString(row["domicilio_calle_hotel"]),
                    Convert.ToInt32(row["domicilio_numero_hotel"]),
                    Convert.ToInt32(row["cantidad_estrellas_hotel"]),
                    new Pais(Convert.ToInt32(row["id_pais"]), 
                        new PaisDAO().ObtenerNombrePais(Convert.ToInt32(row["id_pais"]))),
                    Convert.ToDateTime(row["fecha_creacion_hotel"]),
                    Convert.ToInt32(row["recarga_por_estrellas_hotel"])
                );
                Hoteles.Add(h);
            }

            return Hoteles;
        }

        public List<Hotel> ObtenerHotelesDeUsuario(Usuario Usuario)
        {
            List<Hotel> HotelesUsuario = new List<Hotel>();
            List<Hotel> HotelesTemp = ObtenerHoteles();
            Dictionary<int, Hotel> Hoteles = new Dictionary<int, Hotel>();

            // Filleo el dictionary
            foreach (var rol in HotelesTemp)
                Hoteles.Add(rol.Id.Value, rol);

            List<int> Ids = ObtenerIdsHotelesUsuario(Usuario);

            // Filleo la lista final
            foreach (var Id in Ids)
                HotelesUsuario.Add(Hoteles[Id]);

            return HotelesUsuario;
        }

        public List<int> ObtenerIdsHotelesUsuario(Usuario Usuario)
        {
            List<int> Ids = new List<int>();

            SqlParameter param = new SqlParameter("@id_usuario", Usuario.Id);

            foreach (var row in DatabaseConnection.GetInstance()
                .ExecuteProcedure("OBTENER_HOTELES_DE_UN_USUARIO", param))
            {
                Ids.Add(Convert.ToInt32(row["id_hotel"]));
            }

            return Ids;
        }
    }
}
