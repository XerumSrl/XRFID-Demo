using Microsoft.EntityFrameworkCore;
using XRFID.Server.Demo.V2.Entities;

namespace XRFID.Server.Demo.V2.Database;

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
    public DbSet<Location> Locations { get; set; }
    public DbSet<Printer> Printers { get; set; }
    public DbSet<RawTagHistory> RawTagHistory { get; set; }

    public XRFIDSampleContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
#if DEBUG
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.EnableDetailedErrors();
#endif
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {


        modelBuilder.SetupAuditEntity<Label>();

        modelBuilder.SetupAuditEntity<LoadingUnit>();
        modelBuilder.Entity<LoadingUnit>().HasOne(ho => ho.Reader).WithMany(wm => wm.LoadingUnits).HasForeignKey(fk => fk.ReaderId).OnDelete(DeleteBehavior.NoAction).IsRequired();

        modelBuilder.SetupAuditEntity<LoadingUnitItem>();
        modelBuilder.Entity<LoadingUnitItem>().HasOne(ho => ho.LoadingUnit).WithMany(wm => wm.LoadingUnitItems).HasForeignKey(fk => fk.LoadingUnitId).OnDelete(DeleteBehavior.Cascade).IsRequired();

        modelBuilder.SetupAuditEntity<Location>();
        modelBuilder.Entity<Location>().HasIndex(i => i.Name).IsUnique();
        modelBuilder.Entity<Location>().HasOne(o => o.ParentLocation).WithMany(o => o.ChildLocations).HasForeignKey(fk => fk.ParentLocationId).IsRequired(false).OnDelete(DeleteBehavior.NoAction);


        modelBuilder.SetupAuditEntity<Movement>();
        modelBuilder.Entity<Movement>().HasOne(ho => ho.Reader).WithMany(wm => wm.Movements).HasForeignKey(fk => fk.ReaderId).OnDelete(DeleteBehavior.Cascade).IsRequired(false);
        modelBuilder.Entity<Movement>().HasOne(ho => ho.Printer).WithMany(wm => wm.Movements).HasForeignKey(fk => fk.PrinterId).OnDelete(DeleteBehavior.Cascade).IsRequired(false);

        modelBuilder.SetupAuditEntity<MovementItem>();
        modelBuilder.Entity<MovementItem>().HasOne(ho => ho.Movement).WithMany(wm => wm.MovementItems).HasForeignKey(fk => fk.MovementId).OnDelete(DeleteBehavior.Cascade).IsRequired();
        modelBuilder.Entity<MovementItem>().HasOne(ho => ho.LoadingUnitItem).WithMany(wm => wm.MovementItems).HasForeignKey(fk => fk.LoadingUnitItemId).IsRequired(false);
        modelBuilder.Entity<MovementItem>().HasOne(ho => ho.Product).WithMany(wm => wm.MovementItems).HasForeignKey(fk => fk.ProductId).IsRequired(false);

        modelBuilder.SetupAuditEntity<Printer>();

        modelBuilder.SetupAuditEntity<Product>();
        modelBuilder.Entity<Product>().HasIndex(ak => ak.Epc).IsUnique();
        modelBuilder.Entity<Product>().HasOne(ho => ho.Sku).WithMany(wm => wm.Products).HasForeignKey(fk => fk.SkuId).IsRequired(false);

        modelBuilder.SetupAuditEntity<Reader>();
        modelBuilder.Entity<Reader>()
            .OwnsOne(
            o => o.ReaderConfiguration,
            onb =>
            {
                onb.ToJson();
                onb.OwnsOne(o => o.WorkflowSettings, onb1 => onb1.OwnsOne(o => o.TagFoundFilter));
                onb.OwnsOne(o => o.ModeSpecificSettings,
                    onb1 => onb1.OwnsOne(o => o.AdvancedConfig)
                        .OwnsMany(o => o.Aars));
                onb.OwnsOne(o => o.ModeSpecificSettings,
                    onb1 => onb1.OwnsOne(o => o.BasicConfig));
            });
        modelBuilder.Entity<Reader>()
            .OwnsOne(
            o => o.GPIOConfiguration,
            onb =>
            {
                onb.ToJson();
                onb.OwnsOne(o => o.GPInSwitch);
                onb.OwnsOne(o => o.GPInTrigger);
                onb.OwnsOne(o => o.GPInCustom);
                onb.OwnsOne(o => o.GPOutBuzzer);
                onb.OwnsOne(o => o.GPOutYellowLED);
                onb.OwnsOne(o => o.GPOutRedLED);
                onb.OwnsOne(o => o.GPOutGreenLED);
            });

        modelBuilder.SetupEntity<RawTagHistory>();

        modelBuilder.SetupAuditEntity<Sku>();

        base.OnModelCreating(modelBuilder);
    }
}
