using static Medicina_api.DB.Conexion;
using System.Data;
using Medicina_api.Models;
using Medicina_api.Utilidades;

namespace Medicina_api.DB
{
    public class Consultas
    {
        Conexion conexion = new Conexion();
        public ResultModel ObtenerPaciente(string dpi)
        {
            ResultModel resultado = new ResultModel();
            DataTable dtDatos = new DataTable();
            List<Paciente> list = new List<Paciente>();
            try
            {
                List<DaoParametro> lstparametros = new List<DaoParametro>();
                lstparametros.Add(new DaoParametro { Nombre = "@p_opcion", Valor = 2, Tipo = System.Data.DbType.Int32 });
                lstparametros.Add(new DaoParametro { Nombre = "@p_dpi", Valor = dpi, Tipo = System.Data.DbType.Int32 });
                dtDatos = conexion.Consultar("sp_Paciente_CRUD", lstparametros);
                foreach (DataRow dr in dtDatos.Rows)
                {
                    Paciente resPac = new Paciente();
                    resPac.codigo = (int)dr["codigo"];
                    resPac.nombrecompleto = dr["nombrecompleto"].ToString();
                    resPac.fechanacimiento = dr["fechanacimiento"].ToString();
                    resPac.sexo = dr["sexo"].ToString();
                    resPac.dpi = dr["dpi"].ToString();
                    resPac.fechaingreso = dr["fechaingreso"].ToString();
                    

                    list.Add(resPac);
                }

                resultado.Estado = true;
                resultado.Mensaje = "informacion Consultada correctamente";
                resultado.Objeto = list;
            }
            catch (Exception ex)
            {
                resultado.Estado = false;
                resultado.Mensaje = "No se pudo completar la operacion";
                Utilidades.Logs.RegistrarLog("Problemas al consulta el SP " + ex.Message);
            }

            return resultado;
        }


        public ResultModel InsertarPaciente(Paciente paciente)
        {
            ResultModel resultModel = new ResultModel();
            try
            {
                List<DaoParametro> lstparametros = new List<DaoParametro>();
                //opcion
                lstparametros.Add(new DaoParametro { Nombre = "@p_opcion", Valor = 1, Tipo = System.Data.DbType.Int16 });
                // peronsasl 
                lstparametros.Add(new DaoParametro { Nombre = "@p_nombrecompleto", Valor = paciente.nombrecompleto, Tipo = System.Data.DbType.String });
                lstparametros.Add(new DaoParametro { Nombre = "@p_fechanacimiento", Valor = paciente.fechanacimiento, Tipo = System.Data.DbType.String });
                lstparametros.Add(new DaoParametro { Nombre = "@p_sexo", Valor = paciente.sexo, Tipo = System.Data.DbType.String });
                lstparametros.Add(new DaoParametro { Nombre = "@p_dpi", Valor = paciente.dpi, Tipo = System.Data.DbType.String });
                conexion.Insertar("sp_Paciente_CRUD", lstparametros);
                resultModel.Estado = true;
                resultModel.Mensaje = "Paciente ingresado Correctamente";
                Logs.RegistrarLog("paciente ingresado corrrectamente");

            }
            catch (Exception ex)
            {
                Logs.RegistrarLog("Problemas al ingresar la informacion " + ex.Message);
                resultModel.Estado = false;
                resultModel.Mensaje = "Problemas al ingresar la informacion";
            }

            return resultModel;

        }


        public ResultModel ObtenerPaciente_orden(string orden)
        {
            ResultModel resultado = new ResultModel();
            DataTable dtDatos = new DataTable();
            List<Paciente> ListaPacientes = new List<Paciente>();
            try
            {
                List<DaoParametro> lstparametros = new List<DaoParametro>();
                lstparametros.Add(new DaoParametro { Nombre = "@p_opcion", Valor = 3, Tipo = System.Data.DbType.Int32 });
                dtDatos = conexion.Consultar("sp_Paciente_CRUD", lstparametros);
                foreach (DataRow dr in dtDatos.Rows)
                {
                    Paciente resPac = new Paciente();
                    resPac.codigo = (int)dr["codigo"];
                    resPac.nombrecompleto = dr["nombrecompleto"].ToString();
                    resPac.fechanacimiento = dr["fechanacimiento"].ToString();
                    resPac.sexo = dr["sexo"].ToString();
                    resPac.dpi = dr["dpi"].ToString();
                    resPac.fechaingreso = dr["fechaingreso"].ToString();
                    ListaPacientes.Add(resPac);
                }


                resultado.Estado = true;
                resultado.Mensaje = "informacion Consultada correctamente";
                resultado.Objeto = Orden(ListaPacientes, orden);
            }
            catch (Exception ex)
            {
                resultado.Estado = false;
                resultado.Mensaje = "No se pudo completar la operacion";
                Utilidades.Logs.RegistrarLog("Problemas al consulta el SP " + ex.Message);
            }

            return resultado;
        }

        public List<Paciente> Orden(List<Paciente> pacientes, string orden) {
            if (orden == "D")
                return pacientes.OrderByDescending(p => p.codigo).ToList();
            else
                return pacientes.OrderBy(p => p.codigo).ToList();
         
        }


        // dos llaves
    }
}
