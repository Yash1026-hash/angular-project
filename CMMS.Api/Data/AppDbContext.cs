
using CMMS.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CMMS.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    // Database tables
    public DbSet<SparePart> SpareParts { get; set; }
    public DbSet<PmSchedule> PmSchedules { get; set; }
    public DbSet<WorkOrder> WorkOrders { get; set; }
    public DbSet<WoPartsUsed> WoPartsUsed { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<Asset> Assets { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User table configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);

            entity.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(150);

            entity.HasIndex(u => u.Email)
                .IsUnique();

            entity.Property(u => u.PasswordHash)
                .IsRequired();

            entity.Property(u => u.Role)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(u => u.IsActive)
                .HasDefaultValue(true);

            entity.Property(u => u.CreatedAt)
                .IsRequired();
        });


        // Asset table configuration
        modelBuilder.Entity<Asset>(entity =>
        {
            entity.HasKey(a => a.Id);

            entity.Property(a => a.AssetCode)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasIndex(a => a.AssetCode)
                .IsUnique();

            entity.Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(a => a.Location)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(a => a.Criticality)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(a => a.Status)
                .IsRequired()
                .HasMaxLength(30)
                .HasDefaultValue("Running");

            entity.Property(a => a.IsActive)
                .HasDefaultValue(true);

            entity.Property(a => a.CreatedAt)
                .IsRequired();
        });
    }
}
