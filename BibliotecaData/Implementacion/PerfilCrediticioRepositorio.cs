using BibliotecaData.Configuracion;
using BibliotecaData.Contrato;
using BibliotecaEntidades;
using BibliotecaEntidades.DTOs;
using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;
using System.Text.Json;

namespace BibliotecaData.Implementacion
{
    public class PerfilCrediticioRepositorio : IPerfilCrediticioRepositorio
    {
        private readonly ConnectionStrings con;
        public PerfilCrediticioRepositorio(IOptions<ConnectionStrings> options)
        {
            con = options.Value;
        }

        public async Task<string> Guardar(PerfilCrediticio perfil)
        {
            string respuesta = "";
            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_GuardarPerfilCrediticio", conexion);
                cmd.Parameters.AddWithValue("@Periodo", perfil.Periodo);
                cmd.Parameters.AddWithValue("@Numero", perfil.Numero);
                cmd.Parameters.AddWithValue("@IdTipoDocumento", perfil.IdTipoDocumento);
                cmd.Parameters.AddWithValue("@IdPais", perfil.IdPais);
                cmd.Parameters.AddWithValue("@IdUsuarioConsigna", perfil.IdUsuarioConsigna);

                cmd.Parameters.Add("@msgError", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await cmd.ExecuteNonQueryAsync();
                    respuesta = Convert.ToString(cmd.Parameters["@msgError"].Value)!;
                }
                catch
                {
                    respuesta = "Error al guardar perfil crediticio";
                }
            }
            return respuesta;
        }

        public async Task<List<PerfilCrediticioDetailsDTO>> Lista(int idUsuario)
        {
            List<PerfilCrediticioDetailsDTO> lista = new List<PerfilCrediticioDetailsDTO>();

            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_ListaPerfilCrediticio", conexion);
                cmd.Parameters.AddWithValue("@idUsuario", idUsuario);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        lista.Add(new PerfilCrediticioDetailsDTO()
                        {
                            IdPerfilCrediticio = Convert.ToInt32(dr["IdPerfilCrediticio"]),
                            Numero = dr["Numero"].ToString()!,
                            IdTipoDocumento = Convert.ToInt32(dr["IdTipoDocumento"]),
                            TipoDescripcion = dr["TipoDescripcion"].ToString()!,
                            Nombre = dr["Nombre"].ToString()!,
                            FechaVencimiento = Convert.ToDateTime(dr["FechaVencimiento"]),
                            OperacionAsociada = Convert.ToInt32(dr["OperacionAsociada"]),
                            MontoAsociado = Convert.ToInt32(dr["MontoAsociado"]),
                            Semaforo = dr["Semaforo"].ToString()!,
                            Estado = dr["Estado"].ToString()!,
                            FechaCreacion = Convert.ToDateTime(dr["FechaCreacion"])
                        });
                    }
                }
            }
            return lista;
        }

        public async Task<String> Clonar(int idPerfil)
        {

            string respuesta = "";
            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_clonarControlCampo", conexion);
                cmd.Parameters.AddWithValue("@Id", idPerfil);
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

        public async Task<int> Eliminar(int idPerfil)
        {
            int resultado = 0;

            try
            {
                using (var conexion = new SqlConnection(con.CadenaSQL))
                {
                    await conexion.OpenAsync();
                    SqlCommand cmd = new SqlCommand("sp_EliminarPerfilCrediticio", conexion);
                    cmd.Parameters.AddWithValue("@IdPerfilCrediticio", idPerfil);
                    cmd.Parameters.Add("@Result", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    await cmd.ExecuteNonQueryAsync();
                    resultado = Convert.ToInt32(cmd.Parameters["@Result"].Value)!;
                }
            }
            catch (Exception ex)
            {
                resultado = 2;
            }

            return resultado;
        }

        public async Task<PerfilCrediticioClonarDTO> Obtener(int IdPerfil)
        {
            PerfilCrediticioClonarDTO perfil = null!;

            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_ObtenerPerfilCrediticio", conexion);
                cmd.Parameters.AddWithValue("@IdPerfilCrediticio", IdPerfil);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        perfil = new PerfilCrediticioClonarDTO()
                        {
                            IdPerfilCrediticio = Convert.ToInt32(dr["IdPerfilCrediticio"]),
                            IdPais = Convert.ToInt32(dr["IdPais"]),
                            IdTipoDocumento = Convert.ToInt32(dr["IdTipoDocumento"]),
                            Numero = dr["Numero"].ToString()!
                        };
                    }
                }
            }
            return perfil;
        }

        public async Task<PerfilCrediticioCalificacionDTO> ListaCalificacion(int idPerfil)
        {
            PerfilCrediticioCalificacionDTO perfilCalificaciones = null!;

            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_ListaCalificacionInstitucion", conexion);
                cmd.Parameters.AddWithValue("@IdPerfilCrediticio", idPerfil);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        string jsonCalificaciones = dr["Calificaciones"].ToString()!;

                        perfilCalificaciones = new PerfilCrediticioCalificacionDTO()
                        {
                            IdPerfilCrediticio = Convert.ToInt32(dr["IdPerfilCrediticio"]),
                            Nombre = dr["Nombre"].ToString()!,
                            PlazoNegocio = Convert.ToInt32(dr["PlazoNegocio"]),
                            NumeroOperacion = Convert.ToInt32(dr["NumeroOperacion"]),
                            ValorAsociado = Convert.ToInt32(dr["ValorAsociado"]),
                            Semaforo = dr["Semaforo"].ToString()!,
                            Observaciones = dr["Observaciones"].ToString()!,
                            Calificaciones = string.IsNullOrWhiteSpace(jsonCalificaciones) ?
                                            new List<CalificacionInstitucion>() :
                                            JsonSerializer.Deserialize<List<CalificacionInstitucion>>(jsonCalificaciones)
                                            ?? new List<CalificacionInstitucion>()
                        };
                    }
                }
            }
            return perfilCalificaciones;
        }

        public async Task<int> Actualizar(PerfilCrediticioActualizarDTO perfil)
        {
            int resultado = 0;

            try
            {
                using (var conexion = new SqlConnection(con.CadenaSQL))
                {
                    await conexion.OpenAsync();
                    SqlCommand cmd = new SqlCommand("sp_ActualizarPerfilCrediticio", conexion);
                    cmd.Parameters.AddWithValue("@IdPerfilCrediticio", perfil.IdPerfilCrediticio);
                    cmd.Parameters.AddWithValue("@PlazoNegocio", perfil.PlazoNegocio);
                    cmd.Parameters.AddWithValue("@NumeroOperacion", perfil.NumeroOperacion);
                    cmd.Parameters.AddWithValue("@ValorAsociado", perfil.ValorAsociado);
                    cmd.Parameters.AddWithValue("@Semaforo", perfil.Semaforo);
                    cmd.Parameters.AddWithValue("@Observaciones", perfil.Observaciones);
                    cmd.CommandType = CommandType.StoredProcedure;

                    resultado = await cmd.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                resultado = 2;
            }

            return resultado;
        }
    }
}