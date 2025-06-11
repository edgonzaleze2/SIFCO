using System.Text;

namespace Medicina_api.Utilidades
{
    public class Logs
    {

        public static void RegistrarLog(string contenido)
        {

            try
            {

                var config = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                 .Build();


                string path = config["AppSettings:LOGS"] ?? "C:/DefaultLogs/app.log";

                string nombreArchivo = DateTime.Now.ToString("ddMMyyyy") + ".log";
                string archivoLog = path + "\\" + nombreArchivo;
                contenido = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + contenido;

                using (StreamWriter sw = new StreamWriter(archivoLog, true, Encoding.UTF8))
                {

                    sw.WriteLine(contenido);
                    sw.Close();
                }

            }
            catch (Exception)
            {
            }
        }
    }

}
