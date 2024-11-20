using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

public class Usuario
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [MaxLength(30, ErrorMessage = "El nombre no puede tener más de 30 caracteres.")]
    public string Nombre { get; set; }

    [Required(ErrorMessage = "El apellido es obligatorio.")]
    [MaxLength(30, ErrorMessage = "El apellido no puede tener más de 30 caracteres.")]
    public string Apellido { get; set; }

    [Required(ErrorMessage = "El correo es obligatorio.")]
    [EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
    [MaxLength(50, ErrorMessage = "El correo no puede tener más de 50 caracteres.")]
    public string Correo { get; set; }

    [Required(ErrorMessage = "El teléfono es obligatorio.")]
    [MaxLength(20, ErrorMessage = "El teléfono no puede tener más de 20 caracteres.")]
    public string Telefono { get; set; }

    [Required(ErrorMessage = "El DNI es obligatorio.")]
    [MaxLength(20, ErrorMessage = "El DNI no puede tener más de 20 caracteres.")]
    public string DNI { get; set; }

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [MaxLength(256, ErrorMessage = "La contraseña no puede tener más de 256 caracteres.")]
    public string Contraseña { get; set; }

    [Required(ErrorMessage = "El rol es obligatorio.")]
    [MaxLength(20, ErrorMessage = "El rol no puede tener más de 20 caracteres.")]
    public string Rol { get; set; }

    // Método para establecer la contraseña de manera encriptada
    public void SetPassword(string password)
    {
        var hasher = new PasswordHasher<Usuario>();
        Contraseña = hasher.HashPassword(this, password);
    }

    // Método para verificar la contraseña ingresada
    public bool VerifyPassword(string password)
    {
        var hasher = new PasswordHasher<Usuario>();
        var result = hasher.VerifyHashedPassword(this, Contraseña, password);
        return result == PasswordVerificationResult.Success;
    }
}
