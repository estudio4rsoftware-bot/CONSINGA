using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BibliotecaEntidades
{
    public class Empresa
    {
        public int IdEmpresa { get; set; }
        public string RUT { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public string Direccion { get; set; } = null!;
        public string Contacto { get; set; } = null!;
        public string Mail { get; set; } = null!;
        public string UsuarioSnig { get; set; } = null!;
        public string ClaveSnig { get; set; } = null!;

        //public string TipoContrato { get; set; } = null!;


    }
}
