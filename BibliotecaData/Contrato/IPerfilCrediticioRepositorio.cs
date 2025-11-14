using BibliotecaEntidades;
using BibliotecaEntidades.DTOs;

namespace BibliotecaData.Contrato
{
    public interface IPerfilCrediticioRepositorio
    {
        Task<string> Guardar(PerfilCrediticio perfil);
        Task<int> Actualizar(PerfilCrediticioActualizarDTO perfil); 
        Task<List<PerfilCrediticioDetailsDTO>> Lista(int idUsuario);
        Task<string> Clonar(int idPerfil);
        Task<int> Eliminar(int idPerfil);
        Task<PerfilCrediticioClonarDTO> Obtener(int idPerfil);
        Task<PerfilCrediticioCalificacionDTO> ListaCalificacion(int idPerfil);
        Task<PerfilCrediticioMediaDTO> DescargarImagen(int idPerfil);
    }
}
