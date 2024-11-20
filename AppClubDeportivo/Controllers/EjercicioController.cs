using System;
using AppClubDeportivo.Models;
using Microsoft.AspNetCore.Mvc;

namespace AppClubDeportivo.Controllers
{
    public class EjercicioController : Controller
    {
        public IActionResult Ejercicios()
        {
            // Crear algunos ejemplos de ejercicios con los nuevos campos
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

            return View(listaEjercicios);  // Pasar la lista a la vista
        }
    }

}
