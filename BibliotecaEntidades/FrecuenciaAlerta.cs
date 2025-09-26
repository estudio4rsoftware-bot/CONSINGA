using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BibliotecaEntidades
{
    public  class FrecuenciaAlerta
    {
        public int IdFrecuenciaAlerta { get; set; }
        public string Estado { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public string TextoCombo { get; set; } = null!;
        public int DiasValidez { get; set; } = 0;
        public int Valor { get; set; } = 0;
     
    }
}
