namespace BibliotecaEntidades.DTOs
{
    public class PerfilCrediticioDetailsDTO
    {
        public int IdPerfilCrediticio { get; set; }
        public string Numero { get; set; } = string.Empty;
        public int IdTipoDocumento { get; set; }
        public string TipoDescripcion { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Semaforo { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
    }
}
