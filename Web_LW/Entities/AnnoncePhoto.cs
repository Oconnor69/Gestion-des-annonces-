namespace Web_LW.Entities
{
    public class AnnoncePhoto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool IsPrimary { get; set; }

        public int AnnonceId { get; set; }
        public Annonce Annonce { get; set; }
    }
}
