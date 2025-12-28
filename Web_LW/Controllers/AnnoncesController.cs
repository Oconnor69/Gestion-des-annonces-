using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_LW.Models;

public class AnnoncesController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _env;

    public AnnoncesController(AppDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    private int UserId()
    {
        var userIdStr = HttpContext.Session.GetString("UserId");

        if (string.IsNullOrEmpty(userIdStr))
            throw new Exception("Utilisateur non connecté (Session UserId vide)");

        if (!int.TryParse(userIdStr, out int userId))
            throw new Exception("UserId en session invalide");

        return userId;
    }

    // ===============================
    // CHOIX CATEGORIE
    // ===============================
    public IActionResult ChoisirCategorie()
    {
        return View();
    }

    // ===============================
    // CREATE VOITURE
    // ===============================
    public IActionResult CreateVoiture()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateVoiture(
        Annonce annonce,
        VoitureAnnonce voiture,
        List<IFormFile> photos)
    {
        annonce.UtilisateurId = UserId();
        annonce.CategorieId = 1;

        _context.Annonces.Add(annonce);
        await _context.SaveChangesAsync();

        voiture.AnnonceId = annonce.Id;
        _context.VoitureAnnonces.Add(voiture);

        if (photos != null && photos.Count > 0)
        {
            await SavePhotos(annonce.Id, photos);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction("MesAnnonces");
    }

    // ===============================
    // CREATE IMMOBILIER
    // ===============================
    public IActionResult CreateImmobilier()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateImmobilier(
        Annonce annonce,
        ImmobilierAnnonce immo,
        List<IFormFile> photos)
    {
        annonce.UtilisateurId = UserId();
        annonce.CategorieId = 2;

        _context.Annonces.Add(annonce);
        await _context.SaveChangesAsync();

        immo.AnnonceId = annonce.Id;
        _context.ImmobilierAnnonces.Add(immo);

        if (photos != null && photos.Count > 0)
        {
            await SavePhotos(annonce.Id, photos);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction("MesAnnonces");
    }

    // ===============================
    // MES ANNONCES
    // ===============================
    public async Task<IActionResult> MesAnnonces()
    {
        int uid = UserId();

        var annonces = await _context.Annonces
            .Include(a => a.Photos)
            .Include(a => a.Categorie)
            .Where(a => a.UtilisateurId == uid)
            .ToListAsync();

        return View(annonces);
    }

    // ===============================
    // DELETE (SECURISÉ)
    // ===============================
    public async Task<IActionResult> Delete(int id)
    {
        var annonce = await _context.Annonces.FindAsync(id);
        if (annonce == null || annonce.UtilisateurId != UserId())
            return Unauthorized();

        // Note: Les photos sur le disque ne sont pas supprimées ici, 
        // idéalement il faudrait boucler dessus pour nettoyer le dossier uploads.
        _context.Annonces.Remove(annonce);
        await _context.SaveChangesAsync();
        return RedirectToAction("MesAnnonces");
    }

    // ===============================
    // UPLOAD PHOTOS
    // ===============================
    private async Task SavePhotos(int annonceId, List<IFormFile> photos)
    {
        var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");

        if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

        foreach (var photo in photos)
        {
            if (photo.Length > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(photo.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await photo.CopyToAsync(stream);
                }

                _context.AnnoncePhotos.Add(new AnnoncePhoto
                {
                    AnnonceId = annonceId,
                    Url = "/uploads/" + fileName
                });
            }
        }
    }

    // ===============================
    // DETAILS (Voir l'annonce)
    // ===============================
    public async Task<IActionResult> Details(int id)
    {
        var annonce = await _context.Annonces
            .Include(a => a.Photos)
            .Include(a => a.Voiture)
            .Include(a => a.Immobilier) // CORRECTION : J'ai décommenté cette ligne
            .Include(a => a.Utilisateur)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (annonce == null)
        {
            return NotFound();
        }

        return View(annonce);
    }

    // ===============================
    // EDIT (Afficher le formulaire)
    // ===============================
    public async Task<IActionResult> Edit(int id)
    {
        var annonce = await _context.Annonces
            .Include(a => a.Voiture)
            .Include(a => a.Immobilier) // CORRECTION : AJOUT INDISPENSABLE
            .Include(a => a.Photos)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (annonce == null) return NotFound();

        if (annonce.UtilisateurId != UserId()) return Unauthorized();

        return View(annonce);
    }

    // ===============================
    // EDIT (Enregistrer les modifs)
    // ===============================
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        Annonce annonceModif,
        VoitureAnnonce voitureModif,
        ImmobilierAnnonce immoModif, // CORRECTION : AJOUT DU PARAMETRE IMMO
        List<IFormFile> photos)
    {
        var annonceDb = await _context.Annonces
            .Include(a => a.Voiture)
            .Include(a => a.Immobilier) // CORRECTION : Charger l'immo existant
            .Include(a => a.Photos)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (annonceDb == null) return NotFound();

        if (annonceDb.UtilisateurId != UserId()) return Unauthorized();

        // Mise à jour des infos générales
        annonceDb.Titre = annonceModif.Titre;
        annonceDb.Description = annonceModif.Description;
        annonceDb.Prix = annonceModif.Prix;
        annonceDb.Ville = annonceModif.Ville;
        annonceDb.Statut = string.IsNullOrWhiteSpace(annonceModif.Statut) ? annonceDb.Statut : annonceModif.Statut;

        // Mise à jour VOITURE
        if (annonceDb.Voiture != null)
        {
            annonceDb.Voiture.Marque = voitureModif.Marque;
            annonceDb.Voiture.Modele = voitureModif.Modele;
            annonceDb.Voiture.Annee = voitureModif.Annee;
            annonceDb.Voiture.Kilometrage = voitureModif.Kilometrage;
            annonceDb.Voiture.Carburant = voitureModif.Carburant;
            annonceDb.Voiture.BoiteVitesse = voitureModif.BoiteVitesse;
            annonceDb.Voiture.Climatisation = voitureModif.Climatisation;
            annonceDb.Voiture.GPS = voitureModif.GPS;
            // Ajoute ici les autres champs voiture si nécessaire...
        }

        // CORRECTION : Mise à jour IMMOBILIER
        if (annonceDb.Immobilier != null)
        {
            annonceDb.Immobilier.TypeBien = immoModif.TypeBien;
            annonceDb.Immobilier.TypeTransaction = immoModif.TypeTransaction;
            annonceDb.Immobilier.Surface = immoModif.Surface;
            annonceDb.Immobilier.NombreChambres = immoModif.NombreChambres;
            annonceDb.Immobilier.NombreSallesBain = immoModif.NombreSallesBain;
            annonceDb.Immobilier.Etage = immoModif.Etage;
            annonceDb.Immobilier.NombreEtages = immoModif.NombreEtages;
            annonceDb.Immobilier.AnneeConstruction = immoModif.AnneeConstruction;
            annonceDb.Immobilier.Orientation = immoModif.Orientation;
            annonceDb.Immobilier.EtatBien = immoModif.EtatBien;

            // Checkboxes
            annonceDb.Immobilier.Ascenseur = immoModif.Ascenseur;
            annonceDb.Immobilier.Parking = immoModif.Parking;
            annonceDb.Immobilier.Jardin = immoModif.Jardin;
            annonceDb.Immobilier.Balcon = immoModif.Balcon;
            annonceDb.Immobilier.Meuble = immoModif.Meuble;

            // Location
            annonceDb.Immobilier.LoyerMensuel = immoModif.LoyerMensuel;
            annonceDb.Immobilier.Caution = immoModif.Caution;
        }

        // Ajout nouvelles photos
        if (photos != null && photos.Count > 0)
        {
            await SavePhotos(annonceDb.Id, photos);
        }

        try
        {
            _context.Update(annonceDb);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            throw;
        }

        return RedirectToAction(nameof(MesAnnonces));
    }

    // ===============================
    // DELETE PHOTO (AJAX ou Form)
    // ===============================
    [HttpPost]
    public async Task<IActionResult> DeletePhoto(int photoId)
    {
        var photo = await _context.AnnoncePhotos.FindAsync(photoId);
        if (photo == null) return NotFound();

        var annonce = await _context.Annonces.FindAsync(photo.AnnonceId);
        if (annonce == null || annonce.UtilisateurId != UserId()) return Unauthorized();

        var path = Path.Combine(_env.WebRootPath, photo.Url.TrimStart('/'));
        if (System.IO.File.Exists(path)) System.IO.File.Delete(path);

        _context.AnnoncePhotos.Remove(photo);
        await _context.SaveChangesAsync();

        return RedirectToAction("Edit", new { id = photo.AnnonceId });
    }
}