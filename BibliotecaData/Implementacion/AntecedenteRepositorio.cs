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
    public class AntecedenteRepositorio:IAntecedenteRepositorio
    {
        private readonly ConnectionStrings con;
        public AntecedenteRepositorio(IOptions<ConnectionStrings> options)
        {
            con = options.Value;
        }
        public async Task<List<Antecedente>> Lista() {

            List<Antecedente> lista = new List<Antecedente>();

            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_listaAntecedente", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        lista.Add(new Antecedente()
                        {
                            IdAntecedente = Convert.ToInt32(dr["IdAntecedentes"]),
                            Documento = dr["Documento"].ToString()!,
                            Estado = dr["Estado"].ToString()!,
                            ResultadoBCU = dr["ResultadoBCU"].ToString()!,
                            ResultadoDGI = dr["ResultadoDGI"].ToString()!,
                            ResultadoClearing = dr["ResultadoClearing"].ToString()!,
                            ResultadoOtro = dr["ResultadoOtro"].ToString()!,
                            FechaIngreso = dr["FechaIngreso"].ToString()!
                            
                        });
                    }

                }
            }
            return lista;

        }

        public async Task<Antecedente> Obtener(int IdAntecedente) {

           
                Antecedente objeto = null!;

                using (var conexion = new SqlConnection(con.CadenaSQL))
                {
                    await conexion.OpenAsync();
                    SqlCommand cmd = new SqlCommand("sp_obtenerAntecedente", conexion);
                    cmd.Parameters.AddWithValue("@IdAntecedente", IdAntecedente);
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (var dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {

                          objeto = new Antecedente()
                          {
                              IdAntecedente = IdAntecedente,
                              FechaIngreso = dr["FechaIngreso"].ToString()!,
                              Documento = dr["Documento"].ToString()!,
                              ResultadoBCU =  dr["ResultadoBCU"].ToString()!,
                              ResultadoDGI =  dr["ResultadoDGI"].ToString()!,
                              ResultadoClearing = dr["ResultadoClearing"].ToString()!,
                              ResultadoOtro =dr["ResultadoOtro"].ToString()!,
                              UsuarioId =  dr["UsuarioId"].ToString()!,
                              EmpresaId =  dr["EmpresaId"].ToString()!,
                              Estado  =dr["Estado"].ToString()!
                          };

                        var nombre = objeto.Documento;
                        }
                    }
                }
                return objeto;
            
        }

        public async Task<string> Guardar(Antecedente objeto) {

            string respuesta = "";
            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_guardarAntecedentes", conexion);
                cmd.Parameters.AddWithValue("@Documento", objeto.Documento);
                cmd.Parameters.AddWithValue("@Usuario", objeto.UsuarioId);
                cmd.Parameters.AddWithValue("@Empresa", objeto.EmpresaId);
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
        public async Task<string> Editar(Antecedente objeto) {
            string respuesta = "";
            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_editarUsuario", conexion);
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
        public async Task<int> Eliminar(int IdEmpresa) {
            int respuesta = 1;
            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_eliminarUsuario", conexion);
                cmd.Parameters.AddWithValue("@IdUsuario", IdEmpresa);
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
