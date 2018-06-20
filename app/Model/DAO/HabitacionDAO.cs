using FrbaHotel.Database;
using FrbaHotel.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FrbaHotel.Model.DAO
{
    class HabitacionDAO
    {
        public List<Habitacion> ObtenerHabitacionesFiltradas(Hotel Hotel, string Numero, string Piso, TipoHabitacion Tipo)
        {
            List<Habitacion> Habitaciones = new List<Habitacion>();

            Dictionary<int, TipoHabitacion> TiposHab = new Dictionary<int, TipoHabitacion>();

            List<TipoHabitacion> tempHabs = new TipoHabitacionDAO().ObtenerTiposHabitacion();
            foreach (var TipoHab in tempHabs)
                TiposHab.Add(TipoHab.Id, TipoHab);

            foreach (var row in DatabaseConnection.GetInstance().
                ExecuteProcedure("OBTENER_HABITACIONES_FILTRADAS", GenerateParamsFilter(Hotel,
                    Numero, Piso, Tipo)))
            {
                Habitacion h = new Habitacion(
                    Convert.ToInt32(row["id_habitacion"]),
                    Session.Hotel,
                    Convert.ToInt32(row["numero_habitacion"]),
                    Convert.ToInt32(row["piso_habitacion"]),
                    Convert.ToString(row["ubicacion_habitacion"]),
                    TiposHab[Convert.ToInt32(row["id_tipo_habitacion"])],
                    Convert.ToString(row["descripcion_habitacion"])
                );

                Habitaciones.Add(h);
            }
            return Habitaciones;
        }

        public List<Habitacion> ObtenerHabitacionesDisponiblesReserva(DateTime inicio, DateTime fin, Hotel hotel, Reserva reserva)
        {
            List<Habitacion> Habitaciones = new List<Habitacion>();

            Dictionary<int, TipoHabitacion> TiposHab = new Dictionary<int, TipoHabitacion>();

            List<TipoHabitacion> tempHabs = new TipoHabitacionDAO().ObtenerTiposHabitacion();
            foreach (var TipoHab in tempHabs)
                TiposHab.Add(TipoHab.Id, TipoHab);

            foreach (var row in DatabaseConnection.GetInstance().
                ExecuteProcedure("OBTENER_HABITACIONES_DISPONIBLES_RESERVA", 
                    GetHabitacionesDisponiblesParameters(inicio, fin, hotel, reserva)))
            {
                int Id = Convert.ToInt32(row["id_habitacion"]);

                if (Id == -1) // Hotel cerrado??
                {
                    Habitaciones.Add(new Habitacion(Id));
                    break;
                }

                Habitacion h = new Habitacion(
                    Id,
                    hotel,
                    Convert.ToInt32(row["numero_habitacion"]),
                    Convert.ToInt32(row["piso_habitacion"]),
                    Convert.ToString(row["ubicacion_habitacion"]),
                    TiposHab[Convert.ToInt32(row["id_tipo_habitacion"])],
                    Convert.ToString(row["descripcion_habitacion"])
                );

                Habitaciones.Add(h);
            }

            return Habitaciones;
        }

        public bool InsertarNuevaHabitacion(Habitacion NuevaHab)
        {
            try
            {
                DatabaseConnection.GetInstance()
                    .ExecuteProcedureNonQuery("INSERTAR_HABITACION", GenerateParamsDML(NuevaHab));
                LogUtils.LogInfo("Se creó habitación " + NuevaHab.Número + " en hotel " + Session.Hotel.Nombre);
                MessageBox.Show("Se agregó satisfactoriamente la habitación " + NuevaHab.Número + " en hotel " + Session.Hotel.Nombre, "INFO");
                return true;
            }
            catch (SqlException Sex)
            {
                LogUtils.LogError(Sex);
                if (Sex.Number == 2627)
                {
                    if (Sex.Message.Contains("UNIQUE"))
                        MessageBox.Show("No se pudo agregar la habitación.\n Ya hay una habitación con ese número en el hotel", "ERROR");
                }
                else
                    MessageBox.Show("Ha ocurrido un error al intentar insertar: " + Sex.Message);
                return false;
            }
            catch (Exception Ex)
            {
                LogUtils.LogError(Ex);
                MessageBox.Show("Hubo un error al intentar agregar una habitación. Revise el log", "ERROR");
                return false;
            }
        }

        public bool ModificarHabitacion(Habitacion Habitacion)
        {
            try
            {
                DatabaseConnection.GetInstance()
                    .ExecuteProcedureNonQuery("MODIFICAR_HABITACION", GenerateParamsDML(Habitacion));
                LogUtils.LogInfo("Se modificó habitación " + Habitacion.Número + " en hotel " + Session.Hotel.Nombre);
                MessageBox.Show("Se modificó satisfactoriamente la habitación " + Habitacion.Número + " en hotel " + Session.Hotel.Nombre, "INFO");
                return true;
            }
            catch (SqlException Sex)
            {
                LogUtils.LogError(Sex);
                if (Sex.Number == 2627)
                {
                    if (Sex.Message.Contains("UNIQUE"))
                        MessageBox.Show("No se pudo agregar la habitación.\n Ya hay una habitación con ese número en el hotel", "ERROR");
                }
                else
                    MessageBox.Show("Ha ocurrido un error al intentar insertar: " + Sex.Message);
                return false;
            }
            catch (Exception Ex)
            {
                LogUtils.LogError(Ex);
                MessageBox.Show("Hubo un error al intentar agregar una habitación. Revise el log", "ERROR");
                return false;
            }
        }

        private SqlParameter[] GenerateParamsFilter(Hotel Hotel, string Numero, string Piso,
            TipoHabitacion Tipo)
        {
            List<SqlParameter> Params = new List<SqlParameter>();

            Params.Add(new SqlParameter("@id_hotel", Hotel.Id));
            Params.Add(new SqlParameter("@numero_habitacion", Convert.ToInt32("0" + Numero)));
            Params.Add(new SqlParameter("@piso_habitacion", Convert.ToInt32("0" + Piso)));
            Params.Add(new SqlParameter("@id_tipo_habitacion", Tipo.Id));

            return Params.ToArray();
        }

        private SqlParameter[] GenerateParamsDML(Habitacion Habitacion)
        {
            List<SqlParameter> Params = new List<SqlParameter>();

            Params.Add(new SqlParameter("@id_rol_user", Session.Rol.Id));

            if (Habitacion.Id != null)
                Params.Add(new SqlParameter("@id_habitacion", Habitacion.Id));
            else
            {
                Params.Add(new SqlParameter("@id_hotel", Session.Hotel.Id));
                Params.Add(new SqlParameter("@id_tipo_habitacion", Habitacion.TipoHabitación.Id));
            }

            
            Params.Add(new SqlParameter("@numero_habitacion", Habitacion.Número));
            Params.Add(new SqlParameter("@piso_habitacion", Habitacion.Piso));
            Params.Add(new SqlParameter("@ubicacion_habitacion", Habitacion.Ubicación.Equals("") ?
                (object) DBNull.Value : Habitacion.Ubicación));
            Params.Add(new SqlParameter("@descripcion_habitacion", Habitacion.Descripción.Equals("") ?
                (object) DBNull.Value : Habitacion.Descripción));

            return Params.ToArray();
        }

        private SqlParameter[] GetHabitacionesDisponiblesParameters(DateTime inicio, DateTime fin, Hotel hotel, Reserva reserva)
        {
            List<SqlParameter> Params = new List<SqlParameter>();

            Params.Add(new SqlParameter("@id_rol", Session.Rol.Id));
            Params.Add(new SqlParameter("@today", Config.GetInstance().GetCurrentDate()));
            Params.Add(new SqlParameter("@fecha_inicio", inicio));
            Params.Add(new SqlParameter("@fecha_fin", fin));
            Params.Add(new SqlParameter("@id_hotel", hotel.Id));
            Params.Add(new SqlParameter("@id_reserva", reserva.Id));

            return Params.ToArray();
        }
    }
}
