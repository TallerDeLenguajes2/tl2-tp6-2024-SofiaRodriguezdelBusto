public class ModificarProductoViewModel
{
    
    int idProducto;
    string descripcion;
    int precio;

    public ModificarProductoViewModel()
    {
        
    }

    public int IdProducto { get => idProducto; set => idProducto = value; }
    public string Descripcion { get => descripcion; set => descripcion = value; }
    public int Precio { get => precio; set => precio = value; }
    
}