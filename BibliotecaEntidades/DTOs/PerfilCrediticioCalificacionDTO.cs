using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaEntidades.DTOs
{
    public class PerfilCrediticioCalificacionDTO
    {
        public int IdPerfilCrediticio { get; set; }
        public string Nombre { get; set; } = String.Empty;
        public string Semaforo { get; set; } = String.Empty;
        public string Observaciones { get; set; } = String.Empty;
        public List<CalificacionInstitucion> Calificaciones { get; set; } = new List<CalificacionInstitucion>();
    }
}