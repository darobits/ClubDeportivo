using AppClubDeportivo.Data;
using AppClubDeportivo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq;

namespace AppClubDeportivo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDBContext _context;

        public HomeController(ILogger<HomeController> logger, AppDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        // Acción principal (Index)
        public IActionResult Index()
        {
            if (!ValidarSesionActiva(out Usuario usuario))
            {
                return RedirectToAction("Login", "Usuario"); 
            }

            ViewBag.UsuarioNombre = $"{usuario.Nombre} {usuario.Apellido}";
            ViewBag.RolUsuario = usuario.Rol;

            if (usuario.Rol == "Admin")
            {
                return RedirectToAction("Dashboard", "Admin");
            }

            // Obtener los eventos desde la base de datos
            var eventos = _context.Eventos.ToList();

            // Pasar los eventos a la vista
            return View(eventos);
        }


        // Acción para crear un evento
        [HttpPost]
        public IActionResult CrearEvento(string nombre, string sede, DateTime fecha, string descripcion)
        {
            if (!ValidarSesionActiva(out Usuario usuario))
            {
                return RedirectToAction("Login", "Usuario");
            }

            // Crear un nuevo evento
            var evento = new Evento
            {
                Nombre = nombre,
                Sede = sede,
                Fecha = fecha,
                Descripcion = descripcion
            };

            // Guardar el evento en la base de datos
            _context.Eventos.Add(evento);
            _context.SaveChanges();

            // Redirigir a la vista principal (Index)
            return RedirectToAction("Index");
        }

        // Acción para eliminar un evento
        [HttpPost]
        public IActionResult EliminarEvento(int id)
        {
            if (!ValidarSesionActiva(out Usuario usuario))
            {
                return RedirectToAction("Login", "Usuario");
            }

            // Buscar el evento por ID
            var evento = _context.Eventos.Find(id);
            if (evento != null)
            {
                // Eliminar el evento de la base de datos
                _context.Eventos.Remove(evento);
                _context.SaveChanges();
            }

            // Redirigir a la vista principal (Index)
            return RedirectToAction("Index");
        }

        // Acción para mostrar el formulario de edición de evento
        [HttpGet]
        public IActionResult EditarEvento(int id)
        {
            if (!ValidarSesionActiva(out Usuario usuario))
            {
                return RedirectToAction("Login", "Usuario");
            }

            // Buscar el evento por ID
            var evento = _context.Eventos.Find(id);
            if (evento == null)
            {
                return NotFound();
            }

            // Pasar el evento a la vista de edición
            return View(evento);
        }

        // Acción para guardar los cambios del evento editado
        [HttpPost]
        public IActionResult EditarEvento(Evento evento)
        {
            if (!ValidarSesionActiva(out Usuario usuario))
            {
                return RedirectToAction("Login", "Usuario");
            }

            if (ModelState.IsValid)
            {
                // Actualizar el evento en la base de datos
                _context.Eventos.Update(evento);
                _context.SaveChanges();

                // Redirigir a la vista principal (Index)
                return RedirectToAction("Index");
            }

            // Si el modelo no es válido, volver a mostrar el formulario de edición
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
