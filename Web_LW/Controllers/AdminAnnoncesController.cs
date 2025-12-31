using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_LW.Models;

namespace Web_LW.Controllers;

public class AdminAnnoncesController : Controller
{
    private readonly AppDbContext _context;

    public AdminAnnoncesController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var annonces = await _context.Annonces
            .Include(a => a.Utilisateur)
            .Include(a => a.Categorie)
            .OrderByDescending(a => a.DatePublication)
            .Select(a => new AdminAnnonceRow
            {
                Id = a.Id,
                Titre = a.Titre,
                Utilisateur = a.Utilisateur != null ? a.Utilisateur.Nom : "Inconnu",
                Categorie = a.Categorie != null ? a.Categorie.Nom : "Sans categorie",
                DatePublication = a.DatePublication,
                Statut = string.IsNullOrWhiteSpace(a.Statut) ? "Active" : a.Statut
            })
            .ToListAsync();

        return View(annonces);
    }

    public async Task<IActionResult> Details(int id)
    {
        var annonce = await _context.Annonces
            .Include(a => a.Utilisateur)
            .Include(a => a.Categorie)
            .Include(a => a.Photos)
            .Include(a => a.Voiture)
            .Include(a => a.Immobilier)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (annonce == null)
        {
            TempData["AdminMessage"] = "Annonce introuvable.";
            return RedirectToAction(nameof(Index));
        }

        return View(annonce);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var annonce = await _context.Annonces
            .Include(a => a.Commentaires)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (annonce == null)
        {
            TempData["AdminMessage"] = "Annonce introuvable.";
            return RedirectToAction(nameof(Index));
        }

        if (annonce.Commentaires?.Any() == true)
        {
            _context.Commentaires.RemoveRange(annonce.Commentaires);
        }

        _context.Annonces.Remove(annonce);
        await _context.SaveChangesAsync();

        TempData["AdminMessage"] = "Annonce supprimee.";
        return RedirectToAction(nameof(Index));
    }
}
