using System;

namespace Web_LW.Models
{
    public class Commentaire
    {
        public int Id { get; set; }
        public string Contenu { get; set; }
        public DateTime DateCommentaire { get; set; } = DateTime.UtcNow;

        public int AnnonceId { get; set; }
        public Annonce Annonce { get; set; }

        public int UtilisateurId { get; set; }
        public Utilisateur Utilisateur { get; set; }
    }
}
