using System.ComponentModel.DataAnnotations; 
public class ModificarPresupuestoViewModel
{
    int idPresupuesto;
    int idCliente;

    DateTime fechaCreacion;

    public ModificarPresupuestoViewModel()
    {
    }

    public int IdPresupuesto { get => idPresupuesto; set => idPresupuesto = value; }

    [Required(ErrorMessage = "El id del cliente es obligatorio.")]
    public int IdCliente { get => idCliente; set => idCliente = value; }

    [Required(ErrorMessage = "La fecha es obligatoria.")]
    public DateTime FechaCreacion { get => fechaCreacion; set => fechaCreacion = value; }
}