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
    public class FacturaDAO
    {
        public bool CrearFactura(Factura factura)
        {
            try
            {
                int Id = Convert.ToInt32(DatabaseConnection.GetInstance().ExecuteProcedureScalar("CREAR_FACTURA",
                    GenerateParams(factura)));
                factura.Id = Id;
                LogUtils.LogInfo("Se creó la factura " + factura.Id);
                MessageBox.Show("Se creó satisfactoriamente la factura número " + factura.Id
                    + ". A su vez se creó en la carpeta 'facturas' el archivo "
                    + factura.Id.Value.ToString("000000000000") + ".txt", "INFO");
                return true;
            }
            catch (Exception ex)
            {
                LogUtils.LogError(ex);
                MessageBox.Show("Hubo un error al intentar crear una factura. Revise el log", "ERROR");
                return false;
            }
        }

        public SqlParameter[] GenerateParams(Factura factura)
        {
            List<SqlParameter> Params = new List<SqlParameter>();

            DataTable ItemsFactura = new DataTable();
            ItemsFactura.Columns.Add("id_consumo", typeof(int));
            ItemsFactura.Columns.Add("precio_unitario_item_factura", typeof(double));
            ItemsFactura.Columns.Add("descripcion_item_factura", typeof(string));
            ItemsFactura.Columns.Add("cantidad_item_factura", typeof(int));

            foreach (ItemFactura item in factura.Items_Factura)
            {
                if (item.Consumo == null)
                    ItemsFactura.Rows.Add(DBNull.Value, item.Precio, item.Descripción, item.Cantidad);
                else
                    ItemsFactura.Rows.Add(item.Consumo.Id.Value, item.Precio, item.Descripción, item.Cantidad);
            }

            Params.Add(new SqlParameter("@id_rol_user", Session.Rol.Id));

            Params.Add(new SqlParameter("@fecha_factura", Config.GetInstance().GetCurrentDate()));
            Params.Add(new SqlParameter("@total_factura", factura.Monto_Total));
            Params.Add(new SqlParameter("@id_estadia", factura.Estadia.Id));
            Params.Add(new SqlParameter("@id_forma_de_pago",factura.Forma_Pago.Id));
            Params.Add(new SqlParameter("@detalle_pago", factura.Detalle_Forma_Pago));

            Params.Add(new SqlParameter("@listaDeItems", ItemsFactura));

            return Params.ToArray();
        }
    }
}
