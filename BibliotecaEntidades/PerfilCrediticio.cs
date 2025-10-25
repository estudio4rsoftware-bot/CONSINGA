namespace BibliotecaEntidades
{
    public class PerfilCrediticio
    {
        public int IdPerfilCrediticio { get; set; }
        public string Periodo { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public int IdTipoDocumento { get; set; }
        public int IdPais { get; set; }
        public string Semaforo { get; set; } = string.Empty;
        public string Observaciones { get; set; } = string.Empty;
        public int IdUsuarioConsigna { get; set; }
        public string Estado { get; set; } = string.Empty;
    }
}
