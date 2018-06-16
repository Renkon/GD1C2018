using FrbaHotel.Database;
using FrbaHotel.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
                    Convert.ToInt32(row["recarga_por_estrellas_hotel"]),
                    null
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

        public List<Hotel> ObtenerHotelesFiltrados(string Nombre, int Estrellas, string Ciudad, Pais Pais)
        {
            List<Hotel> Hoteles = new List<Hotel>();

            foreach (var row in DatabaseConnection.GetInstance().
                ExecuteProcedure("OBTENER_HOTELES_FILTRADOS", GenerateParamsFilter(Nombre,
                    Estrellas, Ciudad, Pais)))
            {
                Hotel h = new Hotel(
                    Convert.ToInt32(row["id_hotel"]),
                    Convert.ToString(row["nombre_hotel"]),
                    Convert.ToString(row["correo_hotel"]),
                    Convert.ToString(row["telefono_hotel"]),
                    Convert.ToString(row["ciudad_hotel"]),
                    Convert.ToString(row["domicilio_calle_hotel"]),
                    Convert.ToInt32(row["domicilio_numero_hotel"]),
                    Convert.ToInt32(row["cantidad_estrellas_hotel"]),
                    new Pais(Convert.ToInt32(row["id_pais"]), new PaisDAO().ObtenerNombrePais(Convert.ToInt32(row["id_pais"]))),
                    Convert.ToDateTime(row["fecha_creacion_hotel"]),
                    Convert.ToInt32(row["recarga_por_estrellas_hotel"]),
                    null
                );

                Hoteles.Add(h);
            }
            return Hoteles;
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

        public bool InsertarNuevoHotel(Hotel NuevoHotel)
        {
            try
            {
                DatabaseConnection.GetInstance()
                    .ExecuteProcedureNonQuery("AGREGAR_NUEVO_HOTEL", GenerateParamsDML(NuevoHotel));
                LogUtils.LogInfo("Se creó hotel " + NuevoHotel.Nombre);
                MessageBox.Show("Se agregó satisfactoriamente el hotel " + NuevoHotel.Nombre, "INFO");
                return true;
            }
            catch (Exception Ex)
            {
                LogUtils.LogError(Ex);
                MessageBox.Show("Hubo un error al intentar agregar un hotel. Revise el log", "ERROR");
                return false;
            }
        }

        public bool ModificarHotel(Hotel Hotel)
        {
            try
            {
                DatabaseConnection.GetInstance()
                    .ExecuteProcedureNonQuery("MODIFICAR_HOTEL", GenerateParamsDML(Hotel));
                LogUtils.LogInfo("Se modificó el hotel " + Hotel.Nombre);
                MessageBox.Show("Se modificó satisfactoriamente el hotel " + Hotel.Nombre, "INFO");
                return true;
            }
            catch (Exception Ex)
            {
                LogUtils.LogError(Ex);
                MessageBox.Show("Hubo un error al intentar modificar un hotel. Revise el log", "ERROR");
                return false;
            }
        }

        private SqlParameter[] GenerateParamsFilter(string Nombre, int Estrellas, string Ciudad,
            Pais Pais)
        {
            List<SqlParameter> Params = new List<SqlParameter>();

            Params.Add(new SqlParameter("@nombre", Nombre));
            Params.Add(new SqlParameter("@estrellas", Estrellas));
            Params.Add(new SqlParameter("@ciudad", Ciudad));
            Params.Add(new SqlParameter("@id_pais", Pais != null ? Pais.Id : -1));

            return Params.ToArray();
        }

        private SqlParameter[] GenerateParamsDML(Hotel Hotel)
        {
            List<SqlParameter> Params = new List<SqlParameter>();

            DataTable Regimenes = DatabaseUtils
                .ConvertToDataTable<Regimen>(Hotel.Regimenes, "Id");

            Params.Add(new SqlParameter("@id_rol_user", Session.Rol.Id));

            if (Hotel.Id != null)
                Params.Add(new SqlParameter("@id_hotel", Hotel.Id));

            Params.Add(new SqlParameter("@nombre_hotel", Hotel.Nombre));
            Params.Add(new SqlParameter("@correo_hotel", Hotel.Correo));
            Params.Add(new SqlParameter("@telefono_hotel", Hotel.Teléfono));
            Params.Add(new SqlParameter("@ciudad_hotel", Hotel.Ciudad));
            Params.Add(new SqlParameter("@domicilio_calle_hotel", Hotel.Domicilio_Calle));
            Params.Add(new SqlParameter("@domicilio_numero_hotel", Hotel.Domicilio_Número));
            Params.Add(new SqlParameter("@cantidad_estrellas_hotel", Hotel.Cantidad_Estrellas));
            Params.Add(new SqlParameter("@id_pais", Hotel.País.Id));
            Params.Add(new SqlParameter("@fecha_creacion_hotel", Hotel.Fecha_Creación));
            Params.Add(new SqlParameter("@recarga_por_estrellas_hotel", Hotel.Recarga_Por_Estrellas));
            Params.Add(new SqlParameter("@regimenes", Regimenes));

            return Params.ToArray();
        }
    }
}
