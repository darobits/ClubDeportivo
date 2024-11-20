namespace AppClubDeportivo.Models
{
    public class Inscripcion
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
        public int ActividadId { get; set; }
        public Actividad Actividad { get; set; }
        public DateTime FechaInscripcion { get; set; }
        public string Estado { get; set; } // Ej. "Confirmada", "Pendiente"
    }


}
