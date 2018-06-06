using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrbaHotel.Utils
{
    class PropertyUtils
    {
        public static Dictionary<string, string> GetPropertiesFromFile(string FilePath)
        {
            var Properties = new Dictionary<string, string>();

            try
            {
                // Leo el archivo y voy analizando línea por línea
                // Source: https://stackoverflow.com/questions/485659/can-net-load-and-parse-a-properties-file-equivalent-to-java-properties-class
                foreach (var Row in File.ReadAllLines(FilePath))
                    Properties.Add(Row.Split('=')[0], string.Join("=", Row.Split('=').Skip(1).ToArray()));

            }
            catch (Exception Ex)
            {
                LogUtils.LogError(Ex);
            }
            return Properties;
        }
    }
}
