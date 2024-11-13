
using System.ComponentModel.DataAnnotations; 
public class AltaProductoViewModel
{
    string descripcion;
    double precio;

    public AltaProductoViewModel()
    {
    }

    [StringLength(250, ErrorMessage = "La descripción no puede exceder los 250 caracteres.")]
    public string Descripcion { get => descripcion; set => descripcion = value; }

    [Required(ErrorMessage = "El precio es obligatorio.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser un valor positivo.")]
    public double Precio { get => precio; set => precio = value; }
}