using System.ComponentModel.DataAnnotations; 
public class AgregarProductoAPresuViewModel
{
    int idPresupuesto;
    int idProducto;

    int cantidad;

    public AgregarProductoAPresuViewModel()
    {
    }


    public int IdPresupuesto { get => idPresupuesto; set => idPresupuesto = value; }


    [Required(ErrorMessage = "El Id del producto es obligario")]
    [Range(1, int.MaxValue, ErrorMessage = "El número debe ser un entero positivo.")]
    public int IdProducto { get => idProducto; set => idProducto = value; }

    [Required(ErrorMessage = "La cantidad del producto es obligatoria.")]
    [Range(1, int.MaxValue, ErrorMessage = "El número debe ser un entero positivo.")]
    public int Cantidad { get => cantidad; set => cantidad = value; }
}