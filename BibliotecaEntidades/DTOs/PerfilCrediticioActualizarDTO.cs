namespace BibliotecaEntidades.DTOs
{
    public class PerfilCrediticioActualizarDTO
    {
        public int IdPerfilCrediticio { get; set; }
        public string Semaforo { get; set; } = string.Empty;
        public string Observaciones { get; set; } = string.Empty;
    }
}
