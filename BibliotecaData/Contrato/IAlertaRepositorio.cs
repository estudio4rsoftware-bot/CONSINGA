using BibliotecaEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaData.Contrato
{
    public interface IAlertaRepositorio
    {
        Task<List<Alerta>> Lista(String oEmpresa);
        Task<List<AlertaAgrupadaEtiqueta>> ListaAgrupadaEtiqueta(String oEmpresa);
        Task<Alerta> Obtener(int IdAlerta);
        Task<string> Guardar(Alerta objeto);
        Task<string> Editar(Alerta objeto);
        Task<int> Eliminar(int IdAlerta);
        Task<int> Anular(int IdAlerta);
        Task<int> DesactivarAgrupadas(string etiqueta, string oEmpresa);
        Task<int> ActivarAgrupadas(string etiqueta, string oEmpresa);
    }
}
