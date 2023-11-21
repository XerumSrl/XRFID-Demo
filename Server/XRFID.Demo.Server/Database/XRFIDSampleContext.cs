using Microsoft.EntityFrameworkCore;
using XRFID.Demo.Server.Entities;

namespace XRFID.Demo.Server.Database;

public class XRFIDSampleContext : DbContext
{
    public DbSet<LoadingUnit> LoadingUnits { get; set; }
    public DbSet<LoadingUnitItem> LoadingUnitItems { get; set; }

    public DbSet<Movement> Movements { get; set; }
    public DbSet<MovementItem> MovementItems { get; set; }

    public DbSet<Sku> Skus { get; set; }
    public DbSet<Product> Products { get; set; }

    public DbSet<Reader> Readers { get; set; }
    public DbSet<Label> Labels { get; set; }
    public DbSet<Printer> Printers { get; set; }

    public XRFIDSampleContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
#if DEBUG
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.EnableDetailedErrors();
#endif
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LoadingUnit>().HasKey(k => k.Id);
        modelBuilder.Entity<LoadingUnit>().HasOne(ho => ho.Reader).WithMany(wm => wm.LoadingUnits).HasForeignKey(fk => fk.ReaderId).OnDelete(DeleteBehavior.NoAction).IsRequired();
        modelBuilder.Entity<LoadingUnitItem>().HasKey(k => k.Id);
        modelBuilder.Entity<LoadingUnitItem>().HasOne(ho => ho.LoadingUnit).WithMany(wm => wm.LoadingUnitItems).HasForeignKey(fk => fk.LoadingUnitId).OnDelete(DeleteBehavior.Cascade).IsRequired();
        modelBuilder.Entity<Movement>().HasKey(k => k.Id);
        modelBuilder.Entity<Movement>().HasOne(ho => ho.Reader).WithMany(wm => wm.Movements).HasForeignKey(fk => fk.ReaderId).OnDelete(DeleteBehavior.Cascade).IsRequired();
        modelBuilder.Entity<MovementItem>().HasKey(k => k.Id);
        modelBuilder.Entity<MovementItem>().HasOne(ho => ho.Movement).WithMany(wm => wm.MovementItems).HasForeignKey(fk => fk.MovementId).OnDelete(DeleteBehavior.Cascade).IsRequired();
        modelBuilder.Entity<Product>().HasKey(k => k.Id);
        modelBuilder.Entity<Product>().HasIndex(ak => ak.Epc).IsUnique();
        modelBuilder.Entity<Product>().HasIndex(ak => ak.Name).IsUnique();
        modelBuilder.Entity<Product>().HasIndex(ak => ak.Code).IsUnique();
        modelBuilder.Entity<Product>().HasOne(ho => ho.Sku).WithMany(wm => wm.Products).HasForeignKey(fk => fk.SkuId).OnDelete(DeleteBehavior.Cascade).IsRequired();
        modelBuilder.Entity<Reader>().HasKey(k => k.Id);
        modelBuilder.Entity<Sku>().HasKey(k => k.Id);
        modelBuilder.Entity<Sku>().HasIndex(ak => ak.Name).IsUnique();
        modelBuilder.Entity<Sku>().HasIndex(ak => ak.Code).IsUnique();
        modelBuilder.Entity<Printer>().HasKey(k => k.Id);
        modelBuilder.Entity<Label>().HasKey(k => k.Id);

        base.OnModelCreating(modelBuilder);
    }
}
