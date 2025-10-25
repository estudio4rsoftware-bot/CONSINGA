using BibliotecaData.Configuracion;
using BibliotecaData.Contrato;
using BibliotecaEntidades;
using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;

namespace BibliotecaData.Implementacion
{
    public class TipoDocumentoRepositorio : ITipoDocumentoRepositorio
    {
        private readonly ConnectionStrings con;
        public TipoDocumentoRepositorio(IOptions<ConnectionStrings> options)
        {
            con = options.Value;
        }
        public async Task<List<TipoDocumento>> GetTiposDocumento(int idPais)
        {
            List<TipoDocumento> listaTipoDocumento = new List<TipoDocumento>();

            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_GetTiposDocumento", conexion);
                cmd.Parameters.AddWithValue("@IdPais", idPais);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        listaTipoDocumento.Add(new TipoDocumento()
                        {
                            IdTipo = Convert.ToInt32(dr["IdTipo"]),
                            Pais = new Pais
                            {
                                IdPais = Convert.ToInt32(dr["IdPais"].ToString())
                            },
                            Descripcion = dr["Descripcion"].ToString()!,
                            IdentificadorTipo = dr["IdentificadorTipo"].ToString()!
                        });
                    }
                }
            }
            return listaTipoDocumento;
        }
    }
}
