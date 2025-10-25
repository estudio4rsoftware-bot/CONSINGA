namespace BibliotecaEntidades.DTOs
{
    public class PerfilCrediticioClonarDTO
    {
        public int IdPerfilCrediticio { get; set; }
        public int IdPais { get; set; }
        public int IdTipoDocumento { get; set; }
        public string Numero { get; set; } = string.Empty;
    }
}
