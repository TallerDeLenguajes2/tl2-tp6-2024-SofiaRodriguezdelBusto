public interface IProductosRepository
{
    public void CrearProducto(Producto producto);
    public List<Producto>  ObtenerProductos();
    public void ModificarProducto(Producto producto);
    public Producto  ObtenerProductoPorId(int id);
    public void EliminarProductoPorId(int id);
}