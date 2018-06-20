using FrbaHotel.Database;
using FrbaHotel.Forms;
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
        public bool isCorreoUnico(string Correo)
        {
            return Convert.ToBoolean(DatabaseConnection.GetInstance().ExecuteProcedureScalar("VALIDAR_CORREO_UNICO_CLIENTE",
                new SqlParameter("@correo", Correo)));
        }

        public bool isDocumentoUnico(TipoDocumento Tipo, long Documento)
        {
            return Convert.ToBoolean(
                DatabaseConnection.GetInstance().ExecuteProcedureScalar("VALIDAR_DOCUMENTO_UNICO_CLIENTE",
                new SqlParameter("@id_tipo_documento", Tipo.Id),
                new SqlParameter("@numero_documento_cliente", Documento)
                ));
        }

        public List<Cliente> ObtenerClientesFiltrado(string Nombre, string Apellido, TipoDocumento TipoDocumento, long Documento, string Correo, bool SoloActivos)
        {
            List<Cliente> clientes = new List<Cliente>();
            Dictionary<int, TipoDocumento> TiposDoc = new Dictionary<int, TipoDocumento>();

            List<TipoDocumento> tempDocs = new TipoDocumentoDAO().ObtenerTiposDocumento();
            foreach (var TipoDoc in tempDocs)
                TiposDoc.Add(TipoDoc.Id.Value, TipoDoc);

            foreach (var row in DatabaseConnection.GetInstance().
                ExecuteProcedure("OBTENER_CLIENTES_FILTRADOS", GenerateParamsFilter(Nombre,
                    Apellido, TipoDocumento, Documento, Correo, SoloActivos)))
            {
                int nro = 0;
                int piso = 0;
                Pais pais;

                if (!(Convert.ToString(row["domicilio_numero_cliente"]).Equals("")))
                    nro = Convert.ToInt32(row["domicilio_numero_cliente"]);

                if (!(Convert.ToString(row["domicilio_piso_cliente"]).Equals("")))
                    piso = Convert.ToInt32(row["domicilio_piso_cliente"]);

                if (!(row["id_pais"].Equals(DBNull.Value)))
                    pais = new Pais(Convert.ToInt32(row["id_pais"]), new PaisDAO().ObtenerNombrePais(Convert.ToInt32(row["id_pais"])));
                else
                    pais = new Pais(-1, "");

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
                    Convert.ToBoolean(row["estado_cliente"])
                );

                clientes.Add(cliente);
            }

            return clientes;
        }

        public bool InsertarClientePreexistente(Cliente Cliente)
        {
            try
            {
                int idCliente = Convert.ToInt32(DatabaseConnection.GetInstance().ExecuteProcedureScalar("INSERTAR_CLIENTE_PREEXISTENTE",
                    new SqlParameter("@nombre_cliente", Cliente.Nombre),
                    new SqlParameter("@apellido_cliente", Cliente.Apellido),
                    new SqlParameter("@id_tipo_documento", Cliente.TipoDocumento.Id),
                    new SqlParameter("@numero_documento_cliente", Cliente.Documento),
                    new SqlParameter("@correo_cliente", Cliente.Correo),
                    new SqlParameter("@telefono_cliente", Cliente.Telefono),
                    new SqlParameter("@domicilio_calle_cliente", Cliente.Calle),
                    new SqlParameter("@domicilio_numero_cliente", Cliente.Nro),
                    new SqlParameter("@domicilio_piso_cliente", Cliente.Piso),
                    new SqlParameter("@domicilio_departamento_cliente", Cliente.Departamento),
                    new SqlParameter("@ciudad_cliente", Cliente.Ciudad),
                    new SqlParameter("@id_pais", Cliente.Pais.Id),
                    new SqlParameter("@nacionalidad_cliente", Cliente.Nacionalidad),
                    new SqlParameter("@fecha_nacimiento_cliente", Cliente.FechaNacimiento)
                ));
                Cliente.Id = idCliente;

                LogUtils.LogInfo("Se creó cliente preexistente " + Cliente.Nombre + " " + Cliente.Apellido);
                MessageBox.Show("Se agregó satisfactoriamente el cliente preexistente " + Cliente.Nombre + " " + Cliente.Apellido, "INFO");
                return true;
            }
            catch (Exception Ex)
            {
                LogUtils.LogError(Ex);
                MessageBox.Show("Hubo un error al intentar agregar un cliente preexistente. Revise el log", "ERROR");
                return false;
            }
        }

        private SqlParameter[] GenerateParamsFilter(string Nombre, string Apellido,
                TipoDocumento TipoDocumento, long Documento, string Correo, bool SoloActivos)
        {
            List<SqlParameter> Params = new List<SqlParameter>();

            Params.Add(new SqlParameter("@nombre", Nombre));
            Params.Add(new SqlParameter("@apellido", Apellido));
            Params.Add(new SqlParameter("@id_documento", TipoDocumento != null ? TipoDocumento.Id : -1));
            Params.Add(new SqlParameter("@numero_documento", Documento));
            Params.Add(new SqlParameter("@correo", Correo));
            Params.Add(new SqlParameter("@soloActivos", SoloActivos));

            return Params.ToArray();
        }

        public bool InsertarNuevoUsuario(Cliente cliente, FormType type)
        {
            try
            {
                DatabaseConnection.GetInstance()
                    .ExecuteProcedureNonQuery(type == FormType.Add ? 
                    "INSERTAR_NUEVO_CLIENTE" : "INSERTAR_NUEVO_CLIENTE_SIN_VALIDACION", GenerateParamsDML(cliente, type));
                LogUtils.LogInfo("Se creó cliente " + cliente.Nombre + " " + cliente.Apellido);
                MessageBox.Show("Se agregó satisfactoriamente el cliente " + cliente.Nombre + " " + cliente.Apellido, "INFO");
                return true;
            }
            catch (SqlException Sex)
            {
                LogUtils.LogError(Sex);
                if (Sex.Number == 2627)
                {
                    // Solo hay dos posibilidades de fallo por unique key
                    if (Sex.Message.Contains("UNIQUE"))
                        MessageBox.Show("No se pudo agregar el cliente.\n El documento o el correo ya están en uso", "ERROR");
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

        public bool ModificarUsuario(Cliente Cliente)
        {
            try
            {
                DatabaseConnection.GetInstance()
                    .ExecuteProcedureNonQuery("MODIFICAR_CLIENTE", GenerateParamsDML(Cliente, FormType.Modify));
                LogUtils.LogInfo("Se modificó cliente " + Cliente.Nombre + " " + Cliente.Apellido);
                MessageBox.Show("Se modificó satisfactoriamente el cliente " + Cliente.Nombre + " " + Cliente.Apellido, "INFO");
                return true;
            }
            catch (SqlException Sex)
            {
                LogUtils.LogError(Sex);
                if (Sex.Number == 2627)
                {
                    // Solo hay dos posibilidades de fallo por unique key
                    if (Sex.Message.Contains("UNIQUE"))
                        MessageBox.Show("No se pudo modificar el cliente. \nHay un usuario con ese documento y/o con ese correo.", "ERROR");
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

        private SqlParameter[] GenerateParamsDML(Cliente Cliente, FormType type)
        {
            List<SqlParameter> Params = new List<SqlParameter>();
            
            if (type == FormType.Add)
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
            Params.Add(new SqlParameter("@pais", Cliente.Pais.Id));
            Params.Add(new SqlParameter("@nacionalidad", Cliente.Nacionalidad));
            Params.Add(new SqlParameter("@fecha_nacimiento", Cliente.FechaNacimiento));

            if (Cliente.Id != null)
                Params.Add(new SqlParameter("@estado_cliente", Cliente.Estado));

            return Params.ToArray();
        }

        public bool DeshabilitarUsuario(Cliente Cliente)
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

        public List<Cliente> ObtenerClientesCompletosFiltrado(TipoDocumento TipoDocumento, long Documento, string Correo)
        {
            List<Cliente> clientes = new List<Cliente>();
            Dictionary<int, TipoDocumento> TiposDoc = new Dictionary<int, TipoDocumento>();

            List<TipoDocumento> tempDocs = new TipoDocumentoDAO().ObtenerTiposDocumento();
            foreach (var TipoDoc in tempDocs)
                TiposDoc.Add(TipoDoc.Id.Value, TipoDoc);

            foreach (var row in DatabaseConnection.GetInstance().
                ExecuteProcedure("OBTENER_CLIENTES_COMPLETOS_FILTRADOS",
                    new SqlParameter("@id_documento", TipoDocumento.Id),
                    new SqlParameter("@numero_documento", Documento),
                    new SqlParameter("@correo", Correo)
                ))
            {
                int nro = 0;
                int piso = 0;
                Pais pais;

                if (!(Convert.ToString(row["domicilio_numero_cliente"]).Equals("")))
                    nro = Convert.ToInt32(row["domicilio_numero_cliente"]);

                if (!(Convert.ToString(row["domicilio_piso_cliente"]).Equals("")))
                    piso = Convert.ToInt32(row["domicilio_piso_cliente"]);

                if (!(row["id_pais"].Equals(DBNull.Value)))
                    pais = new Pais(Convert.ToInt32(row["id_pais"]), new PaisDAO().ObtenerNombrePais(Convert.ToInt32(row["id_pais"])));
                else
                    pais = new Pais(-1, "");

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
                    Convert.ToBoolean(row["estado_cliente"])
                );

                clientes.Add(cliente);
            }

            return clientes;
        }
    }
}
