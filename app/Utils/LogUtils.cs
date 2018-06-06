using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrbaHotel.Utils
{
    class LogUtils
    {
        private static readonly string FileName = "frbaHotel.log";

        private static void Log(string Type, string Content)
        {
            var FileDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
            File.AppendAllText(Path.Combine(FileDirectory, "..", "..", FileName),
                GetString(Type, Content));
        }

        public static void LogError(string Content)
        {
            Log("ERROR", Content);
        }

        public static void LogError(Exception Ex)
        {
            Log("ERROR", Ex.ToString());
        }

        public static void LogWarning(string Content)
        {
            Log("WARN", Content);
        }

        public static void LogInfo(string Content)
        {
            Log("INFO", Content);
        }

        private static string GetString(string Type, string Content)
        {
            var TimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            return new StringBuilder(TimeStamp).Append(" [").Append(Type).Append("]: ")
                .Append(Content).Append(Environment.NewLine).ToString();
        }
    }
}
