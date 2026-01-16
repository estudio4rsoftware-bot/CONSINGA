using BibliotecaData.Contrato;
using BibliotecaData.Helpers;
using BibliotecaEntidades;
using BibliotecaEntidades.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BibliotecaWeb.Controllers
{
    [Authorize]
    public class PerfilCrediticioController : Controller
    {
        private readonly IPaisRepositorio _paisRepositorio;
        private readonly ITipoDocumentoRepositorio _tipoDocumentoRepositorio;
        private readonly IPerfilCrediticioRepositorio _perfilCrediticioRepositorio;
        public PerfilCrediticioController(IPaisRepositorio paisRepositorio,
            ITipoDocumentoRepositorio tipoDocumentoRepositorio,
            IPerfilCrediticioRepositorio perfilCrediticioRepositorio)
        {
            _paisRepositorio = paisRepositorio;
            _tipoDocumentoRepositorio = tipoDocumentoRepositorio;
            _perfilCrediticioRepositorio = perfilCrediticioRepositorio;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetPaises()
        {
            List<Pais> lista = await _paisRepositorio.GetPaises();
            return StatusCode(StatusCodes.Status200OK, new { data = lista });
        }

        public async Task<IActionResult> GetTiposDocumento([FromQueryAttribute] int idPais)
        {
            List<TipoDocumento> lista = await _tipoDocumentoRepositorio.GetTiposDocumento(idPais);
            return StatusCode(StatusCodes.Status200OK, new { data = lista });
        }

        [HttpPost]
        public async Task<IActionResult> Guardar([FromBody] PerfilCrediticio perfil)
        {
            ClaimsPrincipal claimuser = HttpContext.User;
            var identitis = claimuser.Identities.ToList();
            var identity = identitis.FirstOrDefault();

            if (identity != null)
            {
                var idUsuarioConsigna = Convert.ToInt32(identity.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                perfil.IdUsuarioConsigna = idUsuarioConsigna;

                string respuesta = await _perfilCrediticioRepositorio.Guardar(perfil);
                return StatusCode(StatusCodes.Status200OK, new { data = respuesta });
            }
            else
                return StatusCode(StatusCodes.Status200OK, "Error al obtener el usuario");
        }

        [HttpGet]
        public async Task<IActionResult> ListPerfilCrediticioBySigningUser()
        {
            ClaimsPrincipal claimuser = HttpContext.User;
            var identitis = claimuser.Identities.ToList();
            var identity = identitis.FirstOrDefault();

            if (identity != null)
            {
                var IdUsuario = Convert.ToInt32(identity.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                List<PerfilCrediticioDetailsDTO> lista = await _perfilCrediticioRepositorio.Lista(IdUsuario);

                return StatusCode(StatusCodes.Status200OK, new { data = lista });
            }
            else
            {
                return StatusCode(StatusCodes.Status200OK, "Error al obtener el usuario");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Clonar([FromQueryAttribute] int idPerfilCrediticio)
        {
            string respuesta = await _perfilCrediticioRepositorio.Clonar(idPerfilCrediticio);
            return StatusCode(StatusCodes.Status200OK, new { data = respuesta });
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar([FromQueryAttribute] int idPerfilCrediticio)
        {
            var resultado = await _perfilCrediticioRepositorio.Eliminar(idPerfilCrediticio);
            return StatusCode(StatusCodes.Status200OK, new { data = resultado });
        }

        [HttpGet]
        public async Task<IActionResult> Obtener([FromQueryAttribute] int idPerfilCrediticio)
        {
            var perfil = await _perfilCrediticioRepositorio.Obtener(idPerfilCrediticio);
            if (perfil != null)
                return StatusCode(StatusCodes.Status200OK, new { data = perfil });
            else
                return StatusCode(StatusCodes.Status404NotFound, new { data = perfil });
        }

        [HttpGet]
        public async Task<IActionResult> ListaCalificacion([FromQuery] int idPerfilCrediticio)
        {
            var perfilCalificacion = await _perfilCrediticioRepositorio.ListaCalificacion(idPerfilCrediticio);

            if (perfilCalificacion == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new { data = perfilCalificacion });
            }
            else
            {
                return StatusCode(StatusCodes.Status200OK, new { data = perfilCalificacion });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Actualizar([FromBody] PerfilCrediticioActualizarDTO perfil)
        {
            var resultado = await _perfilCrediticioRepositorio.Actualizar(perfil);
            return StatusCode(StatusCodes.Status200OK, new { data = resultado });
        }

        [HttpGet]
        public async Task<IActionResult> Descargar([FromQuery] int idPerfilCrediticio)
        {
            var perfilMedia = await _perfilCrediticioRepositorio.DescargarImagen(idPerfilCrediticio);

            if (perfilMedia == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new { message = "Perfil crediticio inválido." });
            }
            return StatusCode(StatusCodes.Status200OK,
                new
                {
                    imagen = perfilMedia.ImageBase64,
                    nombre = CleanData.RemoveSpecialCharacters(perfilMedia.Nombre)
                });
        }
    }
}