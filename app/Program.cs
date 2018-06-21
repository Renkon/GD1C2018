using FrbaHotel.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FrbaHotel
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            LogUtils.LogInfo("--------------------------------------------------");
            LogUtils.LogInfo("Iniciando aplicación FRBA Hotel - EMDLM");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Lo que está a continuación permitirá loguear y informar de excepciones no handleadas.
            Application.ThreadException += new ThreadExceptionEventHandler(onThreadException);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(onUnhandledException);

            Application.Run(new MainForm());
        }

        static void onThreadException(object sender, ThreadExceptionEventArgs e)
        {
            LogUtils.LogError(e.Exception);
            MessageBox.Show(e.Exception.Message + "\nNo se asegura el correcto funcionamiento de la aplicación desde este punto.", 
                "ERROR - EXCEPCIÓN NO HANDLEADA");
        }

        static void onUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            LogUtils.LogError(ex);
            MessageBox.Show(ex.Message + "\nNo se asegura el correcto funcionamiento de la aplicación desde este punto.",
                "ERROR - EXCEPCIÓN NO HANDLEADA");
        }

    }
}
