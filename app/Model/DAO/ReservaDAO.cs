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
    public class ReservaDAO
    {
        public bool InsertarReserva(Reserva reserva)
        {
            try
            {
                int idReserva = Convert.ToInt32(DatabaseConnection.GetInstance()
                    .ExecuteProcedureScalar("INSERTAR_RESERVA", GenerateParams(reserva)));
                LogUtils.LogInfo("Se creó reserva con ID " + idReserva);
                MessageBox.Show("Se guardó la reserva con código " + idReserva + 
                    "\nEste código será necesario para realizar cambios, de ser necesario", "INFO");
                return true;
            }
            catch (Exception Ex)
            {
                LogUtils.LogError(Ex);
                MessageBox.Show("Hubo un error al intentar agregar una reserva. Revise el log", "ERROR");
                return false;
            }
        }

        public bool ModificarReserva(Reserva reserva)
        {
            try
            {
                DatabaseConnection.GetInstance().ExecuteProcedureNonQuery(
                    "MODIFICAR_RESERVA", GenerateParams(reserva));
                LogUtils.LogInfo("Se modificó reserva con ID " + reserva.Id);
                MessageBox.Show("Se modificó la reserva con código " + reserva.Id, "INFO");
                return true;
            }
            catch (Exception Ex)
            {
                LogUtils.LogError(Ex);
                MessageBox.Show("Hubo un error al intentar modificar una reserva. Revise el log", "ERROR");
                return false;
            }
        }

        public Reserva ObtenerReserva(int Id)
        {
            Reserva r = new Reserva(-1);

            foreach (var row in DatabaseConnection.GetInstance().
                ExecuteProcedure("OBTENER_RESERVA", new SqlParameter("@id_reserva", Id), 
                new SqlParameter("@today", Config.GetInstance().GetCurrentDate())))
            {
                r.Id = Id;
                r.Fecha_Realización = Convert.ToDateTime(row["fecha_realizacion_reserva"]);
                r.Fecha_Inicio = Convert.ToDateTime(row["fecha_inicio_reserva"]);
                r.Fecha_Fin = Convert.ToDateTime(row["fecha_fin_reserva"]);
                r.TiposHabitaciones = new TipoHabitacionDAO().ObtenerTiposHabitacionDeReserva(r);
                r.Regimen = new Regimen(Convert.ToInt32(row["id_regimen"]));
            }

            return r;
        }

        private SqlParameter[] GenerateParams(Reserva reserva)
        {
            List<SqlParameter> Params = new List<SqlParameter>();

            DataTable Habitaciones = DatabaseUtils
                .ConvertToDataTable<Habitacion>(reserva.Habitaciones, "Id");

            Params.Add(new SqlParameter("@id_rol_user", Session.Rol.Id));
            Params.Add(new SqlParameter("@fecha_inicio", reserva.Fecha_Inicio));
            Params.Add(new SqlParameter("@fecha_fin", reserva.Fecha_Fin));

            if (reserva.Id == null)
            {
                Params.Add(new SqlParameter("@id_cliente", reserva.Cliente.Id));
                Params.Add(new SqlParameter("@fecha_realizacion", reserva.Fecha_Realización));
            }
            else
            {
                Params.Add(new SqlParameter("@fecha_hoy", Config.GetInstance().GetCurrentDate()));
                Params.Add(new SqlParameter("@id_reserva", reserva.Id));
            }

            Params.Add(new SqlParameter("@id_regimen", reserva.Regimen.Id));
            Params.Add(new SqlParameter("@habitaciones", Habitaciones));
            Params.Add(new SqlParameter("@id_usuario", Session.User.Id));

            return Params.ToArray();
        }
    }
}
