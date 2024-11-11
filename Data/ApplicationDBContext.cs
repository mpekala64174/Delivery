using Delivery.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.Xml;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Magazyn> Magazyn { get; set; }
    public DbSet<Uzytkownicy> Uzytkownicy { get; set; }
    public DbSet<AutomatPaczkowy> AutomatPaczkowy { get; set; }
    public DbSet<Transport> Transport { get; set; }
    public DbSet<PaczkiTransport> PaczkiTransport { get; set; }
    public DbSet<PaczkiAutomat> PaczkiAutomat { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Primary Key Configuration
        modelBuilder.Entity<Magazyn>()
            .HasKey(m => m.ID_paczki);

        modelBuilder.Entity<Uzytkownicy>()
            .HasKey(u => u.ID_uzytkownika);

        modelBuilder.Entity<AutomatPaczkowy>()
            .HasKey(a => a.ID_automat);

        modelBuilder.Entity<Transport>()
            .HasKey(t => t.ID_transport);

        modelBuilder.Entity<PaczkiTransport>()
            .HasKey(pt => pt.ID_paczki_transport);

        modelBuilder.Entity<PaczkiAutomat>()
            .HasKey(pa => pa.ID_paczki_automat);

        // Relationships
        modelBuilder.Entity<Transport>()
            .HasOne(t => t.Uzytkownik)
            .WithMany()
            .HasForeignKey(t => t.ID_uzytkownika);

        modelBuilder.Entity<PaczkiTransport>()
            .HasOne(pt => pt.Paczka)
            .WithMany()
            .HasForeignKey(pt => pt.ID_paczki);

        modelBuilder.Entity<PaczkiTransport>()
            .HasOne(pt => pt.Transport)
            .WithMany()
            .HasForeignKey(pt => pt.ID_transport);

        modelBuilder.Entity<PaczkiAutomat>()
            .HasOne(pa => pa.PaczkiTransport)
            .WithMany()
            .HasForeignKey(pa => pa.ID_paczki_transport);

        modelBuilder.Entity<PaczkiAutomat>()
            .HasOne(pa => pa.AutomatPaczkowy)
            .WithMany()
            .HasForeignKey(pa => pa.ID_automat);

        modelBuilder.Entity<AutomatPaczkowy>().ToTable("automat_paczkowy");
        modelBuilder.Entity<PaczkiAutomat>().ToTable("paczki_automat");
        modelBuilder.Entity<PaczkiTransport>().ToTable("paczki_transport");

        base.OnModelCreating(modelBuilder);
    }


}