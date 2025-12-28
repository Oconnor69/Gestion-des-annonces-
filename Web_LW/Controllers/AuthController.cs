using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Web_LW.Models;
using BCrypt.Net;

namespace Web_LW.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        // PAGE LOGIN
        public IActionResult Login()
        {
            return View();
        }

        // TRAITEMENT LOGIN
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            if (!ModelState.IsValid)
                return View(login);

            // 1) Récupérer l’utilisateur par email
            var user = await _context.Utilisateurs
                .FirstOrDefaultAsync(u => u.Email == login.Email);

            if (user == null)
            {
                ViewBag.Error = "Email ou mot de passe incorrect.";
                return View(login);
            }

            // 2) Vérification BCRYPT
            bool mdpCorrect = BCrypt.Net.BCrypt.Verify(login.MotDePasse, user.MotDePasse);

            if (!mdpCorrect)
            {
                ViewBag.Error = "Email ou mot de passe incorrect.";
                return View(login);
            }

            // 3) Créer la session
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("Role", user.Role);
            HttpContext.Session.SetString("Nom", user.Nom);
            HttpContext.Session.SetString("UserId", user.Id.ToString());
            HttpContext.Session.SetString("Role", user.Role);
            HttpContext.Session.SetString("Nom", user.Nom);


            // 4) Redirection selon rôle
            if (user.Role == "Admin")
                return RedirectToAction("Index", "AdminDashboard");

            return RedirectToAction("Index", "UserDashboard");
        }

        // LOGOUT
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
