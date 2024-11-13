public class ModificarPresupuestoViewModel
{
    int idPresupuesto;
    int idCliente;

    DateTime fechaCreacion;

    public ModificarPresupuestoViewModel()
    {
    }

    public int IdPresupuesto { get => idPresupuesto; set => idPresupuesto = value; }
    public int IdCliente { get => idCliente; set => idCliente = value; }
    public DateTime FechaCreacion { get => fechaCreacion; set => fechaCreacion = value; }
}