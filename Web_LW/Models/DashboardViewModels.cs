using System.Collections.Generic;

namespace Web_LW.Models
{
    public class AdminDashboardViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalCategories { get; set; }
        public int TotalAnnonces { get; set; }

        public List<string> MonthLabels { get; set; } = new();
        public List<int> MonthCounts { get; set; } = new();

        public List<string> CategoryLabels { get; set; } = new();
        public List<int> CategoryCounts { get; set; } = new();

        public List<string> RoleLabels { get; set; } = new();
        public List<int> RoleCounts { get; set; } = new();

        public List<AdminAnnonceRow> Annonces { get; set; } = new();
    }

    public class AdminAnnonceRow
    {
        public int Id { get; set; }
        public string Titre { get; set; } = string.Empty;
        public string Utilisateur { get; set; } = string.Empty;
        public string Categorie { get; set; } = string.Empty;
        public DateTime DatePublication { get; set; }
        public string Statut { get; set; } = "Active";
    }

    public class UserDashboardViewModel
    {
        public string UserName { get; set; } = "Utilisateur";

        public int TotalAnnonces { get; set; }
        public int ActiveAnnonces { get; set; }
        public int InactiveAnnonces { get; set; }

        public List<string> MonthLabels { get; set; } = new();
        public List<int> MonthCounts { get; set; } = new();

        public List<string> CategoryLabels { get; set; } = new();
        public List<int> CategoryCounts { get; set; } = new();
    }
}
