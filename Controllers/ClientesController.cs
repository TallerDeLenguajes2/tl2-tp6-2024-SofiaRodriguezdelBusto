using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MiWebApi.Controllers;


public class ClientesController : Controller
{

    private readonly ILogger<ClientesController> _logger;

    private ClientesRepository repoClientes;

    public ClientesController(ILogger<ClientesController> logger)
    {
        _logger = logger;
        repoClientes = new ClientesRepository();
    }

    public IActionResult Index()
    {
        return View(repoClientes.ObtenerClientes());
    }

        [HttpGet]
    public IActionResult AltaCliente()
    {
        return View();
    }

    [HttpPost]
    public IActionResult CrearCliente(ClienteViewModel cliente)
    {
        if(!ModelState.IsValid) return RedirectToAction ("Index");
        repoClientes.CrearCliente(cliente);
        return RedirectToAction ("Index");

    }

    [HttpGet]
    public IActionResult ModificarCliente(int id)
    {
        var Cliente  = repoClientes.ObtenerCliente(id);
        return View(Cliente);
    }

    [HttpPost]
    public IActionResult ModificarCliente(Cliente cliente)
    {
        repoClientes.ModificarCliente(cliente);
        return RedirectToAction ("Index"); 

    }

    [HttpGet]
    public IActionResult EliminarCliente(int id)
    {
        return View(repoClientes.ObtenerCliente(id));
    }

    [HttpGet]
    public IActionResult EliminarClientePorId(int id)
    {
        repoClientes.EliminarCliente(id);
        return RedirectToAction ("Index"); 
    }

    

}