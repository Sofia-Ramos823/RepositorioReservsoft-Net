using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using reservsoft.DATA;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace reservsoft.Views.Auth
{
    public class LoginModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }

        public LoginModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                ErrorMessage = "Debe completar todos los campos.";
                return Page();
            }

            var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == Email);

            if (user == null || user.Password != Password)
            {
                ErrorMessage = "Credenciales incorrectas.";
                return Page();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Role ?? "User")
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

            return RedirectToPage("/Home/Index");
        }
    }
}
