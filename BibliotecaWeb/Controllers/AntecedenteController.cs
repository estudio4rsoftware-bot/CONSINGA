using BibliotecaData.Contrato;
using BibliotecaEntidades;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Authorization;

namespace BibliotecaWeb.Controllers
{
    [Authorize]
    public class AntecedenteController : Controller
    {
        private readonly IAntecedenteRepositorio _repositorio;
        public AntecedenteController(IAntecedenteRepositorio repositorio)
        {
            _repositorio = repositorio;
        }
        public IActionResult Index()
        {
            return View();
        }
         [HttpGet]
        public async Task<IActionResult> Lista()
        {
            List<Antecedente> listaAntecedentes = await _repositorio.Lista();
            return StatusCode(StatusCodes.Status200OK, new { data = listaAntecedentes });
        }


        [HttpGet]
        public async Task<IActionResult> Obtener(int IdAntecedente)
        {
        Antecedente objeto = await _repositorio.Obtener(IdAntecedente);
            if (objeto != null)
                return StatusCode(StatusCodes.Status200OK, new { data = objeto });
            else
                return StatusCode(StatusCodes.Status404NotFound, new { data = objeto });
        }


        [HttpPost]
        public async Task<IActionResult> Guardar([FromBody] Antecedente objeto)
        {
            string respuesta = await _repositorio.Guardar(objeto);
            return StatusCode(StatusCodes.Status200OK, new { data = respuesta });
        }
    }

}
