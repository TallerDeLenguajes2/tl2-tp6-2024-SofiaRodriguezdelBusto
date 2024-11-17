using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MiWebApi.Controllers;


//[Authorize]
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
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("User"))) return RedirectToAction ("Index", "Login");
        ViewData["EsAdmin"] = HttpContext.Session.GetString("AccessLevel") == "Admin";
        return View(repoProductos.ObtenerProductos());
    }

    [HttpGet]
    public IActionResult AltaProducto()
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("User"))) return RedirectToAction ("Index", "Login");
        if (HttpContext.Session.GetString("AccessLevel") != "Admin")
        {
            TempData["ErrorMessage"] = "No tienes permisos para realizar esta acción.";
            return RedirectToAction("Index");
        }
        return View();
    }

    [HttpPost]
    public IActionResult CrearProducto(AltaProductoViewModel productoVM)
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("User"))) return RedirectToAction ("Index", "Login");
        if (HttpContext.Session.GetString("AccessLevel") != "Admin")
        {
            TempData["ErrorMessage"] = "No tienes permisos para realizar esta acción.";
            return RedirectToAction("Index");
        }
        if(!ModelState.IsValid) return RedirectToAction ("Index");
        var producto = new Producto(productoVM);
        repoProductos.CrearProducto(producto);
        return RedirectToAction ("Index");

    }

    [HttpGet]
    public IActionResult ModificarProducto(int id)
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("User"))) return RedirectToAction ("Index", "Login");
        if (HttpContext.Session.GetString("AccessLevel") != "Admin")
        {
            TempData["ErrorMessage"] = "No tienes permisos para realizar esta acción.";
            return RedirectToAction("Index");
        }
        var producto  = repoProductos.ObtenerProductoPorId(id);
        ModificarProductoViewModel prod = new ModificarProductoViewModel(producto);
        return View(prod);
    }

    [HttpPost]
    public IActionResult ModificarProducto(ModificarProductoViewModel productoVM)
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("User"))) return RedirectToAction ("Index", "Login");
        if (HttpContext.Session.GetString("AccessLevel") != "Admin")
        {
            TempData["ErrorMessage"] = "No tienes permisos para realizar esta acción.";
            return RedirectToAction("Index");
        }
        if(!ModelState.IsValid) return RedirectToAction ("Index");
        var producto = new Producto(productoVM);
        repoProductos.ModificarProducto(producto);
        return RedirectToAction ("Index"); 

    }

    [HttpGet]
    public IActionResult EliminarProducto(int id)
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("User"))) return RedirectToAction ("Index", "Login");
        if (HttpContext.Session.GetString("AccessLevel") != "Admin")
        {
            TempData["ErrorMessage"] = "No tienes permisos para realizar esta acción.";
            return RedirectToAction("Index");
        }
        return View(repoProductos.ObtenerProductoPorId(id));
    }

    [HttpGet]
    public IActionResult EliminarProductoPorId(int id)
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("User"))) return RedirectToAction ("Index", "Login");
        if (HttpContext.Session.GetString("AccessLevel") != "Admin")
        {
            TempData["ErrorMessage"] = "No tienes permisos para realizar esta acción.";
            return RedirectToAction("Index");
        }
        repoProductos.EliminarProductoPorId(id);
        return RedirectToAction ("Index"); 
    }

}