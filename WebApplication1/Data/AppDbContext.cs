using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Goal> Goals { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurar User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.HasIndex(e => e.CreatedAt);
                
                // Email único será validado em application level
                entity.Property(e => e.Email).IsRequired();
            });

            // Configurar Category
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.HasIndex(e => e.Tipo);
                
                // Nome único será validado em application level
                entity.Property(e => e.Nome).IsRequired();
            });

            // Configurar Goal
            modelBuilder.Entity<Goal>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.HasIndex(e => e.IdUser);
                entity.HasIndex(e => e.Tipo);

                // Relação com User
                entity.HasOne(g => g.User)
                    .WithMany(u => u.Goals)
                    .HasForeignKey(g => g.IdUser)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configurar Transaction
            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                // Relação com User
                entity.HasOne(t => t.User)
                    .WithMany(u => u.Transactions)
                    .HasForeignKey(t => t.IdUser)
                    .OnDelete(DeleteBehavior.Cascade);

                // Relação com Category
                entity.HasOne(t => t.Category)
                    .WithMany(c => c.Transactions)
                    .HasForeignKey(t => t.IdCategory)
                    .OnDelete(DeleteBehavior.Cascade);

                // Relação com Goal (opcional)
                entity.HasOne(t => t.Goal)
                    .WithMany(g => g.Transactions)
                    .HasForeignKey(t => t.IdGoal)
                    .OnDelete(DeleteBehavior.SetNull);
            });
        }
    }
}
