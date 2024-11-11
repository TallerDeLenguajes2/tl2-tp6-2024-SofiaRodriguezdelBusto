using System.ComponentModel.DataAnnotations; 
public class AltaPresupuestoViewModel
{
    int idCliente;

    DateTime fechaCreacion;

    public AltaPresupuestoViewModel()
    {
    }
    [Required(ErrorMessage = "El cliente es obligatorio.")]
    public int IdCliente { get => idCliente; set => idCliente = value; }

    [Required(ErrorMessage = "La fecha es obligatoria.")]
    public DateTime FechaCreacion { get => fechaCreacion; set => fechaCreacion = value; }
}