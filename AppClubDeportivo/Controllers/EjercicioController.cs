using AppClubDeportivo.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace AppClubDeportivo.Controllers
{
    public class EjercicioController : Controller
    {
        public IActionResult Ejercicios()
        {
            // Crear algunos ejemplos de ejercicios
            var listaEjercicios = new List<Ejercicio>
            {
                new Ejercicio
                {
                    Id = 1,
                    Nombre = "Sentadillas",
                    Descripcion = "Ejercicio para fortalecer las piernas.",
                    ImagenUrl = "/images/sentadillas.jpg",
                    VideoUrl = "https://www.youtube.com/watch?v=EjercicioSentadilla"
                },
                new Ejercicio
                {
                    Id = 2,
                    Nombre = "Flexiones",
                    Descripcion = "Ejercicio para fortalecer el pecho y los brazos.",
                    ImagenUrl = "/images/flexiones.jpg",
                    VideoUrl = "https://www.youtube.com/watch?v=EjercicioFlexion"
                }
            };

            // Verificar si la lista contiene elementos antes de pasarla a la vista
            if (listaEjercicios != null && listaEjercicios.Count > 0)
            {
                return View(listaEjercicios);  // Pasar la lista a la vista
            }
            else
            {
                // Enviar un modelo vacío en caso de que no haya ejercicios disponibles
                return View(new List<Ejercicio>());
            }
        }
    }
}
