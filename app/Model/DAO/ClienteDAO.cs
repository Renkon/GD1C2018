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
    class ClienteDAO
    {
        internal List<Cliente> ObtenerClientesFiltrado(string Nombre, string Apellido, TipoDocumento TipoDocumento, long Documento, string Correo, bool SoloActivos)
        {
            List<Cliente> clientes = new List<Cliente>();
            Dictionary<int, TipoDocumento> TiposDoc = new Dictionary<int, TipoDocumento>();

            List<TipoDocumento> tempDocs = new TipoDocumentoDAO().ObtenerTiposDocumento();
            foreach (var TipoDoc in tempDocs)
                TiposDoc.Add(TipoDoc.Id.Value, TipoDoc);

            long nro = -1;
            long piso = -1;
            int pais = -1;
            bool estado = true;

            foreach (var row in DatabaseConnection.GetInstance().
                ExecuteProcedure("OBTENER_CLIENTES_FILTRADOS", GenerateParamsFilter(Nombre,
                    Apellido, TipoDocumento, Documento, Correo, SoloActivos)))
            {
                if (!(row["domicilio_numero_cliente"].Equals("")))
                    nro = Convert.ToInt64(row["domicilio_numero_cliente"]);
                if (!(row["domicilio_piso_cliente"].Equals("")))
                    piso = Convert.ToInt64(row["domicilio_piso_cliente"]);
                if (!(row["id_pais"].Equals(DBNull.Value)))
                    pais = Convert.ToInt32(row["id_pais"]);
                if (!(row["estado_cliente"].Equals(DBNull.Value)))
                    estado = Convert.ToBoolean(row["estado_cliente"]);
                    Cliente cliente = new Cliente(
                    Convert.ToInt32(row["id_cliente"]),
                    Convert.ToString(row["nombre_cliente"]),
                    Convert.ToString(row["apellido_cliente"]),
                    TiposDoc[Convert.ToInt32(row["id_tipo_documento"])],
                    Convert.ToInt64(row["numero_documento_cliente"]),
                    Convert.ToString(row["correo_cliente"]),
                    Convert.ToString(row["telefono_cliente"]),
                    Convert.ToString(row["domicilio_calle_cliente"]),
                    nro,
                    piso,
                    Convert.ToString(row["domicilio_departamento_cliente"]),
                    Convert.ToString(row["ciudad_cliente"]),
                    pais,
                    Convert.ToString(row["nacionalidad_cliente"]),
                    Convert.ToDateTime(row["fecha_nacimiento_cliente"]),
                    estado
                );

                clientes.Add(cliente);
            }

            return clientes;
        }

        private SqlParameter[] GenerateParamsFilter(string Nombre, string Apellido,
                TipoDocumento TipoDocumento, long Documento, string Correo, bool SoloActivos)
        {
            List<SqlParameter> Params = new List<SqlParameter>();

            Params.Add(new SqlParameter("@nombre", Nombre));
            Params.Add(new SqlParameter("@apellido", Apellido));
            Params.Add(new SqlParameter("@id_documento", TipoDocumento.Id));
            Params.Add(new SqlParameter("@numero_documento", Documento));
            Params.Add(new SqlParameter("@correo", Correo));
            Params.Add(new SqlParameter("@estado_cliente", SoloActivos));

            return Params.ToArray();
        }


        internal bool InsertarNuevoUsuario(Cliente cliente)
        {
            try
            {
                DatabaseConnection.GetInstance()
                    .ExecuteProcedureNonQuery("INSERTAR_NUEVO_CLIENTE", GenerateParamsDML(cliente));
                LogUtils.LogInfo("Se creó cliente " + cliente.Nombre + " " + cliente.Apellido);
                MessageBox.Show("Se agregó satisfactoriamente el cliente " + cliente.Nombre + " " + cliente.Apellido, "INFO");
                return true;
            }
            catch (SqlException Sex)
            {
                LogUtils.LogError(Sex);
                if (Sex.Number == 2627)
                {
                    // Solo hay dos posibilidades de fallo por unique key, por tabla usuario o cuenta
                    if (Sex.Message.Contains("UNIQUE") && Sex.Message.Contains("EL_MONSTRUO_DEL_LAGO_MASER.cliente"))
                        MessageBox.Show("No se pudo agregar el cliente. Ese correo ya está en uso", "ERROR");
                }
                else
                    MessageBox.Show("Ha ocurrido un error al intentar insertar: " + Sex.Message);
                return false;
            }
            catch (Exception Ex)
            {
                LogUtils.LogError(Ex);
                MessageBox.Show("Hubo un error al intentar agregar un cliente. Revise el log", "ERROR");
                return false;
            }
        }




        internal bool ModificarUsuario(Cliente Cliente)
        {
            try
            {
                DatabaseConnection.GetInstance()
                    .ExecuteProcedureNonQuery("MODIFICAR_CLIENTE", GenerateParamsDML(Cliente));
                LogUtils.LogInfo("Se modificó cliente " + Cliente.Id);
                MessageBox.Show("Se modificó satisfactoriamente el cliente " + Cliente.Id, "INFO");
                return true;
            }
            catch (SqlException Sex)
            {
                LogUtils.LogError(Sex);
                if (Sex.Number == 2627)
                {
                    // Solo hay dos posibilidades de fallo por unique key, por tabla usuario o cuenta
                    if (Sex.Message.Contains("UNIQUE") && Sex.Message.Contains("EL_MONSTRUO_DEL_LAGO_MASER.usuario"))
                        MessageBox.Show("No se pudo agregar el cliente. Ese correo ya está en uso", "ERROR");
                    else throw;
                }
                else throw;
                return false;
            }
            catch (Exception Ex)
            {
                LogUtils.LogError(Ex);
                MessageBox.Show("Hubo un error al intentar modificar un cliente. Revise el log", "ERROR");
                return false;
            }
        }

        private SqlParameter[] GenerateParamsDML(Cliente Cliente)
        {
            List<SqlParameter> Params = new List<SqlParameter>();
            
            Params.Add(new SqlParameter("@id_rol_user", Session.Rol.Id));

            if (Cliente.Id != null)
                Params.Add(new SqlParameter("@id_cliente", Cliente.Id));

            Params.Add(new SqlParameter("@nombre", Cliente.Nombre));
            Params.Add(new SqlParameter("@apellido", Cliente.Apellido));
            Params.Add(new SqlParameter("@id_documento", Cliente.TipoDocumento.Id));
            Params.Add(new SqlParameter("@numero_documento", Cliente.Documento));
            Params.Add(new SqlParameter("@correo", Cliente.Correo));
            Params.Add(new SqlParameter("@telefono", Cliente.Telefono));
            Params.Add(new SqlParameter("@domicilio_calle", Cliente.Calle));
            Params.Add(new SqlParameter("@domicilio_numero", Cliente.Nro));
            Params.Add(new SqlParameter("@domicilio_piso", Cliente.Piso));
            Params.Add(new SqlParameter("@domicilio_departamento", Cliente.Departamento));
            Params.Add(new SqlParameter("@ciudad", Cliente.Ciudad));
            Params.Add(new SqlParameter("@pais", Cliente.Pais));
            Params.Add(new SqlParameter("@nacionalidad", Cliente.Nacionalidad));
            Params.Add(new SqlParameter("@fecha_nacimiento", Cliente.FechaNacimiento));

            if (Cliente.Id != null)
                Params.Add(new SqlParameter("@estado_cliente", Cliente.Estado));

            return Params.ToArray();
        }


        internal bool DeshabilitarUsuario(Cliente Cliente)
        {
            try
            {
                SqlParameter paramUsrRol = new SqlParameter("@id_rol_user", Session.Rol.Id);
                SqlParameter param = new SqlParameter("@id_cliente", Cliente.Id);

                DatabaseConnection.GetInstance()
                    .ExecuteProcedureNonQuery("DESHABILITAR_CLIENTE", paramUsrRol, param);
                LogUtils.LogInfo("Se deshabilitó el cliente " + Cliente.Nombre + " " + Cliente.Apellido);
                MessageBox.Show("Se eliminó satisfactoriamente el cliente " + Cliente.Nombre + " " + Cliente.Apellido, "INFO");
                return true;
            }
            catch (Exception Ex)
            {
                LogUtils.LogError(Ex);
                MessageBox.Show("Hubo un error al intentar deshabilitar un cliente. Revise el log", "ERROR");
                return false;
            }
        }
    }
}
