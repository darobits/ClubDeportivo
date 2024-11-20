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
            return View("register");
        }

        // Registro de un nuevo usuario (POST)
        [HttpPost]
        public IActionResult Registro(Usuario nuevoUsuario, string password, string confirmarPassword)
        {
            // Validar que el modelo sea válido
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Datos inválidos. Por favor, verifica los campos.";
                return View("register");
            }

            // Validar que las contraseñas coincidan
            if (password != confirmarPassword)
            {
                ViewBag.Error = "Las contraseñas no coinciden.";
                return View("register");
            }

            // Verificar si ya existe un usuario con el mismo DNI o correo
            var usuarioExistente = _context.Usuarios
                .FirstOrDefault(u => u.DNI == nuevoUsuario.DNI || u.Correo == nuevoUsuario.Correo);
            if (usuarioExistente != null)
            {
                ViewBag.Error = "El usuario con ese DNI o correo ya está registrado.";
                return View("register");
            }

            // Establecer la contraseña encriptada
            try
            {
                nuevoUsuario.SetPassword(password);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Hubo un problema al establecer la contraseña. Intenta nuevamente.";
                return View("register");
            }

            // Asignar rol por defecto 'Socio'
            nuevoUsuario.Rol = "Socio";

            // Agregar el usuario a la base de datos
            try
            {
                _context.Usuarios.Add(nuevoUsuario);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                // En caso de error al guardar el usuario
                ViewBag.Error = "Hubo un error al registrar el usuario. Intenta nuevamente.";
                return View("register");
            }

            // Mensaje de éxito de registro
            TempData["RegistroExitoso"] = "Tu cuenta se ha creado con éxito. Ahora puedes iniciar sesión.";
            return RedirectToAction("Login");
        }

        // Mostrar el formulario de inicio de sesión (GET)
        [HttpGet]
        public IActionResult Login()
        {
            return View("login");
        }

        // Inicio de sesión (POST)
        [HttpPost]
        public IActionResult Login(string dni, string password)
        {
            // Validar si los campos están vacíos
            if (string.IsNullOrEmpty(dni) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Por favor, completa todos los campos.";
                return View("login");
            }

            // Buscar el usuario por DNI
            var usuario = _context.Usuarios
                .FirstOrDefault(u => u.DNI == dni);

            // Verificar si el usuario existe y si la contraseña es correcta
            if (usuario == null || !usuario.VerifyPassword(password))
            {
                ViewBag.Error = "DNI o contraseña incorrectos.";
                return View("login");
            }

            // Guardar la información del usuario en la sesión
            HttpContext.Session.SetString("Usuario", Newtonsoft.Json.JsonConvert.SerializeObject(usuario));

            // Redirigir al home o a la página que desees después de login
            return RedirectToAction("Index", "Home");
        }

        // Cerrar sesión (POST)
        [HttpPost]
        public IActionResult Logout()
        {
            // Limpiar la sesión del usuario
            HttpContext.Session.Clear();

            // Redirigir a la página de inicio de sesión
            return RedirectToAction("Login", "Usuario");
        }
    }
}
