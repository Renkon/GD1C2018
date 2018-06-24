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
        public int ObtenerCantidadReservasEnPeriodoDeHotel(DateTime Inicio, DateTime Fin, Hotel Hotel)
        {
            return Convert.ToInt32(DatabaseConnection.GetInstance().ExecuteProcedureScalar("OBTENER_CANTIDAD_RESERVAS_EN_PERIODO_HOTEL",
                new SqlParameter("@fecha_inicio", Inicio),
                new SqlParameter("@fecha_fin", Fin),
                new SqlParameter("@id_hotel", Hotel.Id)));
        }

        public Reserva ObtenerReservaAptaEstadia(int Id)
        {
            Reserva r = new Reserva(-1);

            Dictionary<int, Regimen> Regimenes = new Dictionary<int, Regimen>();
            List<Regimen> tempRegs = new RegimenDAO().ObtenerRegimenes();
            foreach (var Reg in tempRegs)
                Regimenes.Add(Reg.Id, Reg);

            foreach (var row in DatabaseConnection.GetInstance().
                ExecuteProcedure("OBTENER_RESERVA_APTA_ESTADIA", 
                new SqlParameter("@id_reserva", Id),
                new SqlParameter("@today", Config.GetInstance().GetCurrentDate()),
                new SqlParameter("@id_rol_user", Session.Rol.Id),
                new SqlParameter("@id_usuario", Session.User.Id)))
            {
                r.Id = Id;
                r.Fecha_Realización = Convert.ToDateTime(row["fecha_realizacion_reserva"]);
                r.Fecha_Inicio = Convert.ToDateTime(row["fecha_inicio_reserva"]);
                r.Fecha_Fin = Convert.ToDateTime(row["fecha_fin_reserva"]);
                r.Habitaciones = new HabitacionDAO().ObtenerHabitacionesDeReserva(r);
                r.Regimen = Regimenes[Convert.ToInt32(row["id_regimen"])];
                r.EstadoReserva = new EstadoReserva(Convert.ToInt32(row["id_estado_reserva"]));
            }

            return r;
        }

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

        public bool CancelarReseva(Reserva reserva, String motivo)
        {
            try
            {
                DatabaseConnection.GetInstance()
                    .ExecuteProcedureNonQuery("CANCELAR_RESERVA", GenerateParamsCancel(reserva, motivo));
                LogUtils.LogInfo("Se canceló reserva con ID " + reserva.Id);
                MessageBox.Show("Se canceló la reserva con código " + reserva.Id, "INFO");
                return true;
            }
            catch (Exception Ex)
            {
                LogUtils.LogError(Ex);
                MessageBox.Show("Hubo un error al intentar cancelar una reserva. Revise el log", "ERROR");
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

        private SqlParameter[] GenerateParamsCancel(Reserva reserva, string motivo)
        {
            List<SqlParameter> Params = new List<SqlParameter>();

            Params.Add(new SqlParameter("@id_rol_user", Session.Rol.Id));
            Params.Add(new SqlParameter("@id_reserva", reserva.Id));
            Params.Add(new SqlParameter("@motivo_cancelacion_reserva", motivo));
            Params.Add(new SqlParameter("@fecha_cancelacion_reserva", Config.GetInstance().GetCurrentDate()));
            Params.Add(new SqlParameter("@id_usuario", Session.User.Id));

            return Params.ToArray();
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
