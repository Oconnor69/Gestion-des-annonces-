using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_LW.Models;

namespace Web_LW.Controllers;

public class AdminDashboardController : Controller
{
    private readonly AppDbContext _context;

    public AdminDashboardController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var model = new AdminDashboardViewModel
        {
            TotalUsers = await _context.Utilisateurs.CountAsync(),
            TotalCategories = await _context.Categories.CountAsync(),
            TotalAnnonces = await _context.Annonces.CountAsync()
        };

        var startMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-5);

        var monthlyAnnonces = await _context.Annonces
            .Where(a => a.DatePublication >= startMonth)
            .GroupBy(a => new { a.DatePublication.Year, a.DatePublication.Month })
            .Select(g => new { g.Key.Year, g.Key.Month, Count = g.Count() })
            .ToListAsync();

        for (var i = 0; i < 6; i++)
        {
            var monthCursor = startMonth.AddMonths(i);
            var count = monthlyAnnonces
                .FirstOrDefault(x => x.Year == monthCursor.Year && x.Month == monthCursor.Month)?.Count ?? 0;

            model.MonthLabels.Add(monthCursor.ToString("MMM yyyy", CultureInfo.InvariantCulture));
            model.MonthCounts.Add(count);
        }

        var categories = await _context.Annonces
            .Include(a => a.Categorie)
            .GroupBy(a => a.Categorie != null ? a.Categorie.Nom : "Sans categorie")
            .Select(g => new { Label = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(8)
            .ToListAsync();

        foreach (var category in categories)
        {
            model.CategoryLabels.Add(category.Label);
            model.CategoryCounts.Add(category.Count);
        }

        var roles = await _context.Utilisateurs
            .GroupBy(u => string.IsNullOrWhiteSpace(u.Role) ? "User" : u.Role)
            .Select(g => new { Label = g.Key, Count = g.Count() })
            .ToListAsync();

        foreach (var role in roles)
        {
            model.RoleLabels.Add(role.Label);
            model.RoleCounts.Add(role.Count);
        }

        model.Annonces = await _context.Annonces
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

        return View(model);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteAnnonce(int id)
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
