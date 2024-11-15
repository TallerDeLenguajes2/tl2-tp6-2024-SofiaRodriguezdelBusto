
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MiWebApi.Controllers;

public class LoginController : Controller
{
    private readonly IUserRepository _inMemoryUserRepository;

    private readonly ILogger<LoginController> _logger;


    public LoginController(IUserRepository inMemoryUserRepository)
    {
        _inMemoryUserRepository = inMemoryUserRepository;
    }

    public IActionResult Index()
    {
        var model = new LoginViewModel
        {
            IsAuthenticated = HttpContext.Session.GetString("IsAuthenticated") == "true"
        };
        return View(model);
    }
    
    public IActionResult Login(LoginViewModel model)
    {
        if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
        {
            model.ErrorMessage = "Por favor ingrese su nombre de usuario y contraseña.";
            return View("Index", model);
        }
        User usuario = _inMemoryUserRepository.GetUser(model.Username, model.Password);
        if(usuario != null)
        {
            HttpContext.Session.SetString("IsAuthenticated", "true");
            HttpContext.Session.SetString("User", usuario.Username);
            HttpContext.Session.SetString("AccessLevel", usuario.AccessLevel.ToString());
            return RedirectToAction("Index", "Home");
        }
        model.ErrorMessage = "Credenciales Inválidas";
        model.IsAuthenticated = false;
        return View("Index", model);

    }

    public IActionResult Logout()
    {
        // Limpiar la sesión
        HttpContext.Session.Clear();

        // Redirigir a la vista de login
        return RedirectToAction("Index");
    }
}