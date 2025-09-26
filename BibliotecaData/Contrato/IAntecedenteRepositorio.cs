using BibliotecaEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaData.Contrato
{
    public interface IAntecedenteRepositorio
    {
        Task<List<Antecedente>> Lista();
        Task<Antecedente> Obtener(int IAntecedente);
        Task<string> Guardar(Antecedente objeto);
        Task<string> Editar(Antecedente objeto);
        Task<int> Eliminar(int IAntecedente);

    }
}
