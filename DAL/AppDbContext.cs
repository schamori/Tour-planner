using Microsoft.EntityFrameworkCore;
using Models;
public class AppDbContext : DbContext
{
    public DbSet<TourLog> TourLogs { get; set; }
    public DbSet<Tour> Routes { get; set; }
    public AppDbContext() { }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=tour;Username=mpleyer;Password=admin;Include Error Detail=True;");

        }

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TourLog>().ToTable("TourLogs");
        modelBuilder.Entity<TourLog>().HasKey(t => t.Id);

        modelBuilder.Entity<Tour>().ToTable("Routes");
        modelBuilder.Entity<Tour>().HasKey(t => t.Id);

        modelBuilder.Entity<Tour>()
        .HasMany(r => r.TourLogs) // Route has many TourLogs
        .WithOne() // Each TourLog has one Route
        .HasForeignKey(t => t.TourId) // Foreign key in TourLog
        .OnDelete(DeleteBehavior.Cascade);
    }
    public void EnsureDatabase()
    {
        this.Database.EnsureCreated();
    }
}
