using BibliotecaEntidades;

namespace BibliotecaData.Contrato
{
    public interface ITipoDocumentoRepositorio
    {
        Task<List<TipoDocumento>> GetTiposDocumento(int idPais);
    }
}
