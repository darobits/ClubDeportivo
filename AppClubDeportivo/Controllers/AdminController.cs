using AppClubDeportivo.Data;
using AppClubDeportivo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq;

namespace AppClubDeportivo.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly AppDBContext _context;

        public AdminController(ILogger<AdminController> logger, AppDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        // Acción para mostrar el Dashboard de administración
        public IActionResult Dashboard()
        {
            if (!ValidarSesionActiva(out Usuario usuario))
            {
                return RedirectToAction("Login", "Usuario"); // Redirigir al login si no hay sesión activa
            }

            // Obtener todos los usuarios, actividades y eventos
            var usuarios = _context.Usuarios.ToList();
            var actividades = _context.Actividades.ToList();
            var eventos = _context.Eventos.ToList();

            // Pasar los datos a la vista
            ViewBag.Usuarios = usuarios;
            ViewBag.Actividades = actividades;
            ViewBag.Eventos = eventos;

            return View();
        }

        // Método para eliminar un usuario
        [HttpPost]
        public IActionResult EliminarUsuario(int id)
        {
            var usuario = _context.Usuarios.Find(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
                _context.SaveChanges();
            }

            return RedirectToAction("Dashboard");
        }

        // Método para editar un usuario (puedes agregar la lógica de edición)
        [HttpGet]
        public IActionResult EditarUsuario(int id)
        {
            var usuario = _context.Usuarios.Find(id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // Método para guardar los cambios del usuario editado
        [HttpPost]
        public IActionResult EditarUsuario(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                _context.Usuarios.Update(usuario);
                _context.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            return View(usuario);
        }

        // Métodos para eliminar y editar actividades y eventos (de manera similar a los de Usuario)
        [HttpPost]
        public IActionResult EliminarActividad(int id)
        {
            var actividad = _context.Actividades.Find(id);
            if (actividad != null)
            {
                _context.Actividades.Remove(actividad);
                _context.SaveChanges();
            }

            return RedirectToAction("Dashboard");
        }

        [HttpGet]
        public IActionResult EditarActividad(int id)
        {
            var actividad = _context.Actividades.Find(id);
            if (actividad == null)
            {
                return NotFound();
            }

            return View(actividad);
        }

        [HttpPost]
        public IActionResult EditarActividad(Actividad actividad)
        {
            if (ModelState.IsValid)
            {
                _context.Actividades.Update(actividad);
                _context.SaveChanges();
                return RedirectToAction("Dashboard");
            }

            return View(actividad);
        }

        [HttpPost]
        public IActionResult EliminarEvento(int id)
        {
            var evento = _context.Eventos.Find(id);
            if (evento != null)
            {
                _context.Eventos.Remove(evento);
                _context.SaveChanges();
            }

            return RedirectToAction("Dashboard");
        }

        [HttpGet]
        public IActionResult EditarEvento(int id)
        {
            var evento = _context.Eventos.Find(id);
            if (evento == null)
            {
                return NotFound();
            }

            return View(evento);
        }

        [HttpPost]
        public IActionResult EditarEvento(Evento evento)
        {
            if (ModelState.IsValid)
            {
                _context.Eventos.Update(evento);
                _context.SaveChanges();
                return RedirectToAction("Dashboard");
            }

            return View(evento);
        }

        // Método privado para validar si la sesión está activa
        private bool ValidarSesionActiva(out Usuario usuario)
        {
            usuario = null;

            var usuarioJson = HttpContext.Session.GetString("Usuario");
            if (string.IsNullOrEmpty(usuarioJson))
            {
                return false;
            }

            try
            {
                usuario = JsonConvert.DeserializeObject<Usuario>(usuarioJson);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
