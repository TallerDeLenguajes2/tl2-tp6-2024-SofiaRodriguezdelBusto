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
        return View(repoClientes.GetClientes());
    }
    

}