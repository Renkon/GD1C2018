using FrbaHotel.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrbaHotel.Database
{
    public class DatabaseConfig
    {
        private static DatabaseConfig Instance = new DatabaseConfig();

        private readonly string PropertyFilename = "application.properties";
        private readonly string ConnectionString;

        private DatabaseConfig()
        {
            // Directorio donde se ejecuta la app (src si es el VS)
            var DirectoryName = System.AppDomain.CurrentDomain.BaseDirectory;

            // Levanto los properties
            var Properties = PropertyUtils.GetPropertiesFromFile(Path.Combine(DirectoryName, "..", "..", PropertyFilename));

            if (Properties.Count == 0)
                ConnectionString = "";
            else
                ConnectionString = 
                    new StringBuilder("Server=").Append(Properties["Host"]).Append("\\").Append(Properties["DbInstance"]).Append(";")
                              .Append("Database=").Append(Properties["Database"]).Append(";")
                              .Append("User Id=").Append(Properties["Username"]).Append(";")
                              .Append("Password=").Append(Properties["Password"]).Append(";")            
                              .ToString();
        }

        public string GetConnectionString()
        {
            if (ConnectionString.Equals(""))
                throw new Exception("String de conexión inválido. Es posible que no se haya leído correctamente el PROPERTIES");
            return ConnectionString;
        }

        public static DatabaseConfig GetInstance()
        {
            return Instance; 
        }
    }
}
