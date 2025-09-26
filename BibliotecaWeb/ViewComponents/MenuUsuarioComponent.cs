using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BibliotecaWeb.ViewComponents
{
    public class MenuUsuarioViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {

            ClaimsPrincipal claimuser = HttpContext.User;
            string nombreUsuario = "";
            string rolUsuario = "";
            string empresaUsuario = "";


            if (claimuser.Identity!.IsAuthenticated)
            {
                nombreUsuario = claimuser.Claims.Where(c => c.Type == ClaimTypes.Name)!.Select(c => c.Value)!.SingleOrDefault()!;
                rolUsuario = claimuser.Claims.Where(c => c.Type == ClaimTypes.Role)!.Select(c => c.Value)!.SingleOrDefault()!;
                empresaUsuario = claimuser.Claims.Where(c => c.Type == ClaimTypes.Locality)!.Select(c => c.Value)!.SingleOrDefault()!;

            }

            ViewData["nombreUsuario"] = nombreUsuario;
            ViewData["rolUsuario"] = rolUsuario;
            ViewData["empresaUsuario"] = empresaUsuario;



            return View();
        }

    }
}
