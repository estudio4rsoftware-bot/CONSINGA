using BibliotecaData.Configuracion;
using BibliotecaData.Contrato;
using BibliotecaEntidades;
using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;

namespace BibliotecaData.Implementacion
{
    public class PaisRepositorio : IPaisRepositorio
    {
        private readonly ConnectionStrings con;
        public PaisRepositorio(IOptions<ConnectionStrings> options)
        {
            con = options.Value;
        }
        public async Task<List<Pais>> GetPaises()
        {
            List<Pais> lista = new List<Pais>();

            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_GetPaises", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        lista.Add(new Pais()
                        {
                            IdPais = Convert.ToInt32(dr["IdPais"]),
                            Nombre = dr["Nombre"].ToString()!,
                            Abreviatura = dr["Abreviatura"].ToString()!
                        });
                    }
                }
            }
            return lista;
        }
    }
}
