using BibliotecaData.Contrato;
using BibliotecaEntidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaWeb.Controllers
{
    [Authorize]
    public class FrecuenciaAlertaController : Controller
    {
        private readonly IFrecuenciaAlertaRepositorio _repositorio;

        public FrecuenciaAlertaController(IFrecuenciaAlertaRepositorio repositorio)
        {
            _repositorio = repositorio;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Lista()
        {
            List<FrecuenciaAlerta> lista = await _repositorio.Lista();
            return StatusCode(StatusCodes.Status200OK, new { data = lista });
        }
    }
}
