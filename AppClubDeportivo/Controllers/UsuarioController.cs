using Microsoft.AspNetCore.Mvc;
using AppClubDeportivo.Models;
using AppClubDeportivo.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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
        [HttpGet]
        public IActionResult Register()
        {
            return View("register"); // Especificar el nombre exacto de la vista correctamente
        }

        // Manejar el registro de un nuevo usuario (POST)
        [HttpPost]
        public IActionResult Registro(Usuario nuevoUsuario, string password, string confirmarPassword)
        {
            // Validar que las contraseñas coincidan
            if (password != confirmarPassword)
            {
                ViewBag.Error = "Las contraseñas no coinciden.";
                return View("register"); // Retornar a la vista de registro
            }

            // Verificar si ya existe un usuario con ese DNI
            var usuarioExistente = _context.Usuarios.FirstOrDefault(u => u.DNI == nuevoUsuario.DNI);
            if (usuarioExistente != null)
            {
                ViewBag.Error = "Ya existe un usuario con ese DNI.";
                return View("register");
            }

            // Verificar si ya existe un usuario con el correo (opcional, pero recomendado)
            if (_context.Usuarios.Any(u => u.Correo == nuevoUsuario.Correo))
            {
                ViewBag.Error = "El correo ya está registrado.";
                return View("register");
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

        // Mostrar el formulario de inicio de sesión (GET)
        [HttpGet]
        public IActionResult Login()
        {
            return View("login"); // Asegúrate de que esta vista exista y esté correctamente nombrada
        }

        // Manejar la acción de inicio de sesión (POST)
        [HttpPost]
        public IActionResult Login(string dni, string password)
        {
            // Buscar al usuario por DNI
            var usuario = _context.Usuarios.FirstOrDefault(u => u.DNI == dni);

            // Verificar si el usuario existe y si la contraseña es correcta
            if (usuario != null && usuario.VerifyPassword(password))
            {
                // Guardar la información del usuario en la sesión
                HttpContext.Session.SetString("Usuario", Newtonsoft.Json.JsonConvert.SerializeObject(usuario));

                // Redirigir a la página principal después del inicio de sesión exitoso
                return RedirectToAction("Index", "Home");
            }

            // Si las credenciales son incorrectas, mostrar un mensaje de error
            ViewBag.Error = "DNI o contraseña incorrectos.";
            return View("login");
        }

        // Acción para cerrar sesión
        [HttpPost]
        public IActionResult Logout()
        {
            // Limpiar la sesión del usuario
            HttpContext.Session.Clear();

            // Redirigir al formulario de inicio de sesión
            return RedirectToAction("Login");
        }
    }
}
