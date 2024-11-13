using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MiWebApi.Controllers;


public class PresupuestosController : Controller
{

    private readonly ILogger<PresupuestosController> _logger;

    private PresupuestosRepository repoPresupuestos;

    public PresupuestosController(ILogger<PresupuestosController> logger)
    {
        _logger = logger;
        repoPresupuestos = new PresupuestosRepository();
    }

    public IActionResult Index()
    {
        return View(repoPresupuestos.ObtenerPresupuestos());
    }

    [HttpGet]

    public IActionResult DetallesDelPresupuesto(int id)
    {
        return View(repoPresupuestos.ObtenerPresupuestoPorId(id));
    }

    [HttpGet]
    public IActionResult AltaPresupuesto()
    {
        ClientesRepository repoClientes = new ClientesRepository();
        List<Cliente> Clientes = repoClientes.ObtenerClientes();
        ViewData["Clientes"] =  Clientes.Select(c=> new SelectListItem
        {
            Value = c.ClienteId.ToString(), 
            Text = c.Nombre
        }).ToList();

        return View();
    }

    
    [HttpPost]
    public IActionResult CrearPresupuesto(AltaPresupuestoViewModel presupuesto)
    {

       if(!ModelState.IsValid) return RedirectToAction ("Index");
       repoPresupuestos.CrearPresupuesto(presupuesto);
        return RedirectToAction ("Index");

    }

    [HttpGet]

    public IActionResult AgregarProductoAPresupuesto(int id)
    {
        ProductosRepository repoProductos = new ProductosRepository();
        List<Producto> productos = repoProductos.ObtenerProductos();
        ViewData["Productos"] = productos.Select(p => new SelectListItem
        {
            Value = p.IdProducto.ToString(), 
            Text = p.Descripcion 
        }).ToList();

        return View(id);
    }

    [HttpPost]

    public IActionResult AgregarProductoEnPresupuesto(int idPresupuesto, int idProducto, int cantidad)
    {
        repoPresupuestos.AgregarProducto(idPresupuesto, idProducto, cantidad);
        return RedirectToAction ("Index");
    }
    
    [HttpGet]

    public IActionResult EliminarProductoAPresupuesto(int id)
    {
        Presupuesto presupuesto = repoPresupuestos.ObtenerPresupuestoPorId(id);
        ViewData["Productos"] = presupuesto.Detalle.Select(p => new SelectListItem
        {
            Value = p.Producto.IdProducto.ToString(), 
            Text = p.Producto.Descripcion 
        }).ToList();

        return View(id);
    }

    [HttpPost]

    public IActionResult EliminarProductoEnPresupuesto(int idPresupuesto, int idProducto)
    {
        repoPresupuestos.EliminarProducto(idPresupuesto, idProducto);
        return RedirectToAction ("Index");
    }

    [HttpGet]
    public IActionResult ModificarPresupuesto(int id)
    {
        ClientesRepository repoClientes = new ClientesRepository();
        List<Cliente> Clientes = repoClientes.ObtenerClientes();
        ViewData["Clientes"] =  Clientes.Select(c=> new SelectListItem
        {
            Value = c.ClienteId.ToString(), 
            Text = c.Nombre
        }).ToList();
        var presupuesto  = repoPresupuestos.ObtenerPresupuestoPorId(id);
        return View(presupuesto);
    }

    [HttpPost]
    public IActionResult ModificarPresupuesto(ModificarPresupuestoViewModel presupuesto)
    {
        repoPresupuestos.ModificarPresupuesto(presupuesto);
        return RedirectToAction ("Index"); 

    }
    

    [HttpGet]

    public IActionResult EliminarPresupuesto(int id)
    {
        return View(repoPresupuestos.ObtenerPresupuestoPorId(id));
    }

    [HttpGet]
    public IActionResult EliminarPresupuestoPorId(int id)
    {
        repoPresupuestos.EliminarPresupuestoPorId(id);
        return RedirectToAction ("Index"); 
    }
}