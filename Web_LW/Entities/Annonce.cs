using System;
using System.Collections.Generic;

namespace Web_LW.Entities
{
    public class Annonce
    {
        public int Id { get; set; }

        // Infos générales
        public string Titre { get; set; }
        public string Description { get; set; }
        public decimal Prix { get; set; }
        public string Ville { get; set; }
        public DateTime DatePublication { get; set; } = DateTime.Now;
        public string Statut { get; set; } = "Active"; // Active / Vendue / Louée

        // Relations
        public int UtilisateurId { get; set; }
        public Utilisateur Utilisateur { get; set; }

        public int CategorieId { get; set; }
        public Categorie Categorie { get; set; }

        public List<AnnoncePhoto> Photos { get; set; } = new();
        public List<Commentaire> Commentaires { get; set; } = new();

        // Détails spécialisés
        public VoitureAnnonce? Voiture { get; set; }
        public ImmobilierAnnonce? Immobilier { get; set; }
    }
}
