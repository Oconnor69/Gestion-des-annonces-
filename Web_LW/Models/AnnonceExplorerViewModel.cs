using System.Collections.Generic;
using Web_LW.Entities;

namespace Web_LW.Models
{
    public class AnnonceExplorerViewModel
    {
        public string? Search { get; set; }
        public int? CategorieId { get; set; }
        public string? Ville { get; set; }
        public decimal? PrixMin { get; set; }
        public decimal? PrixMax { get; set; }
        public string? Statut { get; set; }

        public List<Categorie> Categories { get; set; } = new();
        public List<Annonce> Annonces { get; set; } = new();
    }
}
