using BibliotecaData.Contrato;
using BibliotecaEntidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BibliotecaWeb.Controllers
{
    [Authorize]
    public class EmpresaController : Controller
    {
        private readonly IEmpresaRepositorio _repositorio;

        public EmpresaController(IEmpresaRepositorio repositorio)
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

            ClaimsPrincipal claimuser = HttpContext.User;
            var identitis = claimuser.Identities.ToList();
            var identity = identitis.FirstOrDefault();

            string oEmpresa = "";

            if (identity != null)
            {
                var empresa = identity.FindFirst(ClaimTypes.Country)?.Value;
                oEmpresa = empresa;
                List<Empresa> lista = await _repositorio.Lista(oEmpresa);
                return StatusCode(StatusCodes.Status200OK, new { data = lista });
            }
            else
            {
                return StatusCode(StatusCodes.Status200OK, "Error al obtener el usuario");
            }


          
        }

        [HttpGet]
        public async Task<IActionResult> Obtener(int IdEmpresa)
        {
            Empresa objeto = await _repositorio.Obtener(IdEmpresa);
            if (objeto != null)
                return StatusCode(StatusCodes.Status200OK, new { data = objeto });
            else
                return StatusCode(StatusCodes.Status404NotFound, new { data = objeto });
        }

        [HttpPost]
        public async Task<IActionResult> Guardar([FromBody] Empresa objeto)
        {
            string respuesta = await _repositorio.Guardar(objeto);
            return StatusCode(StatusCodes.Status200OK, new { data = respuesta });
        }


        [HttpPut]
        public async Task<IActionResult> Editar([FromBody] Empresa objeto)
        {
            string respuesta = await _repositorio.Editar(objeto);
            return StatusCode(StatusCodes.Status200OK, new { data = respuesta });
        }

        [HttpDelete]
        public async Task<IActionResult> Eliminar(int IdEmpresa)
        {
            int respuesta = await _repositorio.Eliminar(IdEmpresa);
            return StatusCode(StatusCodes.Status200OK, new { data = respuesta });
        }

    }
}
