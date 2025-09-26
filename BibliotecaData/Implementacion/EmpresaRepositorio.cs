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

namespace BibliotecaData.Implementacion
{
    public class EmpresaRepositorio:IEmpresaRepositorio
    {
        private readonly ConnectionStrings con;
        public EmpresaRepositorio(IOptions<ConnectionStrings> options)
        {
            con = options.Value;
        }
        public async Task<List<Empresa>> Lista(String oEmpresa) {
            List<Empresa> lista = new List<Empresa>();

            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_listaEmpresa", conexion);
                cmd.Parameters.AddWithValue("@oEmpresa", oEmpresa);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        lista.Add(new Empresa()
                        {
                            IdEmpresa = Convert.ToInt32(dr["IdEmpresa"]),
                            Nombre = dr["Nombre"].ToString()!,
                            RUT = dr["RUT"].ToString()!,
                            Direccion = dr["Direccion"].ToString()!,
                            Mail = dr["Mail"].ToString()!,
                            ClaveSnig = dr["ClaveSnig"].ToString()!,
                            UsuarioSnig = dr["UsuarioSnig"].ToString()!
                        });
                    }
                }
            }
            return lista;
        }
        public async Task<Empresa> Obtener(int IdEmpresa) {
            Empresa objeto = null!;

            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_obtenerEmpresa", conexion);
                cmd.Parameters.AddWithValue("@IdEmpresa", IdEmpresa);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        objeto = new Empresa()
                        {
                            IdEmpresa = Convert.ToInt32(dr["IdEmpresa"]),
                            Nombre = dr["Nombre"].ToString()!,
                            Direccion = dr["Direccion"].ToString()!,
                            RUT = dr["Rut"].ToString()!,
                            UsuarioSnig = dr["UsuarioSnig"].ToString()!,
                            ClaveSnig = dr["ClaveSnig"].ToString()!,
                            Mail = dr["Mail"].ToString()!
                        };
                    }
                }
            }
            return objeto;
        }
    
        public async Task<string>  Guardar(Empresa objeto) { return null; }
        public async Task<string>  Editar(Empresa objeto) { return null; }
        public async Task<int>  Eliminar(int IEmpresa) { return 0; }


    }
}
