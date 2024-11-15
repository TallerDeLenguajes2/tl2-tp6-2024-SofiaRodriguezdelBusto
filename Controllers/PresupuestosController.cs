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
        return View(repoPresupuestos.ObtenerPresupuestos());
    }

    [HttpGet]

    public IActionResult DetallesDelPresupuesto(int id)
    {
        return View(repoPresupuestos.ObtenerPresupuestoPorId(id));
    }

    [HttpGet]
    public IActionResult AltaPresupuesto()
    {
        List<Cliente> Clientes = repoClientes.ObtenerClientes();
        ViewData["Clientes"] =  Clientes.Select(c=> new SelectListItem
        {
            Value = c.ClienteId.ToString(), 
            Text = c.Nombre
        }).ToList();

        return View();
    }

    
    [HttpPost]
    public IActionResult CrearPresupuesto(AltaPresupuestoViewModel presupuestoVM)
    {
        //chequear so tiene permiso
       if(!ModelState.IsValid) return RedirectToAction ("Index");
       var presupuesto = new Presupuesto(presupuestoVM);
       repoPresupuestos.CrearPresupuesto(presupuesto);
       return RedirectToAction ("Index");

    }

    [HttpGet]

    public IActionResult AgregarProductoAPresupuesto(int id)
    {
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

    [HttpPost]

    public IActionResult AgregarProductoEnPresupuesto(AgregarProductoAPresuViewModel infoProducto)
    {
        if(!ModelState.IsValid) return RedirectToAction ("Index");
        repoPresupuestos.AgregarProducto(infoProducto.IdPresupuesto, infoProducto.IdProducto, infoProducto.Cantidad);
        return RedirectToAction ("Index");
    }
    
    [HttpGet]

    public IActionResult EliminarProductoAPresupuesto(int id)
    {
        Presupuesto presupuesto = repoPresupuestos.ObtenerPresupuestoPorId(id);
        ViewData["Productos"] = presupuesto.Detalle.Select(p => new SelectListItem
        {
            Value = p.Producto.IdProducto.ToString(), 
            Text = p.Producto.Descripcion 
        }).ToList();

        return View(id);
    }

    [HttpPost]

    public IActionResult EliminarProductoEnPresupuesto(int idPresupuesto, int idProducto)
    {
        repoPresupuestos.EliminarProducto(idPresupuesto, idProducto);
        return RedirectToAction ("Index");
    }

    [HttpGet]
    public IActionResult ModificarPresupuesto(int id)
    {
        List<Cliente> Clientes = repoClientes.ObtenerClientes();
        ViewData["Clientes"] =  Clientes.Select(c=> new SelectListItem
        {
            Value = c.ClienteId.ToString(), 
            Text = c.Nombre
        }).ToList();
        var presupuesto  = repoPresupuestos.ObtenerPresupuestoPorId(id);
        var presupuestoVM = new ModificarPresupuestoViewModel();
        presupuestoVM.IdPresupuesto = id;
        presupuestoVM.FechaCreacion = presupuesto.FechaCreacion;
        return View(presupuestoVM);
    }

    [HttpPost]
    public IActionResult ModificarPresupuesto(ModificarPresupuestoViewModel presupuestoVM)
    {
        if(!ModelState.IsValid) return RedirectToAction ("Index");
        var presupuesto = new Presupuesto(presupuestoVM);
        repoPresupuestos.ModificarPresupuesto(presupuesto);
        return RedirectToAction ("Index"); 

    }
    

    [HttpGet]

    public IActionResult EliminarPresupuesto(int id)
    {
        return View(repoPresupuestos.ObtenerPresupuestoPorId(id));
    }

    [HttpGet]
    public IActionResult EliminarPresupuestoPorId(int id)
    {
        repoPresupuestos.EliminarPresupuestoPorId(id);
        return RedirectToAction ("Index"); 
    }
}