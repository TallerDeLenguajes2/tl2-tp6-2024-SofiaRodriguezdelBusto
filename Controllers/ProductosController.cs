using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MiWebApi.Controllers;


//[Authorize]
public class ProductosController : Controller
{

    private readonly ILogger<ProductosController> _logger;

    private IProductosRepository repoProductos;

    public ProductosController(ILogger<ProductosController> logger, IProductosRepository productosRepository)
    {
        _logger = logger;
        repoProductos = productosRepository;
    }
public IActionResult Index()
{
    try
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("User")))
            return RedirectToAction("Index", "Login");

        ViewData["EsAdmin"] = HttpContext.Session.GetString("AccessLevel") == "Admin";
        return View(repoProductos.ObtenerProductos());
    }
    catch (Exception ex)
    {
        _logger.LogError(ex.ToString());
        ViewBag.ErrorMessage = "No se pudo cargar la lista de productos.";
        return RedirectToAction("Index");
    }
}

[HttpGet]
public IActionResult AltaProducto()
{
    try
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("User")))
            return RedirectToAction("Index", "Login");

        if (HttpContext.Session.GetString("AccessLevel") != "Admin")
        {
            TempData["ErrorMessage"] = "No tienes permisos para realizar esta acción.";
            return RedirectToAction("Index");
        }

        return View();
    }
    catch (Exception ex)
    {
        _logger.LogError(ex.ToString());
        ViewBag.ErrorMessage = "No se pudo cargar el formulario de alta de producto.";
        return RedirectToAction("Index");
    }
}

[HttpPost]
public IActionResult CrearProducto(AltaProductoViewModel productoVM)
{
    try
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("User")))
            return RedirectToAction("Index", "Login");

        if (HttpContext.Session.GetString("AccessLevel") != "Admin")
        {
            TempData["ErrorMessage"] = "No tienes permisos para realizar esta acción.";
            return RedirectToAction("Index");
        }

        if (!ModelState.IsValid)
            return RedirectToAction("Index");

        var producto = new Producto(productoVM);
        repoProductos.CrearProducto(producto);
        return RedirectToAction("Index");
    }
    catch (Exception ex)
    {
        _logger.LogError(ex.ToString());
        ViewBag.ErrorMessage = "No se pudo crear el producto.";
        return RedirectToAction("Index");
    }
}

[HttpGet]
public IActionResult ModificarProducto(int id)
{
    try
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("User")))
            return RedirectToAction("Index", "Login");

        if (HttpContext.Session.GetString("AccessLevel") != "Admin")
        {
            TempData["ErrorMessage"] = "No tienes permisos para realizar esta acción.";
            return RedirectToAction("Index");
        }

        var producto = repoProductos.ObtenerProductoPorId(id);
        var productoVM = new ModificarProductoViewModel(producto);
        return View(productoVM);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex.ToString());
        ViewBag.ErrorMessage = "No se pudo cargar el formulario de modificación del producto.";
        return RedirectToAction("Index");
    }
}

[HttpPost]
public IActionResult ModificarProducto(ModificarProductoViewModel productoVM)
{
    try
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("User")))
            return RedirectToAction("Index", "Login");

        if (HttpContext.Session.GetString("AccessLevel") != "Admin")
        {
            TempData["ErrorMessage"] = "No tienes permisos para realizar esta acción.";
            return RedirectToAction("Index");
        }

        if (!ModelState.IsValid)
            return RedirectToAction("Index");

        var producto = new Producto(productoVM);
        repoProductos.ModificarProducto(producto);
        return RedirectToAction("Index");
    }
    catch (Exception ex)
    {
        _logger.LogError(ex.ToString());
        ViewBag.ErrorMessage = "No se pudo modificar el producto.";
        return RedirectToAction("Index");
    }
}

[HttpGet]
public IActionResult EliminarProducto(int id)
{
    try
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("User")))
            return RedirectToAction("Index", "Login");

        if (HttpContext.Session.GetString("AccessLevel") != "Admin")
        {
            TempData["ErrorMessage"] = "No tienes permisos para realizar esta acción.";
            return RedirectToAction("Index");
        }

        return View(repoProductos.ObtenerProductoPorId(id));
    }
    catch (Exception ex)
    {
        _logger.LogError(ex.ToString());
        ViewBag.ErrorMessage = "No se pudo cargar el producto para eliminar.";
        return RedirectToAction("Index");
    }
}

[HttpGet]
public IActionResult EliminarProductoPorId(int id)
{
    try
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("User")))
            return RedirectToAction("Index", "Login");

        if (HttpContext.Session.GetString("AccessLevel") != "Admin")
        {
            TempData["ErrorMessage"] = "No tienes permisos para realizar esta acción.";
            return RedirectToAction("Index");
        }

        repoProductos.EliminarProductoPorId(id);
        return RedirectToAction("Index");
    }
    catch (Exception ex)
    {
        _logger.LogError(ex.ToString());
        ViewBag.ErrorMessage = "No se pudo eliminar el producto.";
        return RedirectToAction("Index");
    }
}


}