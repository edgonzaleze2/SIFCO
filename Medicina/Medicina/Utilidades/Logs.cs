using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Medicina.Utilidades
{
    public class Logs
    {
        private static readonly object logLock = new object();

        public static void RegistrarLog(string contenido)
        {
            try
            {
                lock (logLock)
                {
                    string path = System.Configuration.ConfigurationManager.AppSettings.Get("LOGS").ToString();

                    if (!System.IO.Directory.Exists(path))
                    {
                        System.IO.Directory.CreateDirectory(path);
                    }


                    string nombreArchivo = DateTime.Now.ToString("ddMMyyyy") + ".log";
                    string archivoLog = path + "\\" + nombreArchivo;
                    contenido = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + contenido;
                    Console.WriteLine(contenido);
                    using (StreamWriter sw = new StreamWriter(archivoLog, true, Encoding.UTF8))
                    {

                        sw.WriteLine(contenido);
                        sw.Close();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

    }
}