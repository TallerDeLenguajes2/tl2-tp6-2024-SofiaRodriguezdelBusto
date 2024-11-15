using Microsoft.AspNetCore.Mvc;

namespace MiWebApi.Controllers;


public class ProductosController : Controller
{

    private readonly ILogger<ProductosController> _logger;

    private ProductosRepository repoProductos;

    public ProductosController(ILogger<ProductosController> logger)
    {
        _logger = logger;
        repoProductos = new ProductosRepository();
    }

    public IActionResult Index()
    {
        return View(repoProductos.ObtenerProductos());
    }

    [HttpGet]
    public IActionResult AltaProducto()
    {
        return View();
    }

    [HttpPost]
    public IActionResult CrearProducto(AltaProductoViewModel productoVM)
    {
        if(!ModelState.IsValid) return RedirectToAction ("Index");
        var producto = new Producto(productoVM);
        repoProductos.CrearProducto(producto);
        return RedirectToAction ("Index");

    }

    [HttpGet]
    public IActionResult ModificarProducto(int id)
    {
        var producto  = repoProductos.ObtenerProductoPorId(id);
        ModificarProductoViewModel prod = new ModificarProductoViewModel(producto);
        return View(prod);
    }

    [HttpPost]
    public IActionResult ModificarProducto(ModificarProductoViewModel productoVM)
    {
        if(!ModelState.IsValid) return RedirectToAction ("Index");
        var producto = new Producto(productoVM);
        repoProductos.ModificarProducto(producto);
        return RedirectToAction ("Index"); 

    }

    [HttpGet]
    public IActionResult EliminarProducto(int id)
    {
        return View(repoProductos.ObtenerProductoPorId(id));
    }

    [HttpGet]
    public IActionResult EliminarProductoPorId(int id)
    {
        repoProductos.EliminarProductoPorId(id);
        return RedirectToAction ("Index"); 
    }

}