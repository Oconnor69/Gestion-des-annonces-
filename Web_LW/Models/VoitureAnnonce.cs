namespace Web_LW.Models
{
    public class VoitureAnnonce
    {
        public int Id { get; set; }

        public string Marque { get; set; }
        public string Modele { get; set; }
        public int Annee { get; set; }
        public int Kilometrage { get; set; }

        public string Carburant { get; set; } // Essence, Diesel, Hybride, Electrique
        public string BoiteVitesse { get; set; } // Manuelle, Automatique
        public string Couleur { get; set; }

        public int Puissance { get; set; } // CV
        public int NombrePortes { get; set; }

        public bool Climatisation { get; set; }
        public bool GPS { get; set; }
        public bool CameraRecul { get; set; }
        public bool Bluetooth { get; set; }
        public bool ABS { get; set; }
        public bool Airbags { get; set; }

        public string Etat { get; set; } // Neuf / Occasion
        public string Origine { get; set; } // Importé / Local

        // Relation
        public int AnnonceId { get; set; }
        public Annonce Annonce { get; set; }
    }

}
