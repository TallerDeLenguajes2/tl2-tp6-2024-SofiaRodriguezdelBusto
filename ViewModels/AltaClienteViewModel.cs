

using System.ComponentModel.DataAnnotations; 
public class AltaClienteViewModel
{
    string nombre;
    string email;

    string telefono;

    public AltaClienteViewModel()
    {
    }
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    public string Nombre { get => nombre; set => nombre = value; }
    
    [Required(ErrorMessage = "El email es obligatorio.")]
    [EmailAddress(ErrorMessage = "El email no es válido.")]
    public string Email { get => email; set => email = value; }
        
    [Required(ErrorMessage = "El teléfono es obligatorio.")]
    [Phone(ErrorMessage = "El teléfono no es válido.")]
    public string Telefono { get => telefono; set => telefono = value; }

    
}