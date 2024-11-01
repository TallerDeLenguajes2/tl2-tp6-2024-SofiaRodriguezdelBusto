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

    [HttpPost("api/Producto")]
    public IActionResult CrearProducto(Producto producto)
    {
        repoProductos.CrearProducto(producto);
        return Created();

    }

    [HttpGet]
    public IActionResult ModificarProducto(int id)
    {
        var producto  = repoProductos.ObtenerProductoPorId(id);
        return View(producto);
    }

    [HttpPost]
    public IActionResult ModificarProducto(Producto producto)
    {
        repoProductos.ModificarProducto(producto);
        return RedirectToAction ("Index"); 

    }

    [HttpDelete]
    public IActionResult EliminarProductoPorId(int id)
    {
        repoProductos.EliminarProductoPorId(id);
        return RedirectToAction ("Index"); 
    }

}