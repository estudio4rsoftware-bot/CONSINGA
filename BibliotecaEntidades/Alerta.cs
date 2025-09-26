using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaEntidades
{
    public class Alerta
    {

        public int IdAlerta { get; set; }
        public string NroCaravana { get; set; } = null!;
        public string Propiedad { get; set; } = null!;
        public string Tenencia { get; set; } = null!;
        public string LugarFisico { get; set; } = null!;
        public string Seguimiento { get; set; } = null!;
        public string FechaControl { get; set; } = null!;
        public string Cambio { get; set; } = null!;
        public string PropiedadAnterior { get; set; } = null!;
        public string TenenciaAnterior { get; set; } = null!;
        public string LugarFisicoAnterior { get; set; } = null!;
        public string FechaIngreso { get; set; } = null!;
        public string FechaFin { get; set; } = null!;
        public string Estado { get; set; } = null!;
        public string FechaUltimoControl { get; set; } = null!;
        public string oFrecuenciaAlerta { get; set; } = null!;

        public string Etiqueta { get; set; } = null!;

        public string oUsuario { get; set; } = null!;

        public string oEmpresa { get; set; } = null!;


    }

}
