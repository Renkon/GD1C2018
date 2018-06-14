using FrbaHotel.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FrbaHotel.Database
{
    class DatabaseConnection
    {
        private static DatabaseConnection Instance = new DatabaseConnection();

        protected SqlConnection Connection { get; private set; }

        private DatabaseConnection()
        {
            try
            {
                Connection = new SqlConnection(Config.GetInstance().GetConnectionString());
            }
            catch (Exception Ex)
            {
                LogUtils.LogError(Ex);
                MessageBox.Show("No se realizó la conexión con la BBDD. Revise el log para más información.", "ERROR GRAVE");
                Environment.Exit(1);
            }
        }

        // Ejecuta un procedure y devuelve el resultado
        // Depende del DAO castearlo como corresponda.
        // Devuelve una matriz con los resultados.
        public List<Dictionary<string, object>> ExecuteProcedure(string ProcedureName, params SqlParameter[] args)
        {
            try
            {
                List<Dictionary<string, object>> Rows = new List<Dictionary<string, object>>();
                Connection.Open();
                SqlCommand Command = new SqlCommand("EL_MONSTRUO_DEL_LAGO_MASER." + ProcedureName, Connection);
                Command.CommandType = CommandType.StoredProcedure;
                Command.Parameters.AddRange(args);
                using (SqlDataReader DataReader = Command.ExecuteReader())
                {
                    while (DataReader.Read())
                    {
                        
                        Rows.Add(Enumerable.Range(0, DataReader.FieldCount)
                            .ToDictionary(DataReader.GetName, DataReader.GetValue));
                    }
                }
                
                return Rows;
            }
            finally
            {
                Connection.Close();
            }
        }

        // Igual que el execute procedure, pero devuelve un valor solo.
        // Útil para los procedures que devuelven UN SOLO ROW/COLUMNA.
        public object ExecuteProcedureScalar(string ProcedureName, params SqlParameter[] args)
        {
            try
            {
                Connection.Open();
                SqlCommand Command = new SqlCommand("EL_MONSTRUO_DEL_LAGO_MASER." + ProcedureName, Connection);
                Command.CommandType = CommandType.StoredProcedure;
                Command.Parameters.AddRange(args);
                var Value = Command.ExecuteScalar();
                return Value;
            }
            finally
            {
                Connection.Close();
            }
        }

        // Igual que los anteriores, pero no espera obtener información
        public void ExecuteProcedureNonQuery(string ProcedureName, params SqlParameter[] args)
        {
            try
            {
                Connection.Open();
                SqlCommand Command = new SqlCommand("EL_MONSTRUO_DEL_LAGO_MASER." + ProcedureName, Connection);
                Command.CommandType = CommandType.StoredProcedure;
                Command.Parameters.AddRange(args);
                Command.ExecuteNonQuery();
            }
            finally
            {
                Connection.Close();
            }
        }


        public void TestConnectionQuery()
        {
            const int Number = 1337;

            SqlCommand Command = new SqlCommand("SELECT " + Number, Connection);
            int QueryResult = Convert.ToInt32(Command.ExecuteScalar());
            if (Number != QueryResult)
                throw new Exception("Error al ejecutar query de prueba. Se esperaba " 
                    + Number + " - se recibió " + QueryResult);
        }

        public void TestConnection()
        {
            try
            {
                Connection.Open();
                TestConnectionQuery();
                Connection.Close();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString(), "ERROR GRAVE");
            }
        }

        public static DatabaseConnection GetInstance()
        {
            return Instance;
        }
            
    }
}
