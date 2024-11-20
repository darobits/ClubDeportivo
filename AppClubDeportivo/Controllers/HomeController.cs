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
            // Obtener la información del usuario desde la sesión
            var usuarioJson = HttpContext.Session.GetString("Usuario");

            // Si no existe la sesión, redirigir al login
            if (string.IsNullOrEmpty(usuarioJson))
            {
                return RedirectToAction("Login", "Usuario");
            }

            Usuario usuario;
            try
            {
                // Intentar deserializar la información del usuario
                usuario = JsonConvert.DeserializeObject<Usuario>(usuarioJson);
            }
            catch (Exception)
            {
                // Si ocurre un error en la deserialización, redirigir al login
                return RedirectToAction("Login", "Usuario");
            }

            // Guardar el nombre y rol del usuario para mostrarlo en la vista
            ViewBag.UsuarioNombre = $"{usuario.Nombre} {usuario.Apellido}";
            ViewBag.RolUsuario = usuario.Rol;

            // Verificación de roles (opcional, si se necesitan restricciones por rol)
            if (usuario.Rol == "Admin")
            {
                // Si es Admin, redirigir al Dashboard de administración
                return RedirectToAction("Dashboard", "Admin");
            }
            else if (usuario.Rol == "Socio")
            {
                // Si es Socio, permitir el acceso a la vista principal del socio
                ViewBag.EsAdmin = false; // Usado para personalizar la vista
            }

            // Mostrar mensaje de éxito de registro, si existe
            if (TempData["RegistroExitoso"] != null)
            {
                ViewBag.MensajeExito = TempData["RegistroExitoso"];
            }

            return View();
        }
    }
}
