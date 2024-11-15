public interface IPresupuestoRepository
{
    
    public void CrearPresupuesto(Presupuesto presupuesto);
    public List<Presupuesto> ObtenerPresupuestos();
    public Presupuesto ObtenerPresupuestoPorId(int id);
    public void AgregarProducto(int idPresupuesto, int idProducto, int cantidad);
    public void EliminarProducto(int idPresupuesto, int idProducto);
    public void ModificarPresupuesto(Presupuesto presupuesto);
    public void EliminarPresupuestoPorId(int idPresupuesto);
}