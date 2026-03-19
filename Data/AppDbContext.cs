using Microsoft.EntityFrameworkCore;
using WalletCorp.API.Models;

namespace WalletCorp.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Company> Companies => Set<Company>();
    public DbSet<User> Users => Set<User>();
    public DbSet<BenefitPlan> BenefitPlans => Set<BenefitPlan>();
    public DbSet<BenefitPlanCategory> BenefitPlanCategories => Set<BenefitPlanCategory>();
    public DbSet<Wallet> Wallets => Set<Wallet>();
    public DbSet<WalletCategory> WalletCategories => Set<WalletCategory>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Company>(entity =>
        {
            entity.ToTable("Companies");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).IsRequired().HasDefaultValue(string.Empty);
            entity.Property(e => e.Rfc).IsRequired().HasDefaultValue(string.Empty).HasColumnName("RFC");
            entity.Property(e => e.Email).IsRequired().HasDefaultValue(string.Empty);
            entity.Property(e => e.GlobalBalanceLimit).HasColumnType("numeric");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasColumnType("timestamptz").HasDefaultValueSql("NOW()");

            entity.HasMany(e => e.Users)
                .WithOne(e => e.Company)
                .HasForeignKey(e => e.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.BenefitPlans)
                .WithOne(e => e.Company)
                .HasForeignKey(e => e.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.FullName).IsRequired().HasDefaultValue(string.Empty);
            entity.Property(e => e.Email).IsRequired().HasDefaultValue(string.Empty);
            entity.Property(e => e.PasswordHash).IsRequired().HasDefaultValue(string.Empty);
            entity.Property(e => e.Role).IsRequired().HasDefaultValue(string.Empty);
            entity.Property(e => e.BenefitPlanId);
            entity.Property(e => e.CreatedAt).HasColumnType("timestamptz").HasDefaultValueSql("NOW()");

            entity.HasOne(e => e.Company)
                .WithMany(e => e.Users)
                .HasForeignKey(e => e.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.BenefitPlan)
                .WithMany(e => e.Users)
                .HasForeignKey(e => e.BenefitPlanId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(e => e.Wallet)
                .WithOne(e => e.User)
                .HasForeignKey<Wallet>(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.AuditLogs)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<BenefitPlan>(entity =>
        {
            entity.ToTable("BenefitPlans");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).IsRequired().HasDefaultValue(string.Empty);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasColumnType("timestamptz").HasDefaultValueSql("NOW()");

            entity.HasOne(e => e.Company)
                .WithMany(e => e.BenefitPlans)
                .HasForeignKey(e => e.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.Categories)
                .WithOne(e => e.BenefitPlan)
                .HasForeignKey(e => e.BenefitPlanId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<BenefitPlanCategory>(entity =>
        {
            entity.ToTable("BenefitPlanCategories");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.CategoryName).IsRequired().HasDefaultValue(string.Empty);
            entity.Property(e => e.Percentage).HasColumnType("numeric");
        });

        modelBuilder.Entity<Wallet>(entity =>
        {
            entity.ToTable("Wallets");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.TotalBalance).HasColumnType("numeric").HasDefaultValue(0m);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasColumnType("timestamptz").HasDefaultValueSql("NOW()");

            entity.HasIndex(e => e.UserId).IsUnique();

            entity.HasOne(e => e.User)
                .WithOne(e => e.Wallet)
                .HasForeignKey<Wallet>(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.Categories)
                .WithOne(e => e.Wallet)
                .HasForeignKey(e => e.WalletId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.Transactions)
                .WithOne(e => e.Wallet)
                .HasForeignKey(e => e.WalletId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<WalletCategory>(entity =>
        {
            entity.ToTable("WalletCategories");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.CategoryName).IsRequired().HasDefaultValue(string.Empty);
            entity.Property(e => e.Balance).HasColumnType("numeric").HasDefaultValue(0m);
            entity.Property(e => e.MonthlyLimit).HasColumnType("numeric").HasDefaultValue(0m);

            entity.HasOne(e => e.Wallet)
                .WithMany(e => e.Categories)
                .HasForeignKey(e => e.WalletId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.Transactions)
                .WithOne(e => e.WalletCategory)
                .HasForeignKey(e => e.WalletCategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.ToTable("Transactions");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Type).IsRequired().HasDefaultValue(string.Empty);
            entity.Property(e => e.Amount).HasColumnType("numeric");
            entity.Property(e => e.Description).IsRequired().HasDefaultValue(string.Empty);
            entity.Property(e => e.Status).IsRequired().HasDefaultValue(string.Empty);
            entity.Property(e => e.CreatedAt).HasColumnType("timestamptz").HasDefaultValueSql("NOW()");

            entity.HasOne(e => e.Wallet)
                .WithMany(e => e.Transactions)
                .HasForeignKey(e => e.WalletId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.WalletCategory)
                .WithMany(e => e.Transactions)
                .HasForeignKey(e => e.WalletCategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.ToTable("AuditLogs");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Action).IsRequired().HasDefaultValue(string.Empty);
            entity.Property(e => e.Entity).IsRequired().HasDefaultValue(string.Empty);
            entity.Property(e => e.Detail).IsRequired().HasDefaultValue(string.Empty);
            entity.Property(e => e.CreatedAt).HasColumnType("timestamptz").HasDefaultValueSql("NOW()");

            entity.HasOne(e => e.User)
                .WithMany(e => e.AuditLogs)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
