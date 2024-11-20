namespace AppClubDeportivo.Models
{
    public class Ejercicio
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string ImagenUrl { get; set; }  // Agregar esta propiedad
        public string VideoUrl { get; set; }   // Agregar esta propiedad

    }

}
