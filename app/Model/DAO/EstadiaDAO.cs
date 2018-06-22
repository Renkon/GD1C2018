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
    public class EstadiaDAO
    {
        public Estadia ObtenerEstadia(int Id)
        {
            Estadia e = null;

            foreach (var row in DatabaseConnection.GetInstance().ExecuteProcedure("OBTENER_ESTADIA",
                new SqlParameter("@id_estadia", Id),
                new SqlParameter("@id_rol_user", Session.Rol.Id)))
            {
                e = new Estadia(Id,
                        Convert.ToDateTime(row["fecha_ingreso_estadia"]),
                        Convert.ToDateTime(row["fecha_egreso_estadia"]),
                        Convert.ToBoolean(row["consumos_cerrados"])
                    );
            }

            return e;
        }

        public bool CerrarEstadia(Estadia estadia)
        {
            estadia.Fecha_Fin = Config.GetInstance().GetCurrentDate();
            estadia.Usuario_Fin = Session.User;

            try
            {
                DatabaseConnection.GetInstance().ExecuteProcedureNonQuery("EGRESAR_ESTADIA",
                    new SqlParameter("@id_rol_user", Session.Rol.Id),
                    new SqlParameter("@id_estadia", estadia.Id),
                    new SqlParameter("@id_usuario_egreso", estadia.Usuario_Fin.Id),
                    new SqlParameter("@fecha_egreso_estadia", estadia.Fecha_Fin));
                LogUtils.LogInfo("Se cerró la estadía " + estadia.Id);
                MessageBox.Show("Se cerró satisfactoriamente la estadía " + estadia.Id
                    + "\n\nGuárdese el código, pues será necesario para registrar consumibles y la facturación", "INFO");
                return true;
            }
            catch (Exception Ex)
            {
                LogUtils.LogError(Ex);
                MessageBox.Show("Hubo un error al intentar cerrar una estadía. Revise el log", "ERROR");
                return false;
            }
        }

        public bool InsertarEstadia(Estadia estadia, Reserva reserva, List<Cliente> clientes)
        {
            estadia.Fecha_Inicio = Config.GetInstance().GetCurrentDate();
            estadia.Usuario_Inicio = Session.User;

            DataTable clientesDataTable = DatabaseUtils
                .ConvertToDataTable<Cliente>(clientes, "Id");

            try
            {
                int id = Convert.ToInt32(DatabaseConnection.GetInstance()
                    .ExecuteProcedureScalar("INGRESAR_ESTADIA",
                        new SqlParameter("@id_rol_user", Session.Rol.Id),
                        new SqlParameter("@id_reserva", reserva.Id),
                        new SqlParameter("@id_usuario_ingreso", estadia.Usuario_Inicio.Id),
                        new SqlParameter("@fecha_ingreso_estadia", estadia.Fecha_Inicio),
                        new SqlParameter("@clientes", clientesDataTable)
                    ));
                estadia.Id = id;
                LogUtils.LogInfo("Se ingresó la estadía " + estadia.Id);
                MessageBox.Show("Se ingresó satisfactoriamente la estadía " + estadia.Id
                    + "\n\nGuárdese el código, pues será necesario para registrar consumibles y la facturación", "INFO");
                return true;
            }
            catch (Exception Ex)
            {
                LogUtils.LogError(Ex);
                MessageBox.Show("Hubo un error al intentar ingresar una estadía. Revise el log", "ERROR");
                return false;
            }
        }

        public Estadia ObtenerEstadiaDeReserva(Reserva reserva)
        {
            Estadia estadia = new Estadia(null, null, null);

            foreach (var row in DatabaseConnection.GetInstance().
                ExecuteProcedure("OBTENER_ESTADIA_DE_RESERVA", new SqlParameter("@id_reserva", reserva.Id)))
            {
                estadia.Id = Convert.ToInt32(row["id_estadia"]);
                estadia.Fecha_Inicio = Convert.ToDateTime(row["fecha_ingreso_estadia"]);
                if (row["fecha_egreso_estadia"] != DBNull.Value)
                    estadia.Fecha_Fin = Convert.ToDateTime(row["fecha_egreso_estadia"]);
            }

            return estadia;
        }
    }
}
