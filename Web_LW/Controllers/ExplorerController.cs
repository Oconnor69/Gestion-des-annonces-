using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_LW.Models;

namespace Web_LW.Controllers
{
    public class ExplorerController : Controller
    {
        private readonly AppDbContext _context;

        public ExplorerController(AppDbContext context)
        {
            _context = context;
        }

        private int? TryGetUserId()
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (int.TryParse(userIdStr, out var uid)) return uid;
            return null;
        }

        public async Task<IActionResult> Index([FromQuery] AnnonceExplorerViewModel filters)
        {
            var query = _context.Annonces
                .Include(a => a.Categorie)
                .Include(a => a.Utilisateur)
                .Include(a => a.Photos)
                .AsQueryable();

            // Exclure les annonces vendues ou louees de l'exploration publique
            query = query.Where(a => a.Statut != "Vendue" && a.Statut != "Louee");

            if (!string.IsNullOrWhiteSpace(filters.Search))
            {
                var term = filters.Search.Trim();
                query = query.Where(a =>
                    EF.Functions.Like(a.Titre, $"%{term}%") ||
                    EF.Functions.Like(a.Description, $"%{term}%"));
            }

            if (filters.CategorieId.HasValue)
            {
                query = query.Where(a => a.CategorieId == filters.CategorieId.Value);
            }

            if (!string.IsNullOrWhiteSpace(filters.Ville))
            {
                var ville = filters.Ville.Trim();
                query = query.Where(a => EF.Functions.Like(a.Ville, $"%{ville}%"));
            }

            if (filters.PrixMin.HasValue)
            {
                query = query.Where(a => a.Prix >= filters.PrixMin.Value);
            }

            if (filters.PrixMax.HasValue)
            {
                query = query.Where(a => a.Prix <= filters.PrixMax.Value);
            }

            var annonces = await query
                .OrderByDescending(a => a.DatePublication)
                .ToListAsync();

            filters.Categories = await _context.Categories
                .OrderBy(c => c.Nom)
                .ToListAsync();

            filters.Annonces = annonces;

            return View(filters);
        }

        public async Task<IActionResult> Details(int id)
        {
            var annonce = await _context.Annonces
                .Include(a => a.Categorie)
                .Include(a => a.Utilisateur)
                .Include(a => a.Photos)
                .Include(a => a.Voiture)
                .Include(a => a.Immobilier)
                .Include(a => a.Commentaires)
                    .ThenInclude(c => c.Utilisateur)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (annonce == null)
            {
                return NotFound();
            }

            ViewBag.CurrentUserId = TryGetUserId();
            return View(annonce);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(int annonceId, string contenu)
        {
            var uid = TryGetUserId();
            if (uid == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (string.IsNullOrWhiteSpace(contenu))
            {
                TempData["CommentError"] = "Le commentaire est vide.";
                return RedirectToAction(nameof(Details), new { id = annonceId });
            }

            var annonce = await _context.Annonces.FindAsync(annonceId);
            if (annonce == null) return NotFound();

            _context.Commentaires.Add(new Commentaire
            {
                AnnonceId = annonceId,
                UtilisateurId = uid.Value,
                Contenu = contenu.Trim(),
                DateCommentaire = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = annonceId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditComment(int id, string contenu)
        {
            var uid = TryGetUserId();
            if (uid == null) return RedirectToAction("Login", "Auth");

            if (string.IsNullOrWhiteSpace(contenu))
            {
                TempData["CommentError"] = "Le commentaire est vide.";
                var commentAnnonceId = await _context.Commentaires.Where(c => c.Id == id).Select(c => c.AnnonceId).FirstOrDefaultAsync();
                return RedirectToAction(nameof(Details), new { id = commentAnnonceId });
            }

            var comment = await _context.Commentaires.FirstOrDefaultAsync(c => c.Id == id);
            if (comment == null) return NotFound();
            if (comment.UtilisateurId != uid.Value) return Unauthorized();

            comment.Contenu = contenu.Trim();
            comment.DateCommentaire = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = comment.AnnonceId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var uid = TryGetUserId();
            if (uid == null) return RedirectToAction("Login", "Auth");

            var comment = await _context.Commentaires.FirstOrDefaultAsync(c => c.Id == id);
            if (comment == null) return NotFound();
            if (comment.UtilisateurId != uid.Value) return Unauthorized();

            var annonceId = comment.AnnonceId;
            _context.Commentaires.Remove(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = annonceId });
        }
    }
}
