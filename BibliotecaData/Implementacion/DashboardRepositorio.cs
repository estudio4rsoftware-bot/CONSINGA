using BibliotecaData.Configuracion;
using BibliotecaData.Contrato;
using BibliotecaEntidades;
using BibliotecaEntidades.DTOs;
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
    public class DashboardRepositorio : IDashboardRepositorio
    {
        private readonly ConnectionStrings con;
        public DashboardRepositorio(IOptions<ConnectionStrings> options)
        {
            con = options.Value;
        }

        public async Task<DashboardDTO> Obtener(string oEmpresa)
        {
            DashboardDTO objeto = null!;

             using (var conexion = new SqlConnection(con.CadenaSQL))
             {
                 await conexion.OpenAsync();
                 SqlCommand cmd = new SqlCommand("sp_obtenerDashboard", conexion);
                cmd.Parameters.AddWithValue("@oEmpresa", oEmpresa);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@oEmpresa", oEmpresa);
                 using (var dr = await cmd.ExecuteReaderAsync())
                 {
                     if (await dr.ReadAsync())
                     {
                         objeto = new DashboardDTO()
                         {
                             TotalControlCampo = Convert.ToInt32(dr["TotalControlCampo"]),
                             TotalAlertasActivas = Convert.ToInt32(dr["TotalAlertasActivas"]),
                             TotalControlesProceso = Convert.ToInt32(dr["TotalControlesProceso"]),
                             TotalAntecedentes = Convert.ToInt32(dr["TotalAntecedentes"])
                         };
                     }
                 }
             }

           

            return objeto;
        }
    }
}
