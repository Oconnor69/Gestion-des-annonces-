using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
namespace Web_LW.Controllers;
using Web_LW.Models;

public class UtilisateursController : Controller
{
    private readonly AppDbContext _context;

    public UtilisateursController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Liste
    public async Task<IActionResult> Index()
    {
        return View(await _context.Utilisateurs.ToListAsync());
    }

    // GET: Details
    public async Task<IActionResult> Details(int id)
    {
        var user = await _context.Utilisateurs.FindAsync(id);
        if (user == null) return NotFound();
        return View(user);
    }

    // GET: Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Create
    [HttpPost]
    public async Task<IActionResult> Create(Utilisateur u)
    {
        try
        {
            // Hasher le mot de passe avec BCrypt
            u.MotDePasse = BCrypt.Net.BCrypt.HashPassword(u.MotDePasse);
            
            u.Annonces = new List<Annonce>();
            u.Commentaires = new List<Commentaire>();
            u.MessagesEnvoyes = new List<Message>();
            u.MessagesRecus = new List<Message>();

            _context.Utilisateurs.Add(u);
            await _context.SaveChangesAsync();
            
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", $"Erreur: {ex.Message}");
            return View(u);
        }
    }

    // GET: Edit
    public async Task<IActionResult> Edit(int id)
    {
        var user = await _context.Utilisateurs.FindAsync(id);
        if (user == null) return NotFound();
        
        // Ne pas afficher le mot de passe hashé
        user.MotDePasse = "";
        return View(user);
    }

    // POST: Edit
    [HttpPost]
    public async Task<IActionResult> Edit(Utilisateur u)
    {
        try
        {
            var existingUser = await _context.Utilisateurs.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == u.Id);
            
            if (existingUser == null)
            {
                return NotFound();
            }

            // Si le mot de passe est vide, garder l'ancien
            if (string.IsNullOrEmpty(u.MotDePasse))
            {
                u.MotDePasse = existingUser.MotDePasse;
            }
            else
            {
                // Hasher le nouveau mot de passe
                u.MotDePasse = BCrypt.Net.BCrypt.HashPassword(u.MotDePasse);
            }

            _context.Update(u);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", $"Erreur: {ex.Message}");
            return View(u);
        }
    }

    // GET: Delete - Affiche la confirmation
    [HttpGet]
    [Route("Utilisateurs/Delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await _context.Utilisateurs.FindAsync(id);
        if (user == null) return NotFound();
        return View(user);
    }

    // POST: Delete - Effectue la suppression
    [HttpPost]
    [Route("Utilisateurs/Delete/{id}")]
    public async Task<IActionResult> DeletePost(int id)
    {
        try
        {
            var user = await _context.Utilisateurs.FindAsync(id);
            if (user != null)
            {
                _context.Utilisateurs.Remove(user);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            return RedirectToAction("Index");
        }
    }

    // Méthode utilitaire pour vérifier le mot de passe (pour la connexion future)
    public bool VerifyPassword(string hashedPassword, string providedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword);
    }
}
