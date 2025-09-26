using BibliotecaEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
 
namespace BibliotecaData.Contrato
{
    public interface IEmpresaRepositorio
    {
        Task<List<Empresa>> Lista(String oEmpresa);
        Task<Empresa> Obtener(int IEmpresa);
        Task<string> Guardar(Empresa objeto);
        Task<string> Editar(Empresa objeto);
        Task<int> Eliminar(int IEmpresa);
    }
}
