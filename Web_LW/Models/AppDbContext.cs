using Microsoft.EntityFrameworkCore;

namespace Web_LW.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Utilisateur> Utilisateurs { get; set; }
        public DbSet<Categorie> Categories { get; set; }

        public DbSet<VoitureAnnonce> VoitureAnnonces { get; set; }

        public DbSet<Annonce> Annonces { get; set; }
        public DbSet<AnnoncePhoto> AnnoncePhotos { get; set; }
        public DbSet<Commentaire> Commentaires { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<ImmobilierAnnonce> ImmobilierAnnonces { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Utilisateur - Annonce
            modelBuilder.Entity<Annonce>()
                .HasOne(a => a.Utilisateur)
                .WithMany(u => u.Annonces)
                .HasForeignKey(a => a.UtilisateurId)
                .OnDelete(DeleteBehavior.Cascade);

            // Annonce - Photos
            modelBuilder.Entity<Annonce>()
                .HasMany(a => a.Photos)
                .WithOne(p => p.Annonce)
                .HasForeignKey(p => p.AnnonceId)
                .OnDelete(DeleteBehavior.Cascade);

            // Annonce - Commentaires
            modelBuilder.Entity<Annonce>()
                .HasMany(a => a.Commentaires)
                .WithOne(c => c.Annonce)
                .HasForeignKey(c => c.AnnonceId)
               .OnDelete(DeleteBehavior.NoAction);


            // Utilisateur - Commentaire
            modelBuilder.Entity<Commentaire>()
                .HasOne(c => c.Utilisateur)
                .WithMany(u => u.Commentaires)
                .HasForeignKey(c => c.UtilisateurId)
                .OnDelete(DeleteBehavior.Cascade);

            // Messages (Expéditeur)
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Expediteur)
                .WithMany(u => u.MessagesEnvoyes)
                .HasForeignKey(m => m.ExpediteurId)
                .OnDelete(DeleteBehavior.NoAction);

            // Messages (Destinataire)
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Destinataire)
                .WithMany(u => u.MessagesRecus)
                .HasForeignKey(m => m.DestinataireId)
                .OnDelete(DeleteBehavior.NoAction);

            // Annonce - VoitureAnnonce (1–1)
            modelBuilder.Entity<Annonce>()
                .HasOne(a => a.Voiture)
                .WithOne(v => v.Annonce)
                .HasForeignKey<VoitureAnnonce>(v => v.AnnonceId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Annonce>()
            .HasOne(a => a.Immobilier)
            .WithOne(i => i.Annonce)
            .HasForeignKey<ImmobilierAnnonce>(i => i.AnnonceId)
            .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
