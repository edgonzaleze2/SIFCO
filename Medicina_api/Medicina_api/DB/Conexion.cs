using System.Data;
using Microsoft.Data.SqlClient;

namespace Medicina_api.DB
{
    public class Conexion
    {
        private readonly string _cadenaConexion;

        public Conexion()
        {
            var config = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
               .Build();

            _cadenaConexion = config.GetConnectionString("ConexionGT") ?? throw new InvalidOperationException("Cadena de conexión no encontrada.");
        }

        public void Insertar(string nombreProcedimiento, List<DaoParametro> parametros)
        {

            try
            {
                using (SqlConnection cn = new SqlConnection(_cadenaConexion))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = nombreProcedimiento;

                    // Añadir los parámetros con SqlParameter, especificando explícitamente el tipo
                    foreach (var item in parametros)
                    {
                        SqlParameter param = new SqlParameter
                        {
                            ParameterName = item.Nombre,
                            Value = item.Valor,
                            SqlDbType = ConvertToSqlDbType(item.Tipo)
                        };

                        cmd.Parameters.Add(param);
                    }

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string mensaje = reader[0].ToString();
                            Console.WriteLine("Mensaje desde SP: " + mensaje);
                        }
                    }
                    // cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }


        }
        public void Actualizar(string nombreProcedimiento, List<DaoParametro> parametros)
        {
            using (SqlConnection cn = new SqlConnection(_cadenaConexion))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand(nombreProcedimiento, cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    foreach (var item in parametros)
                    {
                        SqlParameter parametro = new SqlParameter(item.Nombre, item.Valor)
                        {
                            DbType = item.Tipo
                        };
                        cmd.Parameters.Add(parametro);
                    }

                    cmd.ExecuteNonQuery();
                }
            }
        }


        public void Eliminar(string nombreProcedimiento, List<DaoParametro> parametros)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(_cadenaConexion))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand(nombreProcedimiento, cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Agregar parámetros
                        foreach (var item in parametros)
                        {
                            cmd.Parameters.AddWithValue(item.Nombre, item.Valor).DbType = item.Tipo;
                        }

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public DataTable Consultar(string nombreProcedimiento, List<DaoParametro> parametros = null)
        {
            DataTable dtlDatos = new DataTable("datos");
            try
            {
                using (SqlConnection cn = new SqlConnection(_cadenaConexion))
                {
                    cn.Open();
                    using (SqlCommand cmd = cn.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = nombreProcedimiento;

                        if (parametros != null)
                        {
                            foreach (var item in parametros)
                            {
                                cmd.Parameters.AddWithValue(item.Nombre, item.Valor).DbType = item.Tipo;
                            }
                        }

                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd))
                        {
                            sqlDataAdapter.Fill(dtlDatos);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return dtlDatos;
        }

        private SqlDbType ConvertToSqlDbType(DbType dbType)
        {
            return dbType switch
            {
                DbType.Int16 => SqlDbType.SmallInt,
                DbType.Int32 => SqlDbType.Int,
                DbType.Int64 => SqlDbType.BigInt,
                DbType.String => SqlDbType.NVarChar,
                DbType.DateTime => SqlDbType.DateTime,
                DbType.Decimal => SqlDbType.Decimal,
                DbType.Boolean => SqlDbType.Bit,
                DbType.Double => SqlDbType.Float,
                DbType.Guid => SqlDbType.UniqueIdentifier,
                DbType.Byte => SqlDbType.TinyInt,
                DbType.Binary => SqlDbType.VarBinary,
                _ => throw new ArgumentOutOfRangeException(nameof(dbType), $"No hay conversión definida para {dbType}")
            };
        }

        public class DaoParametro
        {


            /// <summary>
            /// Nombre del parametro.
            /// </summary>
            public string Nombre;

            /// <summary>
            /// Valor del parametro.
            /// </summary>
            public Object Valor;

            /// <summary>
            /// Tipo de dato del parametro.
            /// </summary>
            public System.Data.DbType Tipo;

            /// <summary>
            /// True si es un parametro de salida del procedimiento almacenado.
            /// </summary>
            public bool Out;

        }


    }
}
