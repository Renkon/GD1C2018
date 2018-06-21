using FrbaHotel.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrbaHotel
{
    public class Config
    {
        private static Config Instance = new Config();

        private readonly string PropertyFilename = "application.properties";
        private readonly string ConnectionString;
        private readonly DateTime CurrentDate;

        private Config()
        {
            // Directorio donde se ejecuta la app (src si es el VS)
            var DirectoryName = System.AppDomain.CurrentDomain.BaseDirectory;

            // Levanto los properties
            var Properties = PropertyUtils.GetPropertiesFromFile(Path.Combine(DirectoryName, "..", "..", PropertyFilename));

            if (Properties.Count == 0)
            {
                ConnectionString = "";
                CurrentDate = DateTime.Now;
            }
            else
            {
                // Armo el string de conexión a la BBDD
                ConnectionString =
                    new StringBuilder("Server=").Append(Properties["Host"]).Append("\\").Append(Properties["DbInstance"]).Append(";")
                              .Append("Database=").Append(Properties["Database"]).Append(";")
                              .Append("User Id=").Append(Properties["Username"]).Append(";")
                              .Append("Password=").Append(Properties["Password"]).Append(";")
                              .ToString();
                // Seteo el día de hoy
                CurrentDate = DateTime.ParseExact(Properties["Date"], "dd-MM-yyyy", CultureInfo.InvariantCulture);
            }
        }

        public string GetConnectionString()
        {
            if (ConnectionString.Equals(""))
                throw new Exception("String de conexión inválido. Es posible que no se haya leído correctamente el PROPERTIES");
            return ConnectionString;
        }

        public DateTime GetCurrentDate()
        {
            return CurrentDate;
        }

        public static Config GetInstance()
        {
            return Instance; 
        }
    }
}
