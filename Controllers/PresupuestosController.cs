using Microsoft.AspNetCore.Mvc;

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

    public IActionResult ObtenerDetallesDelPresupuesto(int id)
    {
        
    }

    [HttpGet]
    public IActionResult AltaProducto()
    {
        return View();
    }

    [HttpPost]
    public IActionResult CrearProducto(Producto producto)
    {
        repoPresupuestos.CrearProducto(producto);
        return RedirectToAction ("Index");

    }

    [HttpGet]
    public IActionResult ModificarProducto(int id)
    {
        var producto  = repoPresupuestos.ObtenerProductoPorId(id);
        return View(producto);
    }

    [HttpPost]
    public IActionResult ModificarProducto(Producto producto)
    {
        repoPresupuestos.ModificarProducto(producto);
        return RedirectToAction ("Index"); 

    }

    [HttpGet]
    public IActionResult EliminarProductoPorId(int id)
    {
        repoPresupuestos.EliminarProductoPorId(id);
        return RedirectToAction ("Index"); 
    }

}