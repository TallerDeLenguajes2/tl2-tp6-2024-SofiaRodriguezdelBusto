using System.ComponentModel.DataAnnotations; 
public class ModificarProductoViewModel
{
    
    int idProducto;
    string descripcion;
    int precio;

    public ModificarProductoViewModel()
    {
        
    }

    public ModificarProductoViewModel(Producto producto)
    {
        idProducto = producto.IdProducto;
        descripcion = producto.Descripcion;
        precio = producto.Precio;
        
    }


    public int IdProducto { get => idProducto; set => idProducto = value; }
    
    [StringLength(250, ErrorMessage = "La descripción no puede exceder los 250 caracteres.")]
    public string Descripcion { get => descripcion; set => descripcion = value; }

    [Required(ErrorMessage = "El precio es obligatorio.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser un valor positivo.")]
    public int Precio { get => precio; set => precio = value; }
    
}