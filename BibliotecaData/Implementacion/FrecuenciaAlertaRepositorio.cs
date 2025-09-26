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
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Drawing;
using System.Security.Principal;

namespace BibliotecaData.Implementacion
{
    public class FrecuenciaAlertaRepositorio: IFrecuenciaAlertaRepositorio
    {
        private readonly ConnectionStrings con;
        public FrecuenciaAlertaRepositorio(IOptions<ConnectionStrings> options)
        {
            con = options.Value;
        }

        public Task<int> Anular(int IdFrecuenciaAlerta)
        {
            throw new NotImplementedException();
        }

        public Task<string> Editar(FrecuenciaAlerta objeto)
        {
            throw new NotImplementedException();
        }

        public Task<int> Eliminar(int IdFrecuenciaAlerta)
        {
            throw new NotImplementedException();
        }

        public Task<string> Guardar(FrecuenciaAlerta objeto)
        {
            throw new NotImplementedException();
        }

        public async Task<List<FrecuenciaAlerta>> Lista()
        {

            List<FrecuenciaAlerta> lista = new List<FrecuenciaAlerta>();

            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_listaFrecuenciaAlerta", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        lista.Add(new FrecuenciaAlerta()
                        {
                            IdFrecuenciaAlerta = Convert.ToInt32(dr["IdFrecuenciaAlerta"]),
                            Estado = dr["Estado"].ToString()!,
                            Nombre = dr["Nombre"].ToString()!,
                            TextoCombo = dr["TextoCombo"].ToString()!,
                            DiasValidez = Convert.ToInt32(dr["DiasValidez"]),
                            Valor = Convert.ToInt32(dr["Valor"])

                        });
                    }

                }
            }
            return lista;

        }

        public Task<FrecuenciaAlerta> Obtener(int IdFrecuenciaAlerta)
        {
            throw new NotImplementedException();
        }
    }
}
