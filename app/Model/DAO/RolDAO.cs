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
    class RolDAO
    {
        public bool InsertarNuevoRol(Rol NuevoRol)
        {
            try
            {
                DatabaseConnection.GetInstance()
                    .ExecuteProcedureNonQuery("INSERTAR_NUEVO_ROL", GenerateParams(NuevoRol));
                LogUtils.LogInfo("Se creó rol " + NuevoRol.Nombre);
                MessageBox.Show("Se agregó satisfactoriamente el rol " + NuevoRol.Nombre, "INFO");
                return true;
            }
            catch (Exception Ex)
            {
                LogUtils.LogError(Ex);
                MessageBox.Show("Hubo un error al intentar agregar un rol. Revise el log", "ERROR");
                return false;
            }
        }

        public SqlParameter[] GenerateParams(Rol Rol)
        {
            List<SqlParameter> Params = new List<SqlParameter>();

            DataTable Funcionalidades = DatabaseUtils
                .ConvertToDataTable<Funcionalidad>(Rol.Funcionalidades, "Id");

            Params.Add(new SqlParameter("@nombre_rol", Rol.Nombre));
            Params.Add(new SqlParameter("@funcionalidades", Funcionalidades));

            return Params.ToArray();
        }
    }
}
