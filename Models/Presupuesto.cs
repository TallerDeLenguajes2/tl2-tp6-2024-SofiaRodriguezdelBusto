using System.Text.Json.Serialization;

public class Presupuesto
{
    int idPresupuesto;
    Cliente cliente;

    DateTime fechaCreacion;
    List<PresupuestoDetalle> detalle;


    public int IdPresupuesto { get => idPresupuesto; set => idPresupuesto = value; }
    public Cliente Cliente { get => cliente; set => cliente = value; }
    public DateTime FechaCreacion { get => fechaCreacion; set => fechaCreacion = value; }
    public List<PresupuestoDetalle> Detalle { get => detalle; set => detalle = value; }

    [JsonConstructor]

    public Presupuesto()
    {
        
    }
    public Presupuesto(int idPresupuesto, Cliente cliente, DateTime fechaCreacion)
    {
        IdPresupuesto = idPresupuesto;
        Cliente = cliente;
        FechaCreacion = fechaCreacion;
        detalle = new List<PresupuestoDetalle>();

    }

    public Presupuesto(AltaPresupuestoViewModel presuVM)
    {
        cliente = new Cliente();
        cliente.ClienteId = presuVM.IdCliente;
        FechaCreacion = presuVM.FechaCreacion;
    }

    public Presupuesto(ModificarPresupuestoViewModel presuVM)
    {
        idPresupuesto = presuVM.IdPresupuesto;
        cliente = new Cliente();
        cliente.ClienteId = presuVM.IdCliente;
        fechaCreacion = presuVM.FechaCreacion;
    }



    public double MontoPresupuesto()
    {
        int monto = detalle.Sum(d => d.Cantidad*d.Producto.Precio);
        return monto;

    }
    public double MontoPresupuestoConIva()
    {
        return MontoPresupuesto()*1.21;
    }
    public int CantidadProductos ()
    {
        return detalle.Sum(d => d.Cantidad);
    }


}