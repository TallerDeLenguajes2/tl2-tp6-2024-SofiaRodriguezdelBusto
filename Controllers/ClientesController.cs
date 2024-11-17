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
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("User"))) return RedirectToAction ("Index", "Login");
        return View(repoClientes.ObtenerClientes());
    }

        [HttpGet]
    public IActionResult AltaCliente()
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
    public IActionResult CrearCliente(AltaClienteViewModel clienteVM)
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("User"))) return RedirectToAction ("Index", "Login");
        if (HttpContext.Session.GetString("AccessLevel") != "Admin")
        {
            TempData["ErrorMessage"] = "No tienes permisos para realizar esta acción.";
            return RedirectToAction("Index");
        }
        if(!ModelState.IsValid) return RedirectToAction ("Index");
        var cliente = new Cliente(clienteVM);
        repoClientes.CrearCliente(cliente);
        return RedirectToAction ("Index");

    }

    [HttpGet]
    public IActionResult ModificarCliente(int id)
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("User"))) return RedirectToAction ("Index", "Login");
        if (HttpContext.Session.GetString("AccessLevel") != "Admin")
        {
            TempData["ErrorMessage"] = "No tienes permisos para realizar esta acción.";
            return RedirectToAction("Index");
        }
        var Cliente  = repoClientes.ObtenerCliente(id);
        var clienteVM = new ModificarClienteViewModel(Cliente);
        return View(clienteVM);
    }

    [HttpPost]
    public IActionResult ModificarCliente(ModificarClienteViewModel clienteVM)
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("User"))) return RedirectToAction ("Index", "Login");
        if (HttpContext.Session.GetString("AccessLevel") != "Admin")
        {
            TempData["ErrorMessage"] = "No tienes permisos para realizar esta acción.";
            return RedirectToAction("Index");
        }
        if(!ModelState.IsValid) return RedirectToAction ("Index");
        var cliente = new Cliente(clienteVM);
        repoClientes.ModificarCliente(cliente);
        return RedirectToAction ("Index"); 

    }

    [HttpGet]
    public IActionResult EliminarCliente(int id)
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("User"))) return RedirectToAction ("Index", "Login");
        if (HttpContext.Session.GetString("AccessLevel") != "Admin")
        {
            TempData["ErrorMessage"] = "No tienes permisos para realizar esta acción.";
            return RedirectToAction("Index");
        }
        return View(repoClientes.ObtenerCliente(id));
    }

    [HttpGet]
    public IActionResult EliminarClientePorId(int id)
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("User"))) return RedirectToAction ("Index", "Login");
        if (HttpContext.Session.GetString("AccessLevel") != "Admin")
        {
            TempData["ErrorMessage"] = "No tienes permisos para realizar esta acción.";
            return RedirectToAction("Index");
        }
        repoClientes.EliminarCliente(id);
        return RedirectToAction ("Index"); 
    }

    

}