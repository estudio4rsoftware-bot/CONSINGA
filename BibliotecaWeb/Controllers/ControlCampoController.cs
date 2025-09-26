using BibliotecaData.Contrato;
using BibliotecaEntidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;
using System.Security.Claims;

namespace BibliotecaWeb.Controllers
{
    [Authorize]
    public class ControlCampoController : Controller
    {
        private readonly IControlCampoRepositorio _repositorio;
        public ControlCampoController(IControlCampoRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        // GET: ControlCampoController
        public ActionResult Index()
        {
            return View();
        }
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
                List<ControlCampo> lista = await _repositorio.Lista(oEmpresa);
                return StatusCode(StatusCodes.Status200OK, new { data = lista });
            }
            else {
                return StatusCode(StatusCodes.Status200OK, "Error al obtener el usuario");
            }
                
        }

        [HttpGet]
        public async Task<IActionResult> Obtener(int IdControlCampo)
        {
            ControlCampo objeto = await _repositorio.Obtener(IdControlCampo);
            if (objeto != null)
                return StatusCode(StatusCodes.Status200OK, new { data = objeto });
            else
                return StatusCode(StatusCodes.Status404NotFound, new { data = objeto });
        }

        [HttpPost]
        public async Task<IActionResult> Guardar([FromBody] ControlCampo objeto)
        {
            ClaimsPrincipal claimuser = HttpContext.User;
            var identitis = claimuser.Identities.ToList();
            var identity = identitis.FirstOrDefault();
            if (identity != null)
            {
                var id = identity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var empresa = identity.FindFirst(ClaimTypes.Country)?.Value;
                objeto.oUsuario = id;
                objeto.oEmpresa = empresa;
                string respuesta = await _repositorio.Guardar(objeto);
                return StatusCode(StatusCodes.Status200OK, new { data = respuesta });
            }
            else {
                return StatusCode(StatusCodes.Status200OK, "Error al obtener el usuario");
            }

            //    string respuesta = await _repositorio.Guardar(objeto);
          //  return StatusCode(StatusCodes.Status200OK, new { data = respuesta });
        }

        [HttpPost]
        public async Task<IActionResult> Clonar(int IdControlCampo)
        {
            string respuesta = await _repositorio.Clonar(IdControlCampo);
            return StatusCode(StatusCodes.Status200OK, new { data = respuesta });
            
            //    string respuesta = await _repositorio.Guardar(objeto);
            //  return StatusCode(StatusCodes.Status200OK, new { data = respuesta });
        }


        [HttpPut]
        public async Task<IActionResult> Editar([FromBody] ControlCampo objeto)
        {
            string respuesta = await _repositorio.Editar(objeto);
            return StatusCode(StatusCodes.Status200OK, new { data = respuesta });
        }

        [HttpDelete]
        public async Task<IActionResult> Eliminar(int IdControlCampo)
        {
            int respuesta = await _repositorio.Eliminar(IdControlCampo);
            return StatusCode(StatusCodes.Status200OK, new { data = respuesta });
        }
    }
}
