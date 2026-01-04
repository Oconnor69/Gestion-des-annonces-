using System;

namespace Web_LW.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public string Contenu { get; set; }
        public DateTime DateEnvoi { get; set; } = DateTime.UtcNow;

        public int ExpediteurId { get; set; }
        public Utilisateur Expediteur { get; set; }

        public int DestinataireId { get; set; }
        public Utilisateur Destinataire { get; set; }

        public bool IsRead { get; set; } = false;
    }
}
