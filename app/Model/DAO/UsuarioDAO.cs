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
    public class UsuarioDAO
    {
        public Usuario ObtenerUsuarioDummy()
        {
            var DummyId = Convert.ToInt32(DatabaseConnection.GetInstance()
                .ExecuteProcedureScalar("OBTENER_USUARIO_DUMMY"));
          
            return new Usuario(DummyId);
        }

        public List<Tuple<Usuario, Cuenta>> ObtenerUsuariosFiltrado(string Usuario, Rol Rol, string Nombre,
            string Apellido, TipoDocumento TipoDocumento, long Documento, string Correo, Hotel Hotel, bool SoloActivos)
        {
            List<Tuple<Usuario, Cuenta>> tuples = new List<Tuple<Usuario, Cuenta>>();
            Dictionary<int, TipoDocumento> TiposDoc = new Dictionary<int, TipoDocumento>();
            
            List<TipoDocumento> tempDocs = new TipoDocumentoDAO().ObtenerTiposDocumento();
            foreach (var TipoDoc in tempDocs)
                TiposDoc.Add(TipoDoc.Id.Value, TipoDoc);

            foreach (var row in DatabaseConnection.GetInstance().
                ExecuteProcedure("OBTENER_USUARIOS_FILTRADOS", GenerateParamsFilter(Usuario, Rol, Nombre, 
                    Apellido, TipoDocumento, Documento, Correo, Hotel, SoloActivos)))
            {
                Cuenta c = new Cuenta(
                    Convert.ToString(row["usuario_cuenta"]),
                    null,
                    0
                );
                Usuario u = new Usuario(
                    Convert.ToInt32(row["id_usuario"]),
                    Convert.ToString(row["nombre_usuario"]),
                    Convert.ToString(row["apellido_usuario"]),
                    null,
                    null,
                    TiposDoc[Convert.ToInt32(row["id_tipo_documento"])],
                    Convert.ToInt64(row["numero_documento_usuario"]),
                    Convert.ToString(row["correo_usuario"]),
                    Convert.ToString(row["telefono_usuario"]),
                    Convert.ToString(row["direccion_usuario"]),
                    Convert.ToDateTime(row["fecha_nacimiento_usuario"]),
                    Convert.ToBoolean(row["estado_usuario"])
                );

                tuples.Add(Tuple.Create(u, c));
            }

            return tuples;
        }

        public bool InsertarNuevoUsuario(Usuario Usuario, Cuenta Cuenta)
        {
            try
            {
                DatabaseConnection.GetInstance()
                    .ExecuteProcedureNonQuery("INSERTAR_USUARIO", GenerateParamsDML(Usuario, Cuenta));
                LogUtils.LogInfo("Se creó usuario " + Cuenta.Usuario);
                MessageBox.Show("Se agregó satisfactoriamente el usuario " + Cuenta.Usuario, "INFO");
                return true;
            }
            catch (SqlException Sex)
            {
                LogUtils.LogError(Sex);
                if (Sex.Number == 2627)
                {
                    // Solo hay dos posibilidades de fallo por unique key, por tabla usuario o cuenta
                    if (Sex.Message.Contains("UNIQUE") && Sex.Message.Contains("EL_MONSTRUO_DEL_LAGO_MASER.usuario"))
                        MessageBox.Show("No se pudo agregar el usuario. Ese documento ya está en uso", "ERROR");
                    else if (Sex.Message.Contains("UNIQUE") && Sex.Message.Contains("EL_MONSTRUO_DEL_LAGO_MASER.cuenta"))
                        MessageBox.Show("No se pudo agregar el usuario. Ese nombre de usuario ya está en uso", "ERROR");
                    else throw;
                }
                else throw;
                return false;
            }
            catch (Exception Ex)
            {
                LogUtils.LogError(Ex);
                MessageBox.Show("Hubo un error al intentar agregar un usuario. Revise el log", "ERROR");
                return false;
            }
        }

        private SqlParameter[] GenerateParamsFilter(string Usuario, Rol Rol, string Nombre, 
            string Apellido, TipoDocumento TipoDocumento, long Documento, string Correo, Hotel Hotel, bool SoloActivos)
        {
            List<SqlParameter> Params = new List<SqlParameter>();

            Params.Add(new SqlParameter("@usuario_cuenta", Usuario));
            Params.Add(new SqlParameter("@id_rol", Rol.Id));
            Params.Add(new SqlParameter("@nombre_usuario", Nombre));
            Params.Add(new SqlParameter("@apellido_usuario", Apellido));
            Params.Add(new SqlParameter("@id_tipo_documento", TipoDocumento.Id));
            Params.Add(new SqlParameter("@numero_documento_usuario", Documento));
            Params.Add(new SqlParameter("@correo_usuario", Correo));
            Params.Add(new SqlParameter("@id_hotel", Hotel.Id));
            Params.Add(new SqlParameter("@soloActivos", SoloActivos));

            return Params.ToArray();
        }

        private SqlParameter[] GenerateParamsDML(Usuario Usuario, Cuenta Cuenta)
        {
            List<SqlParameter> Params = new List<SqlParameter>();

            DataTable Roles = DatabaseUtils
                .ConvertToDataTable<Rol>(Usuario.Roles, "Id");
            DataTable Hoteles = DatabaseUtils
                .ConvertToDataTable<Hotel>(Usuario.Hoteles, "Id");

            Params.Add(new SqlParameter("@id_rol_user", Session.Rol.Id));

            if (Usuario.Id != null)
                Params.Add(new SqlParameter("@id_usuario", Usuario.Id));

            Params.Add(new SqlParameter("@usuario_cuenta", Cuenta.Usuario));
            Params.Add(new SqlParameter("@contraseña_cuenta", DatabaseUtils.SHA256of(Cuenta.Contraseña)));
            Params.Add(new SqlParameter("@roles", Roles));
            Params.Add(new SqlParameter("@hoteles", Hoteles));
            Params.Add(new SqlParameter("@nombre_usuario", Usuario.Nombre));
            Params.Add(new SqlParameter("@apellido_usuario", Usuario.Apellido));
            Params.Add(new SqlParameter("@id_tipo_documento", Usuario.TipoDocumento.Id));
            Params.Add(new SqlParameter("@numero_documento_usuario", Usuario.Documento));
            Params.Add(new SqlParameter("@correo_usuario", Usuario.Correo));
            Params.Add(new SqlParameter("@telefono_usuario", Usuario.Teléfono));
            Params.Add(new SqlParameter("@direccion_usuario", Usuario.Dirección));
            Params.Add(new SqlParameter("@fecha_nacimiento_usuario", Usuario.FechaNacimiento));

            if (Usuario.Id != null)
                Params.Add(new SqlParameter("@estado", Usuario.Estado));

            return Params.ToArray();
        }
    }
}
