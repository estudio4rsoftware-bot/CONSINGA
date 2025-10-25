namespace BibliotecaEntidades
{
    public class TipoDocumento
    {
        public int IdTipo { get; set; }
        public Pais Pais { get; set; } = null!;
        public string Descripcion { get; set; } = string.Empty;
        public string IdentificadorTipo { get; set; } = string.Empty;
    }
}
