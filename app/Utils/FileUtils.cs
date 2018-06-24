using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrbaHotel.Utils
{
    public class FileUtils
    {
        private static string DirectoryName = System.AppDomain.CurrentDomain.BaseDirectory;
        public static void CreateDirectory(string fileName)
        {
            Directory.CreateDirectory(Path.Combine(DirectoryName, "..", "..", fileName));
        }

        public static void EscribirFile(string where, string what, string fileName)
        {
            File.WriteAllText(Path.Combine(DirectoryName, "..", "..", where, fileName), what, Encoding.UTF8);
        }
    }
}
