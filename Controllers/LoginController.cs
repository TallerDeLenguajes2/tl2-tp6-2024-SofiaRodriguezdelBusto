
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MiWebApi.Controllers;

public class LoginController : Controller
{
    private readonly IUserRepository _userRepository;

    private readonly ILogger<LoginController> _logger;


    public LoginController(IUserRepository userRepository, ILogger<LoginController> logger)
    {
        _logger = logger;
        _userRepository = userRepository;
    }

    public IActionResult Index()
    {
        try
        {
            var model = new LoginViewModel
            {
                IsAuthenticated = HttpContext.Session.GetString("IsAuthenticated") == "true"
            };
            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            ViewBag.ErrorMessage = "No se pudo cargar la página";
            return View("Index");
            
        }


    }
    
    public IActionResult Login(LoginViewModel model)
    {
        try
        {
            if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
            {
                model.ErrorMessage = "Por favor ingrese su nombre de usuario y contraseña.";
                return View("Index", model);
            }
            User usuario = _userRepository.GetUser(model.Username, model.Password);
            if(usuario != null)
            {
                HttpContext.Session.SetString("IsAuthenticated", "true");
                HttpContext.Session.SetString("User", usuario.Username);
                HttpContext.Session.SetString("AccessLevel", usuario.AccessLevel.ToString());
                _logger.LogInformation("El usuario: "+ usuario.Username+" ingresó correctamente");
                return RedirectToAction("Index", "Home");
            }
            _logger.LogWarning("Intento de acceso invalido - Usuario: "+ usuario.Username + "Clave ingresada: "+ usuario.Password);
            model.ErrorMessage = "Credenciales Inválidas";
            model.IsAuthenticated = false;
            return View("Index", model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            ViewBag.ErrorMessage = "No se puso autenticar el usuario";
            return View("Index", model);
        }

    }

    public IActionResult Logout()
    {
        try
        {
            // Limpiar la sesión
            HttpContext.Session.Clear();

            // Redirigir a la vista de login
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            
            _logger.LogError(ex.ToString());
            ViewBag.ErrorMessage = "No se pudo desloguear el usuario";
            return View("Index");
        }
    }
    [HttpGet]

    public IActionResult CrearUsuario()
    {
        try
        {
            return View();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            ViewBag.ErrorMessage = "No se pudo ejecutar la accion requerida";
            return View("Index");
        }
    }

    [HttpPost]

    public IActionResult AltaUsuario(CrearUsuarioViewModel usuarioVM)
    {
        try
        {            
            if(!ModelState.IsValid) return RedirectToAction ("CrearUsuario");
            User usuario = new User(usuarioVM);
            _userRepository.AltaUsuario(usuario);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            ViewBag.ErrorMessage = "No se puso autenticar el usuario";
            return View("Index");
        }
    }
}