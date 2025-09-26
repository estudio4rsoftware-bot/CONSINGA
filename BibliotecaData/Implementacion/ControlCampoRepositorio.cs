using BibliotecaData.Configuracion;
using BibliotecaData.Contrato;
using BibliotecaEntidades;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaData.Implementacion
{
    public class ControlCampoRepositorio:IControlCampoRepositorio
    {
        private readonly ConnectionStrings con;
        public ControlCampoRepositorio(IOptions<ConnectionStrings> options)
        {
            con = options.Value;
        }
        public async Task<List<ControlCampo>> Lista(String oEmpresa)
        {

            List<ControlCampo> lista = new List<ControlCampo>();

            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_listaControlCampo", conexion);
                cmd.Parameters.AddWithValue("@oEmpresa", oEmpresa);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        lista.Add(new ControlCampo()
                        {
                            IdControlCampo = Convert.ToInt32(dr["IdControlCampo"]),
                            Estado = dr["Estado"].ToString()!,
                            Archivo = dr["ArchivoEnvio"].ToString()!,
                            oUsuario = dr["oUsuario"].ToString()!,
                            oEmpresa = dr["oEmpresa"].ToString()!,
                            ResultadoBase64 = dr["ArchivoResultado"].ToString()!,
                            FechaCreacion = dr["FechaCreacion"].ToString()!,
                            ConAlerta = dr["ArchivoResultado"].ToString()!,
                         //   ArchivoResultado = dr["ArchivoResultado"].ToString()!,
                            Observaciones = dr["Observaciones"].ToString()!,
                            Nombre = dr["Nombre"].ToString()!,

                        });
                    }

                }
            }
            return lista;

        }
       
        public async Task<ControlCampo> Obtener(int IdControlCampo)
        {

            ControlCampo objeto = null!;
             using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_obtenerControlCampo", conexion);
                cmd.Parameters.AddWithValue("@IdControlCampo", IdControlCampo);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        objeto = new ControlCampo()
                        {
                            IdControlCampo = Convert.ToInt32(dr["IdControlCampo"]),
                            Estado = dr["Estado"].ToString()!,
                            Archivo = dr["ArchivoEnvio"].ToString()!,
                            oUsuario = dr["oUsuario"].ToString()!,
                            oEmpresa = dr["oEmpresa"].ToString()!,
                            ArchivoResultado = dr["ArchivoResultado"].ToString()!,
                              FechaCreacion = dr["FechaCreacion"].ToString()!,
                            //ConAlerta = dr["ArchivoResultado"].ToString()!,
                            //   ArchivoResultado = dr["ArchivoResultado"].ToString()!,
                            Observaciones = dr["Observaciones"].ToString()!,
                            Nombre = dr["Nombre"].ToString()!,
                        };

                    }
                }
            }
            return objeto;

        }


        public async Task<String> Clonar(int IdControlCampo)
        {

            string respuesta = "";
            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_clonarControlCampo", conexion);
                cmd.Parameters.AddWithValue("@Id", IdControlCampo);
                cmd.Parameters.Add("@msgError", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await cmd.ExecuteNonQueryAsync();
                    respuesta = Convert.ToString(cmd.Parameters["@msgError"].Value)!;
                }
                catch
                {
                    respuesta = "Error al clonar el control de campo";
                }
            }
            return respuesta;

        }

        public async Task<string> Guardar(ControlCampo objeto)
        {

            string respuesta = "";
            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_guardarControlCampo", conexion);
                cmd.Parameters.AddWithValue("@Archivo", objeto.Archivo);
                cmd.Parameters.AddWithValue("@oUsuario", objeto.oUsuario);
                cmd.Parameters.AddWithValue("@oEmpresa", objeto.oEmpresa);
                cmd.Parameters.AddWithValue("@ConAlerta", objeto.ConAlerta);
                cmd.Parameters.AddWithValue("@FrecuenciaAlerta", objeto.FrecuenciaAlerta);
                cmd.Parameters.AddWithValue("@Nombre", objeto.Nombre);


                cmd.Parameters.Add("@msgError", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await cmd.ExecuteNonQueryAsync();
                    respuesta = Convert.ToString(cmd.Parameters["@msgError"].Value)!;
                }
                catch
                {
                    respuesta = "Error al guardar el control de campo";
                }

            }
            return respuesta;
        }
        public async Task<string> Editar(ControlCampo objeto)
        {
            string respuesta = "";
            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_editarControlCampo", conexion);
                //cmd.Parameters.AddWithValue("@IdUsuario", objeto.IdUsuario);
                //cmd.Parameters.AddWithValue("@NombreCompleto", objeto.NombreCompleto);
                //cmd.Parameters.AddWithValue("@NombreUsuario", objeto.NombreUsuario);
                //cmd.Parameters.AddWithValue("@Clave", objeto.Clave);
                cmd.Parameters.Add("@msgError", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await cmd.ExecuteNonQueryAsync();
                    respuesta = Convert.ToString(cmd.Parameters["@msgError"].Value)!;
                }
                catch
                {
                    respuesta = "Error al editar usuario";
                }

            }
            return respuesta;
        }
        public async Task<int> Eliminar(int IdControlCampo)
        {
            int respuesta = 1;
            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_eliminarControlCampo", conexion);
                cmd.Parameters.AddWithValue("@IdControlCampo", IdControlCampo);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await cmd.ExecuteNonQueryAsync();
                }
                catch
                {
                    respuesta = 0;
                }

            }
            return respuesta;
        }
    }




}

