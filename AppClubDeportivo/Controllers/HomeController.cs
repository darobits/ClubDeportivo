using AppClubDeportivo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AppClubDeportivo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // Comprobar si el usuario está en sesión
            var usuarioJson = HttpContext.Session.GetString("Usuario");
            if (usuarioJson == null)
            {
                return RedirectToAction("Login", "Usuario");
            }

            // Cargar datos del usuario
            var usuario = JsonConvert.DeserializeObject<Usuario>(usuarioJson);
            ViewBag.UsuarioNombre = $"{usuario.Nombre} {usuario.Apellido}";
            ViewBag.RolUsuario = usuario.Rol;

            return View();
        }
    }
}
