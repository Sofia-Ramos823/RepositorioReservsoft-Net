using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using reservsoft.DATA;
using reservsoft.Models;
using reservsoft.Utils;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace reservsoft.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        public async Task<IActionResult> Login(Usuario model)
        {
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
            {
                ViewBag.Error = "Por favor ingrese todos los campos.";
                return View();
            }

            var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == model.Email);

            if (user == null || !PasswordHasher.VerifyHashedPassword(user.Password, model.Password))
            {
                ViewBag.Error = "Credenciales incorrectas.";
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Role ?? "User")
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Index", "Home");
        }

        // GET: Logout
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Auth");
        }

        // GET: Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: Register
        [HttpPost]
        public async Task<IActionResult> Register(Usuario model, string confirmPassword)
        {
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password) || string.IsNullOrEmpty(confirmPassword))
            {
                ViewBag.Error = "Por favor ingrese todos los campos.";
                return View();
            }

            if (model.Password != confirmPassword)
            {
                ViewBag.Error = "Las contraseñas no coinciden.";
                return View();
            }

            if (await _context.Usuarios.AnyAsync(u => u.Email == model.Email))
            {
                ViewBag.Error = "El correo electrónico ya está registrado.";
                return View();
            }

            var newUser = new Usuario
            {
                Nombre = model.Nombre,
                Email = model.Email,
                Password = PasswordHasher.HashPassword(model.Password),
                Role = "User"
            };

            _context.Usuarios.Add(newUser);
            await _context.SaveChangesAsync();

            return RedirectToAction("Login", "Auth");
        }
    }
}