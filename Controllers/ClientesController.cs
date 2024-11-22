using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MiWebApi.Controllers;


//[Authorize]
public class ClientesController : Controller
{

    private readonly ILogger<ClientesController> _logger;

    private IClientesRepository repoClientes;

    public ClientesController(ILogger<ClientesController> logger, IClientesRepository clientesRepository )
    {
        _logger = logger;
        repoClientes = clientesRepository;
    }
public IActionResult Index()
{
    try
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("User"))) 
            return RedirectToAction("Index", "Login");

        ViewData["EsAdmin"] = HttpContext.Session.GetString("AccessLevel") == "Admin";
        return View(repoClientes.ObtenerClientes());
    }
    catch (Exception ex)
    {
        _logger.LogError(ex.ToString());
        ViewBag.ErrorMessage = "No se pudo cargar la lista de clientes";
        return RedirectToAction("Index");
    }
}

[HttpGet]
public IActionResult AltaCliente()
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
        ViewBag.ErrorMessage = "No se pudo cargar el formulario de alta de cliente";
        return RedirectToAction("Index");
    }
}

[HttpPost]
public IActionResult CrearCliente(AltaClienteViewModel clienteVM)
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

        var cliente = new Cliente(clienteVM);
        repoClientes.CrearCliente(cliente);
        return RedirectToAction("Index");
    }
    catch (Exception ex)
    {
        _logger.LogError(ex.ToString());
        ViewBag.ErrorMessage = "No se pudo crear el cliente";
        return RedirectToAction("Index");
    }
}

[HttpGet]
public IActionResult ModificarCliente(int id)
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

        var cliente = repoClientes.ObtenerCliente(id);
        var clienteVM = new ModificarClienteViewModel(cliente);
        return View(clienteVM);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex.ToString());
        ViewBag.ErrorMessage = "No se pudo cargar el formulario de modificación del cliente";
        return RedirectToAction("Index");
    }
}

[HttpPost]
public IActionResult ModificarCliente(ModificarClienteViewModel clienteVM)
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

        var cliente = new Cliente(clienteVM);
        repoClientes.ModificarCliente(cliente);
        return RedirectToAction("Index");
    }
    catch (Exception ex)
    {
        _logger.LogError(ex.ToString());
        ViewBag.ErrorMessage = "No se pudo modificar el cliente";
        return RedirectToAction("Index");
    }
}

[HttpGet]
public IActionResult EliminarCliente(int id)
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

        return View(repoClientes.ObtenerCliente(id));
    }
    catch (Exception ex)
    {
        _logger.LogError(ex.ToString());
        ViewBag.ErrorMessage = "No se pudo cargar el cliente para eliminar";
        return RedirectToAction("Index");
    }
}

[HttpGet]
public IActionResult EliminarClientePorId(int id)
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

        repoClientes.EliminarCliente(id);
        return RedirectToAction("Index");
    }
    catch (Exception ex)
    {
        _logger.LogError(ex.ToString());
        ViewBag.ErrorMessage = "No se pudo eliminar el cliente";
        return RedirectToAction("Index");
    }
}

}