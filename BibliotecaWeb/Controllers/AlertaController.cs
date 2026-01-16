using BibliotecaData.Contrato;
using BibliotecaEntidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Principal;

namespace BibliotecaWeb.Controllers
{
    [Authorize]
    public class AlertaController : Controller
    {
        private readonly IAlertaRepositorio _repositorio;

        public AlertaController(IAlertaRepositorio repositorio)
        {
            _repositorio = repositorio;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult IndexAgrupadas()
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
                List<Alerta> lista = await _repositorio.Lista(oEmpresa);
                return StatusCode(StatusCodes.Status200OK, new { data = lista });
            }
            else
            {
                return StatusCode(StatusCodes.Status200OK, "Error al obtener el usuario");
            }

        }
        [HttpPost]
        public async Task<IActionResult> Guardar([FromBody] Alerta objeto)
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
            else
                return StatusCode(StatusCodes.Status200OK, "Error al obtener el usuario");


            //ClaimsPrincipal claimuser = HttpContext.User;
            //var identitis = claimuser.Identities.ToList();
            //var identity = identitis.FirstOrDefault();

            //if (identity != null)
            //{
            //    var id = identity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //    var empresa = identity.FindFirst(ClaimTypes.Country)?.Value;
            //    objeto.oUsuario = id;

            //    string respuesta = await _repositorio.Guardar(objeto);
            //    return StatusCode(StatusCodes.Status200OK, new { data = respuesta });
            //}
        }
        public async Task<IActionResult> Obtener(int idAlerta)
        {
            Alerta alerta = await _repositorio.Obtener(idAlerta);
            return StatusCode(StatusCodes.Status200OK, new { data = alerta });
        }
        public async Task<IActionResult> Anular(int idAlerta)
        {
            int respuesta = await _repositorio.Anular(idAlerta);
            return StatusCode(StatusCodes.Status200OK, new { data = respuesta });
        }


        public async Task<IActionResult> ListaAgrupadaEtiqueta()
        {
            ClaimsPrincipal claimuser = HttpContext.User;
            var identitis = claimuser.Identities.ToList();
            var identity = identitis.FirstOrDefault();

            string oEmpresa = "";

            if (identity != null)
            {
                var empresa = identity.FindFirst(ClaimTypes.Country)?.Value;
                oEmpresa = empresa;
                List<AlertaAgrupadaEtiqueta> lista = await _repositorio.ListaAgrupadaEtiqueta(oEmpresa);
                return StatusCode(StatusCodes.Status200OK, new { data = lista });
            }
            else
            {
                return StatusCode(StatusCodes.Status200OK, "Error al obtener el usuario");
            }

        }

        [HttpPost]
        public async Task<IActionResult> ActivarAgrupadas(string etiqueta)
        {
            ClaimsPrincipal claimuser = HttpContext.User;
            var identitis = claimuser.Identities.ToList();
            var identity = identitis.FirstOrDefault();

            string oEmpresa = "";

            if (identity != null)
            {
                var empresa = identity.FindFirst(ClaimTypes.Country)?.Value;
                oEmpresa = empresa;
                int respuesta = await _repositorio.ActivarAgrupadas(etiqueta,oEmpresa);
                return StatusCode(StatusCodes.Status200OK, new { data = respuesta });
            }
            else
            {
                return StatusCode(StatusCodes.Status200OK, "Error al obtener el usuario");
            }


          
        }
        [HttpPost]
        public async Task<IActionResult> DesactivarAgrupadas(string etiqueta)
        {
            ClaimsPrincipal claimuser = HttpContext.User;
            var identitis = claimuser.Identities.ToList();
            var identity = identitis.FirstOrDefault();

            string oEmpresa = "";

            if (identity != null)
            {
                var empresa = identity.FindFirst(ClaimTypes.Country)?.Value;
                oEmpresa = empresa;
                int respuesta = await _repositorio.DesactivarAgrupadas(etiqueta,oEmpresa);
                return StatusCode(StatusCodes.Status200OK, new { data = respuesta });
            }
            else
            {
                return StatusCode(StatusCodes.Status200OK, "Error al obtener el usuario");
            }

        }


    }
}
