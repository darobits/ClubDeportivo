using Microsoft.AspNetCore.Mvc;
using AppClubDeportivo.Models;
using AppClubDeportivo.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace AppClubDeportivo.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly AppDBContext _context;

        public UsuarioController(AppDBContext context)
        {
            _context = context;
        }

        // Mostrar el formulario de registro (GET)
        [HttpGet]
        public IActionResult Register()
        {
            return View("register"); // Especificar el nombre exacto de la vista
        }

        // Manejar el registro de un nuevo usuario (POST)
        [HttpPost]
        public IActionResult Registro(Usuario nuevoUsuario, string password, string confirmarPassword)
        {
            // Validar que las contraseñas coincidan
            if (password != confirmarPassword)
            {
                ViewBag.Error = "Las contraseñas no coinciden.";
                return View();
            }

            // Verificar si ya existe un usuario con ese DNI
            var usuarioExistente = _context.Usuarios.FirstOrDefault(u => u.DNI == nuevoUsuario.DNI);
            if (usuarioExistente != null)
            {
                ViewBag.Error = "Ya existe un usuario con ese DNI.";
                return View();
            }

            // Establecer la contraseña encriptada
            nuevoUsuario.SetPassword(password);

            // Asignar un rol por defecto de 'Socio'
            nuevoUsuario.Rol = "Socio";

            // Guardar el nuevo usuario en la base de datos
            _context.Usuarios.Add(nuevoUsuario);
            _context.SaveChanges();

            // Redirigir a la página de inicio de sesión después de registrarse
            return RedirectToAction("Login");
        }

        // Acción para iniciar sesión (ya implementada)
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string dni, string password)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.DNI == dni);

            if (usuario != null && usuario.VerifyPassword(password))
            {
                HttpContext.Session.SetString("Usuario", Newtonsoft.Json.JsonConvert.SerializeObject(usuario));
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "DNI o contraseña incorrectos.";
            return View();
        }
    }
}
