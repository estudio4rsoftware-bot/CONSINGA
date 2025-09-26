using BibliotecaData.Contrato;
using BibliotecaEntidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BibliotecaData.Configuracion;
using Microsoft.Extensions.Options;

namespace BibliotecaData.Implementacion
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly ConnectionStrings con;
        public UsuarioRepositorio(IOptions<ConnectionStrings> options)
        {
            con = options.Value;
        }

        public async Task<string> Editar(Usuario objeto)
        {
            string respuesta = "";
            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_editarUsuario", conexion);
                cmd.Parameters.AddWithValue("@IdUsuario", objeto.IdUsuario);
                cmd.Parameters.AddWithValue("@NombreCompleto", objeto.NombreCompleto);
                cmd.Parameters.AddWithValue("@NombreUsuario", objeto.NombreUsuario);
                cmd.Parameters.AddWithValue("@Clave", objeto.Clave);
                cmd.Parameters.AddWithValue("@MailDeContacto", objeto.MailDeContacto);

                cmd.Parameters.AddWithValue("@UsuarioSnig", objeto.UsuarioSnig);
                cmd.Parameters.AddWithValue("@ClaveSnig", objeto.ClaveSnig);

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

        public async Task<int> Eliminar(int IdUsuario)
        {
            int respuesta = 1;
            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_eliminarUsuario", conexion);
                cmd.Parameters.AddWithValue("@IdUsuario", IdUsuario);
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

        public async Task<string> Guardar(Usuario objeto)
        {
            string respuesta = "";
            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_guardarUsuario", conexion);
                cmd.Parameters.AddWithValue("@NombreCompleto", objeto.NombreCompleto);
                cmd.Parameters.AddWithValue("@NombreUsuario", objeto.NombreUsuario);
                cmd.Parameters.AddWithValue("@Clave", objeto.Clave);
                cmd.Parameters.AddWithValue("@MailDeContacto", objeto.MailDeContacto);
                cmd.Parameters.AddWithValue("@UsuarioSnig", objeto.UsuarioSnig);
                cmd.Parameters.AddWithValue("@ClaveSnig", objeto.ClaveSnig);

                cmd.Parameters.Add("@msgError", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await cmd.ExecuteNonQueryAsync();
                    respuesta = Convert.ToString(cmd.Parameters["@msgError"].Value)!;
                }
                catch
                {
                    respuesta = "Error al guardar usuario";
                }

            }
            return respuesta;
        }

        public async Task<List<Usuario>> Lista(String oEmpresa)
        {
            List<Usuario> lista = new List<Usuario>();

            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_listaUsuario", conexion);
                cmd.Parameters.AddWithValue("@oEmpresa", oEmpresa);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        lista.Add(new Usuario()
                        {
                            IdUsuario = Convert.ToInt32(dr["IdUsuario"]),
                            NombreCompleto = dr["NombreCompleto"].ToString()!,
                            NombreUsuario = dr["NombreUsuario"].ToString()!,
                            Clave = dr["Clave"].ToString()!,
                            FechaCreacion = dr["FechaCreacion"].ToString()!
                        });
                    }
                }
            }
            return lista;
        }

        public async Task<Usuario> Login(string NombreUsuario, string Clave)
        {
            Usuario objeto = null!;

            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_loginUsuario", conexion);
                cmd.Parameters.AddWithValue("@NombreUsuario", NombreUsuario);
                cmd.Parameters.AddWithValue("@Clave", Clave);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        objeto = new Usuario()
                        {
                            IdUsuario = Convert.ToInt32(dr["IdUsuario"]),
                            NombreCompleto = dr["NombreCompleto"].ToString()!,
                            NombreUsuario = dr["NombreUsuario"].ToString()!,
                            Clave = dr["Clave"].ToString()!,
                            FechaCreacion = dr["FechaCreacion"].ToString()!,
                            Rol = dr["Rol"].ToString()!,
                               oEmpresa = new Empresa
                               {
                                   IdEmpresa = Convert.ToInt32(dr["IdEmpresa"].ToString()),
                                   Nombre = dr["Nombre"].ToString()!
                               },
                        };
                    }
                }
            }
            return objeto;
        }

        public async Task<Usuario> Obtener(int IdUsuario)
        {
            Usuario objeto = null!;

            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_obtenerUsuario", conexion);
                cmd.Parameters.AddWithValue("@IdUsuario", IdUsuario);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        objeto = new Usuario()
                        {
                            IdUsuario = Convert.ToInt32(dr["IdUsuario"]),
                            NombreCompleto = dr["NombreCompleto"].ToString()!,
                            NombreUsuario = dr["NombreUsuario"].ToString()!,
                            Clave = dr["Clave"].ToString()!,
                            FechaCreacion = dr["FechaCreacion"].ToString()!,
                            UsuarioSnig = dr["UsuarioSnig"].ToString()!,
                            ClaveSnig = dr["ClaveSnig"].ToString()!,
                            MailDeContacto = dr["Email"].ToString()!
                        };
                    }
                }
            }
            return objeto;
        }
    }
}
