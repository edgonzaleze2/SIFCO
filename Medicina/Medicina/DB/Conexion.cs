using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace Medicina.DB
{
    public class Conexion
    {
        private string cConexion;
        public string _conexion = System.Configuration.ConfigurationManager.AppSettings.Get("cadenaDB");

        public Conexion()
        {
            this.cConexion = _conexion;
        }

        public Conexion(string conexion)
        {
            this.cConexion = conexion;
        }


        public void Insertar(string nombreProcedimiento, List<DaoParametro> parametros)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(this.cConexion))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand(nombreProcedimiento, cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        foreach (var item in parametros)
                        {
                            var param = new SqlParameter(item.Nombre, item.Tipo)
                            {
                                Value = item.Valor ?? DBNull.Value
                            };
                            cmd.Parameters.Add(param);

                        }
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Utilidades.Logs.RegistrarLog("problemas la insertar" + ex.Message); 
            }
            
        }


        public void Actualizar(string nombreProcedimiento, List<DaoParametro> parametros)
        {
            using (SqlConnection cn = new SqlConnection(this.cConexion))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand(nombreProcedimiento, cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    foreach (var item in parametros)
                    {
                        var param = new SqlParameter(item.Nombre, item.Tipo)
                        {
                            Value = item.Valor ?? DBNull.Value
                        };
                        if (item.Out)
                            param.Direction = ParameterDirection.Output;
                    }
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Eliminar(string nombreProcedimiento, List<DaoParametro> parametros)
        {
            using (SqlConnection cn = new SqlConnection(this.cConexion))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand(nombreProcedimiento, cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    foreach (var item in parametros)
                    {
                        var param = new SqlParameter(item.Nombre, item.Tipo)
                        {
                            Value = item.Valor ?? DBNull.Value
                        };
                        if (item.Out)
                            param.Direction = ParameterDirection.Output;
                    }
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public DataTable Consultar(string nombreProcedimiento, List<DaoParametro> parametros = null)
        {
            DataTable dtlDatos = new DataTable("datos");
            using (SqlConnection cn = new SqlConnection(this.cConexion))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand(nombreProcedimiento, cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (parametros != null)
                    {
                        foreach (var item in parametros)
                        {
                            var param = new SqlParameter(item.Nombre, item.Tipo)
                            {
                                Value = item.Valor ?? DBNull.Value
                            };
                            if (item.Out)
                                param.Direction = ParameterDirection.Output;
                        }
                    }
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dtlDatos);
                    }
                }
            }
            return dtlDatos;
        }


        // dos llaves a partir de ahorita
    }

    public class DaoParametro
    {
        public string Nombre;
        public object Valor;
        public SqlDbType Tipo;
        public bool Out;
    }
}