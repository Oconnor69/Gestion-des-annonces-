using System;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_LW.Data;
using Web_LW.Entities;
using Web_LW.Models;

namespace Web_LW.Controllers;

public class UserDashboardController : Controller
{
    private readonly AppDbContext _context;

    public UserDashboardController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var userIdValue = HttpContext.Session.GetString("UserId");

        if (string.IsNullOrWhiteSpace(userIdValue) || !int.TryParse(userIdValue, out var userId))
        {
            return RedirectToAction("Login", "Auth");
        }

        var userName = HttpContext.Session.GetString("Nom") ?? "Utilisateur";

        var annoncesQuery = _context.Annonces
            .Where(a => a.UtilisateurId == userId);

        var total = await annoncesQuery.CountAsync();

        var statusCounts = await annoncesQuery
            .GroupBy(a => string.IsNullOrWhiteSpace(a.Statut) ? "Active" : a.Statut)
            .Select(g => new { Label = g.Key, Count = g.Count() })
            .ToListAsync();

        var activeCount = statusCounts
            .FirstOrDefault(x => x.Label.Equals("Active", StringComparison.OrdinalIgnoreCase))?.Count ?? 0;
        var inactiveCount = statusCounts
            .Where(x => !x.Label.Equals("Active", StringComparison.OrdinalIgnoreCase))
            .Sum(x => x.Count);

        var model = new UserDashboardViewModel
        {
            UserName = userName,
            TotalAnnonces = total,
            ActiveAnnonces = activeCount,
            InactiveAnnonces = inactiveCount
        };

        var startMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-5);

        var monthlyAnnonces = await annoncesQuery
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

        var categories = await annoncesQuery
            .Include(a => a.Categorie)
            .GroupBy(a => a.Categorie != null ? a.Categorie.Nom : "Sans categorie")
            .Select(g => new { Label = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(6)
            .ToListAsync();

        foreach (var category in categories)
        {
            model.CategoryLabels.Add(category.Label);
            model.CategoryCounts.Add(category.Count);
        }

        return View(model);
    }
}
