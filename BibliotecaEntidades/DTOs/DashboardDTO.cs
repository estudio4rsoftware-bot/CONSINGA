using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaEntidades.DTOs
{
    public class DashboardDTO
    {
        public int TotalControlCampo { get; set; }
        public int TotalAlertasActivas { get; set; }
        public int TotalControlesProceso { get; set; }
        public int TotalAntecedentes { get; set; }
    }
}
