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
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BibliotecaData.Implementacion
{
    public class AlertaRepositorio : IAlertaRepositorio
    {


        private readonly ConnectionStrings con;
        public AlertaRepositorio(IOptions<ConnectionStrings> options)
        {
            con = options.Value;
        }
        public async Task<List<AlertaAgrupadaEtiqueta>> ListaAgrupadaEtiqueta(String empresa)
        {
            List<AlertaAgrupadaEtiqueta> lista = new List<AlertaAgrupadaEtiqueta>();

            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_listaAlertaAgrupadaEtiqueta", conexion);
                cmd.Parameters.AddWithValue("@oEmpresa", empresa);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        lista.Add(new AlertaAgrupadaEtiqueta()
                        {

                            Etiqueta = dr["Etiqueta"].ToString()!,
                            CantidadAlertas = Convert.ToInt32(dr["CantidadAlertas"]),
                            PrimerControl = dr["PrimerControl"].ToString()!,
                            UltimoControl = dr["UltimoControl"].ToString()!,
                            Estado = dr["Estado"].ToString()!,
                            oEmpresa = dr["oEmpresa"].ToString()!

                        });
                    }

                }
            }
            return lista;

        }

        public async Task<List<Alerta>> Lista(String oEmpresa)
        {

            List<Alerta> lista = new List<Alerta>();

            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_listaAlerta", conexion);
                cmd.Parameters.AddWithValue("@oEmpresa", oEmpresa);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        lista.Add(new Alerta()
                        {
                            IdAlerta = Convert.ToInt32(dr["Id"]),
                            NroCaravana = dr["NroCaravana"].ToString()!,
                            Propiedad = dr["Propiedad"].ToString()!,
                            Tenencia = dr["Tenencia"].ToString()!,
                            LugarFisico = dr["LugarFisico"].ToString()!,
                            FechaControl = dr["FechaControl"].ToString()!,
                            Seguimiento = dr["Seguimiento"].ToString()!,
                            Cambio = dr["Cambio"].ToString()!,
                            PropiedadAnterior = dr["PropiedadAnterior"].ToString()!,
                            TenenciaAnterior = dr["TenenciaAnterior"].ToString()!,
                            LugarFisicoAnterior = dr["LugarFisicoAnterior"].ToString()!,
                            FechaIngreso = dr["FechaIngreso"].ToString()!,
                            FechaFin = dr["FechaFin"].ToString()!,
                            Estado = dr["Estado"].ToString()!,
                            FechaUltimoControl = dr["FechaUltimoControl"].ToString()!

                        });
                    }

                }
            }
            return lista;

        }

        public async Task<Alerta> Obtener(int idAlerta)
        {


            Alerta objeto = null!;

            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_obtenerAlerta", conexion);
                cmd.Parameters.AddWithValue("@IdAlerta", idAlerta);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        objeto = new Alerta()
                        {
                            IdAlerta = Convert.ToInt32(dr["Id"]),
                            NroCaravana = dr["NroCaravana"].ToString()!,
                            Propiedad = dr["Propiedad"].ToString()!,
                            Tenencia = dr["Tenencia"].ToString()!,
                            LugarFisico = dr["LugarFisico"].ToString()!,
                            FechaControl = dr["FechaControl"].ToString()!,
                            Seguimiento = dr["Seguimiento"].ToString()!,
                            Cambio = dr["Cambio"].ToString()!,
                            PropiedadAnterior = dr["PropiedadAnterior"].ToString()!,
                            TenenciaAnterior = dr["TenenciaAnterior"].ToString()!,
                            LugarFisicoAnterior = dr["LugarFisicoAnterior"].ToString()!,
                            FechaIngreso = dr["FechaIngreso"].ToString()!,
                            FechaFin = dr["FechaFin"].ToString()!,
                            Estado = dr["Estado"].ToString()!,
                            FechaUltimoControl = dr["FechaUltimoControl"].ToString()!
                        };

                       // var nombre = objeto.Documento;
                    }
                }
            }
            return objeto;

        }

        public async Task<string> Guardar(Alerta objeto)
        {
           string respuesta = "";
            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_guardarAlerta", conexion);
                cmd.Parameters.AddWithValue("@NroCaravana", objeto.NroCaravana);
                cmd.Parameters.AddWithValue("@Propiedad", "");
                cmd.Parameters.AddWithValue("@Tenencia", "");
                cmd.Parameters.AddWithValue("@LugarFisico", "");
                cmd.Parameters.AddWithValue("@FechaControl", "");
                cmd.Parameters.AddWithValue("@Cambio", "");
                cmd.Parameters.AddWithValue("@PropiedadAnterior", "");
                cmd.Parameters.AddWithValue("@TenenciaAnterior", "");
                cmd.Parameters.AddWithValue("@LugarFisicoAnterior", "");
                cmd.Parameters.AddWithValue("@FechaIngreso", DateTime.Today.ToString());
                cmd.Parameters.AddWithValue("@FechaFin", "");
                cmd.Parameters.AddWithValue("@Estado", "Nuevo");
                cmd.Parameters.AddWithValue("@FechaUltimoControl", "");
                cmd.Parameters.AddWithValue("@Seguimiento", "");

                cmd.Parameters.AddWithValue("@oFrecuenciaAlerta", objeto.oFrecuenciaAlerta);
                cmd.Parameters.AddWithValue("@oUsuario", objeto.oUsuario);

                cmd.Parameters.AddWithValue("@Etiqueta", objeto.Etiqueta);
                cmd.Parameters.AddWithValue("@oEmpresa", objeto.oEmpresa);
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
        public async Task<string> Editar(Alerta objeto)
        {
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


        public async Task<int> Anular(int IdAlerta)
        {
            int respuesta = 1;
            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_anularAlerta", conexion);
                 cmd.Parameters.AddWithValue("@IdAlerta", IdAlerta);
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


        public async Task<int> ActivarAgrupadas(string etiqueta, string oEmpresa)
        {
            int respuesta = 1;
            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_activarAlertaAgrupada", conexion);
                cmd.Parameters.AddWithValue("@Etiqueta", etiqueta);
                cmd.Parameters.AddWithValue("@oEmpresa", oEmpresa);
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

        public async Task<int> DesactivarAgrupadas(string etiqueta, string oEmpresa)
        {
            int respuesta = 1;
            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_anularAlertaAgrupada", conexion);
                cmd.Parameters.AddWithValue("@Etiqueta", etiqueta);
                cmd.Parameters.AddWithValue("@oEmpresa", oEmpresa);
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

        public async Task<int> Eliminar(int IdAlerta)
        {
            int respuesta = 1;
            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_eliminarUsuario", conexion);
               // cmd.Parameters.AddWithValue("@IdUsuario", IdEmpresa);
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

       /* Task<List<Alerta>> IAlertaRepositorio.Lista()
        {
            throw new NotImplementedException();
        }

        Task<Alerta> IAlertaRepositorio.Obtener(int IdAlerta)
        {
            throw new NotImplementedException();
        }

        public Task<string> Guardar(Alerta objeto)
        {
            throw new NotImplementedException();
        }

        public Task<string> Editar(Alerta objeto)
        {
            throw new NotImplementedException();
        }*/
            }
        }
