using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MiWebApi.Controllers;


public class PresupuestosController : Controller
{

    private readonly ILogger<PresupuestosController> _logger;

    private IPresupuestoRepository repoPresupuestos;

    private IProductosRepository repoProductos;

    private IClientesRepository repoClientes;
    public PresupuestosController(ILogger<PresupuestosController> logger, IPresupuestoRepository repoPresupuestos, IProductosRepository repoProductos, IClientesRepository repoClientes)
    {
        _logger = logger;
        this.repoPresupuestos = repoPresupuestos;
        this.repoProductos = repoProductos;
        this.repoClientes = repoClientes;
    }

    public IActionResult Index()
    {
        try
        {            
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("User"))) return RedirectToAction ("Index", "Login");
            ViewData["EsAdmin"] = HttpContext.Session.GetString("AccessLevel") == "Admin";
            return View(repoPresupuestos.ObtenerPresupuestos());
        }
        catch (Exception ex)
        {             
            _logger.LogError(ex.ToString());
            ViewBag.ErrorMessage("No se pudo cargar el listado de presupuestos");
            return RedirectToAction("Index", "Home");
        }
    }

    [HttpGet]


    public IActionResult DetallesDelPresupuesto(int id)
    {
        try
        {            
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("User"))) return RedirectToAction ("Index", "Login");
            return View(repoPresupuestos.ObtenerPresupuestoPorId(id));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            ViewBag.ErrorMessage("No se pudo mostrar el presupuesto");
            return RedirectToAction("Index");
        }
    }

    [HttpGet]
    public IActionResult AltaPresupuesto()
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

            List<Cliente> Clientes = repoClientes.ObtenerClientes();
            ViewData["Clientes"] = Clientes.Select(c => new SelectListItem
            {
                Value = c.ClienteId.ToString(),
                Text = c.Nombre
            }).ToList();

            return View();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            ViewBag.ErrorMessage = "No se pudo cargar el formulario de alta de presupuesto";
            return RedirectToAction("Index");
        }
    }

  [HttpPost]
    public IActionResult CrearPresupuesto(AltaPresupuestoViewModel presupuestoVM)
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

            var presupuesto = new Presupuesto(presupuestoVM);
            repoPresupuestos.CrearPresupuesto(presupuesto);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            ViewBag.ErrorMessage = "No se pudo crear el presupuesto";
            return RedirectToAction("AltaPresupuesto");
        }
    }

    [HttpGet]

   public IActionResult AgregarProductoAPresupuesto(int id)
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

        List<Producto> productos = repoProductos.ObtenerProductos();
        ViewData["Productos"] = productos.Select(p => new SelectListItem
        {
            Value = p.IdProducto.ToString(),
            Text = p.Descripcion
        }).ToList();

        var model = new AgregarProductoAPresuViewModel();
        model.IdPresupuesto = id;

        return View(model);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex.ToString());
        ViewBag.ErrorMessage = "No se pudo cargar la lista de productos para el presupuesto";
        return RedirectToAction("Index");
    }
}

[HttpPost]
public IActionResult AgregarProductoEnPresupuesto(AgregarProductoAPresuViewModel infoProducto)
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

        repoPresupuestos.AgregarProducto(infoProducto.IdPresupuesto, infoProducto.IdProducto, infoProducto.Cantidad);
        return RedirectToAction("Index");
    }
    catch (Exception ex)
    {
        _logger.LogError(ex.ToString());
        ViewBag.ErrorMessage = "No se pudo agregar el producto al presupuesto";
        return RedirectToAction("Index");
    }
}

[HttpGet]
public IActionResult EliminarProductoAPresupuesto(int id)
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

        Presupuesto presupuesto = repoPresupuestos.ObtenerPresupuestoPorId(id);
        ViewData["Productos"] = presupuesto.Detalle.Select(p => new SelectListItem
        {
            Value = p.Producto.IdProducto.ToString(),
            Text = p.Producto.Descripcion
        }).ToList();

        return View(id);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex.ToString());
        ViewBag.ErrorMessage = "No se pudo cargar la lista de productos para eliminar";
        return RedirectToAction("Index");
    }
}
    public IActionResult EliminarProductoEnPresupuesto(int idPresupuesto, int idProducto)
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

        repoPresupuestos.EliminarProducto(idPresupuesto, idProducto);
        return RedirectToAction("Index");
    }
    catch (Exception ex)
    {
        _logger.LogError(ex.ToString());
        ViewBag.ErrorMessage = "No se pudo eliminar el producto del presupuesto";
        return RedirectToAction("Index");
    }
}

[HttpGet]
public IActionResult ModificarPresupuesto(int id)
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

        List<Cliente> Clientes = repoClientes.ObtenerClientes();
        ViewData["Clientes"] = Clientes.Select(c => new SelectListItem
        {
            Value = c.ClienteId.ToString(),
            Text = c.Nombre
        }).ToList();

        var presupuesto = repoPresupuestos.ObtenerPresupuestoPorId(id);
        var presupuestoVM = new ModificarPresupuestoViewModel
        {
            IdPresupuesto = id,
            FechaCreacion = presupuesto.FechaCreacion
        };

        return View(presupuestoVM);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex.ToString());
        ViewBag.ErrorMessage = "No se pudo cargar el presupuesto para modificar";
        return RedirectToAction("Index");
    }
}

[HttpPost]
public IActionResult ModificarPresupuesto(ModificarPresupuestoViewModel presupuestoVM)
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

        var presupuesto = new Presupuesto(presupuestoVM);
        repoPresupuestos.ModificarPresupuesto(presupuesto);
        return RedirectToAction("Index");
    }
    catch (Exception ex)
    {
        _logger.LogError(ex.ToString());
        ViewBag.ErrorMessage = "No se pudo modificar el presupuesto";
        return RedirectToAction("Index");
    }
}

[HttpGet]
public IActionResult EliminarPresupuesto(int id)
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

        return View(repoPresupuestos.ObtenerPresupuestoPorId(id));
    }
    catch (Exception ex)
    {
        _logger.LogError(ex.ToString());
        ViewBag.ErrorMessage = "No se pudo cargar el presupuesto para eliminar";
        return RedirectToAction("Index");
    }
}

[HttpGet]
public IActionResult EliminarPresupuestoPorId(int id)
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

        repoPresupuestos.EliminarPresupuestoPorId(id);
        return RedirectToAction("Index");
    }
    catch (Exception ex)
    {
        _logger.LogError(ex.ToString());
        ViewBag.ErrorMessage = "No se pudo eliminar el presupuesto";
        return RedirectToAction("Index");
    }
}

}