using Microsoft.AspNetCore.Mvc;
using AppClubDeportivo.Models;
using AppClubDeportivo.Data;
using Microsoft.AspNetCore.Http;
using System.Linq;

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
        // Mostrar el formulario de registro (GET)
        [HttpGet]
        public IActionResult Register()
        {
            return View("Register");
        }

        // Registro de un nuevo usuario (POST)
        [HttpPost]
        public IActionResult Registro(Usuario nuevoUsuario, string Password, string ConfirmarPassword)
        {
            // Validar que las contraseñas coincidan
            if (Password != ConfirmarPassword)
            {
                ViewBag.Error = "Las contraseñas no coinciden.";
                return View("Register");
            }

            // Verificar si ya existe un usuario con el mismo DNI o correo
            var usuarioExistente = _context.Usuarios
                .FirstOrDefault(u => u.DNI == nuevoUsuario.DNI || u.Correo == nuevoUsuario.Correo);

            if (usuarioExistente != null)
            {
                ViewBag.Error = "El usuario con ese DNI o correo ya está registrado.";
                return View("Register");
            }

            // Establecer la contraseña y rol por defecto
            nuevoUsuario.SetPassword(Password);
            nuevoUsuario.Rol = "Socio";

            // Guardar el usuario en la base de datos
            try
            {
                _context.Usuarios.Add(nuevoUsuario);
                _context.SaveChanges();
            }
            catch
            {
                ViewBag.Error = "Hubo un error al registrar el usuario. Intenta nuevamente.";
                return View("Register");
            }

            // Redirigir a la página de login con mensaje de éxito
            TempData["RegistroExitoso"] = "Tu cuenta ha sido creada con éxito. Ahora puedes iniciar sesión.";
            return RedirectToAction("Login");
        }



        // Mostrar el formulario de inicio de sesión (GET)
        [HttpGet]
        public IActionResult Login()
        {
            return View("Login");
        }

        // Inicio de sesión (POST)
        [HttpPost]
        public IActionResult Login(string dni, string password)
        {
            // Validar si los campos están vacíos
            if (string.IsNullOrEmpty(dni) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Por favor, completa todos los campos.";
                return View("Login");
            }

            // Buscar el usuario por DNI
            var usuario = _context.Usuarios
                .FirstOrDefault(u => u.DNI == dni);

            // Verificar si el usuario existe y si la contraseña es correcta
            if (usuario == null || !usuario.VerifyPassword(password))
            {
                ViewBag.Error = "DNI o contraseña incorrectos.";
                return View("Login");
            }

            // Guardar la información del usuario en la sesión
            HttpContext.Session.SetString("Usuario", Newtonsoft.Json.JsonConvert.SerializeObject(usuario));

            // Redirigir dependiendo del rol
            if (usuario.Rol == "Administrador")
            {
                return RedirectToAction("Dashboard", "Admin"); // Si tienes una vista específica para admins
            }

            return RedirectToAction("Index", "Home");
        }

        // Cerrar sesión (POST)
        [HttpPost]
        public IActionResult Logout()
        {
            // Eliminar el usuario de la sesión
            HttpContext.Session.Remove("Usuario");

            // Redirigir al Login
            return RedirectToAction("Login", "Usuario");
        }
    }
}
