using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaEntidades
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string NombreCompleto { get; set; } = null!;
        public string NombreUsuario { get; set; } = null!;
        public string Clave { get; set; } = null!;
        public string FechaCreacion { get; set; } = null!;

        public string MailDeContacto { get; set; } = null!;

        public string UsuarioSnig { get; set; } = null!;

        public string ClaveSnig { get; set; } = null!;

        public string Activo { get; set; } = null!;
        public string Rol { get; set; } = null!;

        public Empresa oEmpresa { get; set; } = null!;
    }
}
