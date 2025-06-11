using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Medicina.DB
{
    public class Consultas
    {
        Conexion ConObj = new Conexion();

        public DataTable Consultarpaciente() {
            DataTable dts = new DataTable();
            try
            {
                dts = ConObj.Consultar("SP_consulta_paciente"); 

            }
            catch (Exception ex)
            {
                Utilidades.Logs.RegistrarLog("Problemas al consultar la data" + ex.Message);
             
            }
            return dts;
        }

        public bool IngresarPaciente(Models.Paciente paciente) {
            bool respuesta = false; 
            try
            {
                List<DaoParametro> lstparametros = new List<DaoParametro>();
                //opcion
                lstparametros.Add(new DaoParametro { Nombre = "@p_opcion", Valor = 1, Tipo = System.Data.SqlDbType.Int });
                // peronsasl 
                lstparametros.Add(new DaoParametro { Nombre = "@p_nombrecompleto", Valor = paciente.nombrecompleto, Tipo = System.Data.SqlDbType.NVarChar });
                lstparametros.Add(new DaoParametro { Nombre = "@p_fechanacimiento", Valor = paciente.fecha_nacimiento, Tipo = System.Data.SqlDbType.NVarChar });
                lstparametros.Add(new DaoParametro { Nombre = "@p_sexo", Valor = paciente.genero, Tipo = System.Data.SqlDbType.NVarChar });
                lstparametros.Add(new DaoParametro { Nombre = "@p_dpi", Valor = paciente.dpi, Tipo = System.Data.SqlDbType.NVarChar });
                ConObj.Insertar("sp_Paciente_CRUD", lstparametros);
                respuesta = true;
            }
            catch (Exception ex)
            {
                Utilidades.Logs.RegistrarLog("Problemas al insertar " + ex.Message);
                respuesta = false; 
            }
            return respuesta;       
        }

    }
}