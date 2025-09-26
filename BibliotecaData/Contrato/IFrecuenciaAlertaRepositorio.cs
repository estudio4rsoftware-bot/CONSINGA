using BibliotecaEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaData.Contrato
{
    public  interface IFrecuenciaAlertaRepositorio
    {
        Task<List<FrecuenciaAlerta>> Lista();
        Task<FrecuenciaAlerta> Obtener(int IdFrecuenciaAlerta);
        Task<string> Guardar(FrecuenciaAlerta objeto);
        Task<string> Editar(FrecuenciaAlerta objeto);
        Task<int> Eliminar(int IdFrecuenciaAlerta);
        Task<int> Anular(int IdFrecuenciaAlerta);
    }
}
