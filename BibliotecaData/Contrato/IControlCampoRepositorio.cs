using BibliotecaEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaData.Contrato
{
    public interface IControlCampoRepositorio
    {
        Task<List<ControlCampo>> Lista(String oEmpresa);
        Task<ControlCampo> Obtener(int IEmpresa);
        Task<String> Clonar(int IEmpresa);
        Task<string> Guardar(ControlCampo objeto);
        Task<string> Editar(ControlCampo objeto);
        Task<int> Eliminar(int IEmpresa);
    }
}
