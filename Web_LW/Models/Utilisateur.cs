using Web_LW.Models;

namespace Web_LW.Models
{
    public class Utilisateur
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string MotDePasse { get; set; } = string.Empty;  // HASH, jamais en clair
        public string Telephone { get; set; } = string.Empty;
        public string Role { get; set; } = "User"; // "Admin" or "User"
        public bool IsActive { get; set; } = true;

        public List<Annonce> Annonces { get; set; } = new();
        public List<Commentaire> Commentaires { get; set; } = new();
        public List<Message> MessagesEnvoyes { get; set; } = new();
        public List<Message> MessagesRecus { get; set; } = new();
    }
}
