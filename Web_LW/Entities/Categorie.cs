using System.Collections.Generic;

namespace Web_LW.Entities
{
    public class Categorie
    {
        public int Id { get; set; }
        public string Nom { get; set; }

        public List<Annonce> Annonces { get; set; } = new();
    }
}
