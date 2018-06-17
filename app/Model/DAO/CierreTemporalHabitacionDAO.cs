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
    public class CierreTemporalHabitacionDAO
    {
        public bool InsertarCierreTemporalHabitacion(CierreTemporalHabitacion Cierre)
        {
            try
            {
                DatabaseConnection.GetInstance()
                    .ExecuteProcedureNonQuery("INSERTAR_CIERRE_TEMPORAL_HABITACION", GenerateParamsDML(Cierre));
                LogUtils.LogInfo("Se cerrará temporalmente la habitación " + Cierre.Habitacion.Número + " del hotel " + Session.Hotel.Nombre);
                MessageBox.Show("Se cerrará temporalmente la habitación " + Cierre.Habitacion.Número + " del hotel " + Session.Hotel.Nombre, "INFO");
                return true;
            }
            catch (SqlException Sex)
            {
                if (Sex.Message.StartsWith("50001"))
                    MessageBox.Show("Esa habitación ya tiene un cierre temporal durante ese margen de fechas", "ERROR");
                else
                    MessageBox.Show("Error SQL desconocido " + Sex.Message, "ERROR");

                return false;
            }
            catch (Exception Ex)
            {
                LogUtils.LogError(Ex);
                MessageBox.Show("Hubo un error al intentar cerrar una habitación. Revise el log", "ERROR");
                return false;
            }

        }


        private SqlParameter[] GenerateParamsDML(CierreTemporalHabitacion Cierre)
        {
            List<SqlParameter> Params = new List<SqlParameter>();

            Params.Add(new SqlParameter("@id_rol_user", Session.Rol.Id));
            Params.Add(new SqlParameter("@inicio", Cierre.Inicio));
            Params.Add(new SqlParameter("@fin", Cierre.Fin));
            Params.Add(new SqlParameter("@id_habitacion", Cierre.Habitacion.Id));
            Params.Add(new SqlParameter("@motivo", Cierre.Descripción));

            return Params.ToArray();
        }
    }
}
