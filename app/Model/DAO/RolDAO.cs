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
        public List<Rol> ObtenerRolesFiltrado(string Namepart, Funcionalidad Funcionalidad, bool SoloActivos)
        {
            List<Rol> Roles = new List<Rol>();

            foreach (var row in DatabaseConnection.GetInstance().
                ExecuteProcedure("OBTENER_ROLES", GenerateParamsFilter(Namepart, Funcionalidad, SoloActivos)))
            {
                Rol r = new Rol(
                    Convert.ToInt32(row["id_rol"]),
                    Convert.ToString(row["nombre_rol"]),
                    Convert.ToBoolean(row["estado_rol"]),
                    null
                );
                Roles.Add(r);
            }

            return Roles;
        }

        public bool InsertarNuevoRol(Rol NuevoRol)
        {
            try
            {
                DatabaseConnection.GetInstance()
                    .ExecuteProcedureNonQuery("INSERTAR_NUEVO_ROL", GenerateParamsDML(NuevoRol));
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

        public bool ModificarRol(Rol RolModificado)
        {
            try
            {
                DatabaseConnection.GetInstance()
                    .ExecuteProcedureNonQuery("MODIFICAR_ROL", GenerateParamsDML(RolModificado));
                LogUtils.LogInfo("Se modificó el rol " + RolModificado.Nombre);
                MessageBox.Show("Se modificó satisfactoriamente el rol " + RolModificado.Nombre, "INFO");
                return true;
            }
            catch (Exception Ex)
            {
                LogUtils.LogError(Ex);
                MessageBox.Show("Hubo un error al intentar agregar un rol. Revise el log", "ERROR");
                return false;
            }
        }

        public bool DeshabilitarRol(Rol RolADeshabilitar)
        {
            try
            {
                SqlParameter param = new SqlParameter("@id_rol", RolADeshabilitar.Id);

                DatabaseConnection.GetInstance()
                    .ExecuteProcedureNonQuery("DESHABILITAR_ROL", param);
                LogUtils.LogInfo("Se deshabilitó el rol " + RolADeshabilitar.Nombre);
                MessageBox.Show("Se eliminó satisfactoriamente el rol " + RolADeshabilitar.Nombre, "INFO");
                return true;
            }
            catch (Exception Ex)
            {
                LogUtils.LogError(Ex);
                MessageBox.Show("Hubo un error al intentar agregar un rol. Revise el log", "ERROR");
                return false;
            }
        }

        private SqlParameter[] GenerateParamsFilter(string Namepart, Funcionalidad Funcionalidad, bool SoloActivos)
        {
            List<SqlParameter> Params = new List<SqlParameter>();

            Params.Add(new SqlParameter("@nombre", Namepart));
            Params.Add(new SqlParameter("@funcionalidad", Funcionalidad.Id));
            Params.Add(new SqlParameter("@soloActivos", SoloActivos));

            return Params.ToArray();
        }

        private SqlParameter[] GenerateParamsDML(Rol Rol)
        {
            List<SqlParameter> Params = new List<SqlParameter>();

            DataTable Funcionalidades = DatabaseUtils
                .ConvertToDataTable<Funcionalidad>(Rol.Funcionalidades, "Id");

            if (Rol.Id != null)
                Params.Add(new SqlParameter("@id_rol", Rol.Id));

            Params.Add(new SqlParameter("@nombre_rol", Rol.Nombre));
            Params.Add(new SqlParameter("@funcionalidades", Funcionalidades));

            if (Rol.Id != null)
                Params.Add(new SqlParameter("@estado", Rol.Estado));

            return Params.ToArray();
        }
    }
}
