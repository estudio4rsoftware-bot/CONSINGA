using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaEntidades
{
    public class Antecedente
    {
        public int IdAntecedente { get; set; }
        public string Estado { get; set; } = null!;
        public string FechaIngreso { get; set; } = null!;
        public string Documento { get; set; } = null!;
        public string UsuarioId { get; set; } = null!;
        public string EmpresaId { get; set; } = null!;
        public string ResultadoBCU { get; set; } = null!;
        public string ResultadoDGI { get; set; } = null!;
        public string ResultadoClearing { get; set; } = null!;
        public string ResultadoOtro { get; set; } = null!;

        public Antecedente() { }
        
    }
}
