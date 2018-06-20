using FrbaHotel.Database;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrbaHotel.Model.DAO
{
    class TipoHabitacionDAO
    {
        public List<TipoHabitacion> ObtenerTiposHabitacion()
        {
            List<TipoHabitacion> TiposHabitacion = new List<TipoHabitacion>();

            foreach (var row in DatabaseConnection.GetInstance()
                .ExecuteProcedure("OBTENER_TIPOS_HABITACION"))
            {
                TipoHabitacion th = new TipoHabitacion(
                    Convert.ToInt32(row["id_tipo_habitacion"]),
                    Convert.ToString(row["descripcion_tipo_habitacion"]),
                    Convert.ToDouble(row["porcentual_tipo_habitacion"]),
                    Convert.ToInt32(row["cantidad_huespedes_tipo_habitacion"])
                );

                TiposHabitacion.Add(th);
            }

            return TiposHabitacion;
        }

        public List<TipoHabitacion> ObtenerTiposHabitacionDeReserva(Reserva reserva)
        {
            List<TipoHabitacion> TiposHabitacion = new List<TipoHabitacion>();

            foreach (var row in DatabaseConnection.GetInstance()
                .ExecuteProcedure("OBTENER_TIPOS_HABITACION_RESERVA", new SqlParameter("@id_reserva", reserva.Id)))
            {
                TipoHabitacion th = new TipoHabitacion(
                    Convert.ToInt32(row["id_tipo_habitacion"]),
                    Convert.ToString(row["descripcion_tipo_habitacion"]),
                    Convert.ToDouble(row["porcentual_tipo_habitacion"]),
                    Convert.ToInt32(row["cantidad_huespedes_tipo_habitacion"])
                );

                TiposHabitacion.Add(th);
            }

            return TiposHabitacion;
        }
    }
}
