using BlueDragon.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace BlueDragon.Data;

public partial class HccContext : IdentityDbContext<ApplicationUser>
{
    public HccContext()
    {
    }

    public HccContext(DbContextOptions<HccContext> options)
        : base(options)
    {
        // TEST MAKING SURE CONNECTION TO SQL SERVER IS CORRECT
        var connStr = options.FindExtension<RelationalOptionsExtension>()?.ConnectionString;
        Console.WriteLine($"Connection String: {connStr}");
    }

    public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; }

    public virtual DbSet<Cable> Cables { get; set; }

    public virtual DbSet<Ecomponent> Ecomponents { get; set; }

    public virtual DbSet<Peripheral> Peripherals { get; set; }

    public virtual DbSet<Hardware> Hardwares { get; set; }

    public virtual DbSet<LuBrandName> LuBrandNames { get; set; }

    public virtual DbSet<LuCableType> LuCableTypes { get; set; }

    public virtual DbSet<SolutionSetting> SolutionSettings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Cable>(entity =>
        {
            entity.HasKey(e => e.Cid);

            entity.Property(e => e.Cid)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("CID");
            entity.Property(e => e.BrandName).HasMaxLength(50);
            entity.Property(e => e.CableType).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Barcode).HasMaxLength(13);
        });

        modelBuilder.Entity<Ecomponent>(entity =>
        {
            entity.HasKey(e => e.Ecid);

            entity.ToTable("EComponents");

            entity.Property(e => e.Ecid)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ECID");
            entity.Property(e => e.BrandName).HasMaxLength(50);
            entity.Property(e => e.Location).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.ModelNumber).HasMaxLength(50);
            entity.Property(e => e.SerialNumber).HasMaxLength(50);
            entity.Property(e => e.Barcode).HasMaxLength(13);
        });

        modelBuilder.Entity<Peripheral>(entity =>
        {
            entity.HasKey(e => e.Pcid);

            entity.ToTable("Peripherals");

            entity.Property(e => e.Pcid)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("PCID");
            entity.Property(e => e.BrandName).HasMaxLength(50);
            entity.Property(e => e.Location).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.ModelNumber).HasMaxLength(50);
            entity.Property(e => e.SerialNumber).HasMaxLength(50);
            entity.Property(e => e.Barcode).HasMaxLength(13);
        });

        modelBuilder.Entity<Hardware>(entity =>
        {
            entity.HasKey(e => e.Hid);

            entity.ToTable("Hardware");

            entity.Property(e => e.Hid)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("HID");
            entity.Property(e => e.BrandName).HasMaxLength(50);
            entity.Property(e => e.Location).HasMaxLength(50);
            entity.Property(e => e.ModelNumber).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.SerialNumber).HasMaxLength(50);
            entity.Property(e => e.Barcode).HasMaxLength(13);
        });

        modelBuilder.Entity<LuBrandName>(entity =>
        {
            entity.ToTable("luBrandNames");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<LuCableType>(entity =>
        {
            entity.ToTable("luCableTypes");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<SolutionSetting>(entity =>
        {
            entity.HasKey(e => e.Sid);

            entity.ToTable("SolutionSettings");

            entity.Property(e => e.Sid)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("SID");

            entity.Property(e => e.Name).HasMaxLength(50);

            entity.Property(e => e.IsEnabled).HasDefaultValueSql("((0))");
        });

        OnModelCreatingPartial(modelBuilder);
    }


    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
