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
        return View();
    }

    
    [HttpPost]
    public IActionResult CrearPresupuesto(Presupuesto presupuesto)
    {
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
        var producto  = repoPresupuestos.ObtenerPresupuestoPorId(id);
        return View(producto);
    }

    [HttpPost]
    public IActionResult ModificarPresupuesto(Presupuesto presupuesto)
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