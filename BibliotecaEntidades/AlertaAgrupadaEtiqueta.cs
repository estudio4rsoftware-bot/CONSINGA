using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaEntidades
{
    public class AlertaAgrupadaEtiqueta
    {
      
        public string Etiqueta { get; set; } = null!;
        public int CantidadAlertas { get; set; }
        public string PrimerControl { get; set; } = null!;
        public string UltimoControl { get; set; } = null!;
        public string Estado { get; set; } = null!;

        public string oEmpresa { get; set; } = null!;

    }
}
