using Microsoft.AspNetCore.Identity;

public class Usuario
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string Correo { get; set; }
    public string Telefono { get; set; }
    public string DNI { get; set; }
    public string Contraseña { get; set; }
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
