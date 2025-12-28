namespace Web_LW.Models
{
    public class Categorie
    {
        public int Id { get; set; }
        public string Nom { get; set; }

        public List<Annonce>? Annonces { get; set; }
    }
}
