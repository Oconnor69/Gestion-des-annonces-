namespace Web_LW.Entities
{
    public class ImmobilierAnnonce
    {
        public int Id { get; set; }

        public string TypeBien { get; set; } // Appartement, Maison, Villa, Terrain
        public string TypeTransaction { get; set; } // Vente / Location

        public double Surface { get; set; }
        public int NombreChambres { get; set; }
        public int NombreSallesBain { get; set; }
        public int Etage { get; set; }
        public int NombreEtages { get; set; }

        public bool Ascenseur { get; set; }
        public bool Parking { get; set; }
        public bool Jardin { get; set; }
        public bool Balcon { get; set; }
        public bool Meuble { get; set; }

        public int AnneeConstruction { get; set; }
        public string Orientation { get; set; } // Nord, Sud, Est, Ouest
        public string EtatBien { get; set; } // Neuf, Bon état, À rénover

        // Location seulement
        public decimal? LoyerMensuel { get; set; }
        public decimal? Caution { get; set; }

        // Relation
        public int AnnonceId { get; set; }
        public Annonce Annonce { get; set; }
    }
}
