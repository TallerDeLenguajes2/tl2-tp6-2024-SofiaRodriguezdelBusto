using System.ComponentModel.DataAnnotations;

public class CrearUsuarioViewModel
{
    [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
    public string Username { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    public string Nombre { get; set; }

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
    public string Password { get; set; }

    [Required(ErrorMessage = "El nivel de acceso es obligatorio.")]
    [EnumDataType(typeof(AccessLevel), ErrorMessage = "El nivel de acceso no es válido.")]
    public AccessLevel AccessLevel { get; set; }
}
