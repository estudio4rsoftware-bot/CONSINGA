using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaEntidades
{
    public class ControlCampo
    {
        public int IdControlCampo { get; set; }
        public string Estado { get; set; } = null!;
        public string Archivo { get; set; } = null!;
        public string oUsuario { get; set; } = null!;
        public string oEmpresa { get; set; } = null!;
        public string ResultadoBase64 { get; set; } = null!;
        public string FechaCreacion { get; set; } = null!;

        public string ConAlerta { get; set; } = null!;
        public string ArchivoResultado  { get; set; } = null!;
        public string Observaciones { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public int FrecuenciaAlerta { get; set; } = 0!;


    }
}
