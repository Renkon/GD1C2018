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
    public class ConsumoDAO
    {
        public List<Consumo> ObtenerConsumosDeEstadia(Estadia estadia)
        {
            List<Consumo> Consumos = new List<Consumo>();

            foreach (var row in DatabaseConnection.GetInstance().ExecuteProcedure("OBTENER_CONSUMOS_DE_ESTADIA",
                new SqlParameter("@id_estadia", estadia.Id)))
            {
                Consumo c = new Consumo(
                    Convert.ToInt32(row["id_consumo"]),
                    new Consumible(
                        Convert.ToInt32(row["id_consumible"]),
                        Convert.ToString(row["descripcion_consumible"]),
                        Convert.ToDouble(row["precio_consumible"])
                    ),
                    estadia,
                    new Habitacion(
                        Convert.ToInt32(row["id_habitacion"]),
                        Convert.ToInt32(row["numero_habitacion"])
                    ),
                    Convert.ToDateTime(row["fecha_consumo"]),
                    Convert.ToInt32(row["cantidad_consumo"])
                );

                Consumos.Add(c);
            }

            return Consumos;
        }

        public bool BorrarConsumo(Consumo consumo)
        {
            try
            {
                DatabaseConnection.GetInstance().ExecuteProcedureNonQuery("BORRAR_CONSUMO",
                    new SqlParameter("@id_consumo", consumo.Id),
                    new SqlParameter("@id_rol_user", Session.Rol.Id));
                LogUtils.LogInfo("Se borró el consumo " + consumo.Id);
                MessageBox.Show("Se eliminó satisfactoriamente el consumo", "INFO");
                return true;
            }
            catch (Exception ex)
            {
                LogUtils.LogError(ex);
                MessageBox.Show("Hubo un error al intentar eliminar un consumo. Revise el log", "ERROR");
                return false;
            }
        }

        public bool ModificarConsumo(Consumo consumo)
        {
            try
            {
                DatabaseConnection.GetInstance().ExecuteProcedureNonQuery("MODIFICAR_CONSUMO",
                    GenerateDMLParams(consumo, consumo.Consumible, null, consumo.Habitacion,
                    consumo.Fecha, consumo.Cantidad));
                LogUtils.LogInfo("Se modificó el consumo " + consumo.Id);
                MessageBox.Show("Se modificó satisfactoriamente el consumo", "INFO");
                return true;
            }
            catch (Exception ex)
            {
                LogUtils.LogError(ex);
                MessageBox.Show("Hubo un error al intentar modificar un consumo. Revise el log", "ERROR");
                return false;
            }
        }

        public Consumo InsertarConsumo(Consumible consumible, Estadia estadia, Habitacion habitación, 
            DateTime fecha, int cantidad)
        {
            try
            {
                int id = Convert.ToInt32(DatabaseConnection.GetInstance().ExecuteProcedureScalar("AGREGAR_CONSUMO",
                    GenerateDMLParams(null, consumible, estadia, habitación, fecha, cantidad)));
                Consumo c = new Consumo(id, consumible, estadia, habitación, fecha, cantidad);
                LogUtils.LogInfo("Se agregó el consumo " + c.Id);
                MessageBox.Show("Se agregó satisfactoriamente el consumo", "INFO");
                
                return c;
            }
            catch (Exception ex)
            {
                LogUtils.LogError(ex);
                MessageBox.Show("Hubo un error al intentar insertar un consumo. Revise el log", "ERROR");
                return null;
            }
        }

        private SqlParameter[] GenerateDMLParams(Consumo consumo, Consumible consumible, Estadia estadia, Habitacion habitacion,
            DateTime fecha, int cantidad)
        {
            List<SqlParameter> Params = new List<SqlParameter>();

            Params.Add(new SqlParameter("@id_rol_user", Session.Rol.Id));
            Params.Add(new SqlParameter("@id_consumible", consumible.Id));
            if (estadia != null)
                Params.Add(new SqlParameter("@id_estadia", estadia.Id));
            if (consumo != null)
                Params.Add(new SqlParameter("@id_consumo", consumo.Id));
            Params.Add(new SqlParameter("@id_habitacion", habitacion.Id));
            Params.Add(new SqlParameter("@fecha_consumo", fecha));
            Params.Add(new SqlParameter("@cantidad_consumo", cantidad));

            return Params.ToArray();
        }
    }
}
