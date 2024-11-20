using AppClubDeportivo.Models;
using AppClubDeportivo.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace AppClubDeportivo.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDBContext _context;

        public AdminController(AppDBContext context)
        {
            _context = context;
        }

        // Método para mostrar el Dashboard
        public IActionResult Index()
        {
            // Verificar si el usuario está en sesión y es admin
            var usuarioJson = HttpContext.Session.GetString("Usuario");
            if (string.IsNullOrEmpty(usuarioJson))
            {
                return RedirectToAction("Login", "Usuario");
            }

            var usuario = Newtonsoft.Json.JsonConvert.DeserializeObject<Usuario>(usuarioJson);
            if (usuario.Rol != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            // Cargar las actividades, usuarios y eventos de forma síncrona
            var actividades = _context.Actividades.ToList();
            var usuarios = _context.Usuarios.ToList();
            var eventos = _context.Eventos.ToList();

            // Pasar los datos a la vista
            ViewBag.Actividades = actividades;
            ViewBag.Usuarios = usuarios;
            ViewBag.Eventos = eventos;

            return View();
        }

        // Acción para crear una nueva actividad
        [HttpGet]
        public IActionResult CrearActividad()
        {
            return View();
        }

        // Acción para crear la actividad (POST)
        [HttpPost]
        public IActionResult CrearActividad(Actividad nuevaActividad)
        {
            if (ModelState.IsValid)
            {
                // Agregar la nueva actividad de forma síncrona
                _context.Actividades.Add(nuevaActividad);
                _context.SaveChanges(); // Guarda los cambios de forma síncrona
                return RedirectToAction("Index");
            }
            return View(nuevaActividad);
        }

        // Acción para editar una actividad (GET)
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

        // Acción para editar la actividad (POST)
        [HttpPost]
        public IActionResult EditarActividad(Actividad actividad)
        {
            if (ModelState.IsValid)
            {
                _context.Update(actividad);
                _context.SaveChanges(); // Guarda los cambios de forma síncrona
                return RedirectToAction("Index");
            }
            return View(actividad);
        }

        // Acción para eliminar una actividad
        [HttpPost]
        public IActionResult EliminarActividad(int id)
        {
            var actividad = _context.Actividades.Find(id);
            if (actividad != null)
            {
                _context.Actividades.Remove(actividad);
                _context.SaveChanges(); // Guarda los cambios de forma síncrona
            }
            return RedirectToAction("Index");
        }

        // Acción para crear un evento
        [HttpGet]
        public IActionResult CrearEventoAdmin()
        {
            return View();
        }

        // Acción para crear evento (POST)
        [HttpPost]
        public IActionResult CrearEventoAdmin(Evento nuevoEvento)
        {
            if (ModelState.IsValid)
            {
                _context.Eventos.Add(nuevoEvento);
                _context.SaveChanges(); // Guarda los cambios de forma síncrona
                return RedirectToAction("Index");
            }
            return View(nuevoEvento);
        }

        // Acción para eliminar un evento
        [HttpPost]
        public IActionResult EliminarEvento(int id)
        {
            var evento = _context.Eventos.Find(id);
            if (evento != null)
            {
                _context.Eventos.Remove(evento);
                _context.SaveChanges(); // Guarda los cambios de forma síncrona
            }
            return RedirectToAction("Index");
        }

        // Acción para editar evento (GET)
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

        // Acción para editar evento (POST)
        [HttpPost]
        public IActionResult EditarEvento(Evento evento)
        {
            if (ModelState.IsValid)
            {
                _context.Update(evento);
                _context.SaveChanges(); // Guarda los cambios de forma síncrona
                return RedirectToAction("Index");
            }
            return View(evento);
        }

        // Acción para editar usuario (GET)
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

        // Acción para editar usuario (POST)
        [HttpPost]
        public IActionResult EditarUsuario(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                _context.Update(usuario);
                _context.SaveChanges(); // Guarda los cambios de forma síncrona
                return RedirectToAction("Index");
            }
            return View(usuario);
        }

        // Acción para eliminar usuario
        [HttpPost]
        public IActionResult EliminarUsuario(int id)
        {
            var usuario = _context.Usuarios.Find(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
                _context.SaveChanges(); // Guarda los cambios de forma síncrona
            }
            return RedirectToAction("Index");
        }
    }
}
