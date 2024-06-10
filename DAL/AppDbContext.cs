using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        string basePath = AppDomain.CurrentDomain.BaseDirectory;
        string folderPath = Path.Combine(basePath, "..\\..\\..\\..\\TourPlanner\\appsettings.json");
        IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile(folderPath, false, true) // add as content / copy-always
            .Build();

        var connectionString = config["ConnectionStrings:DefaultConnection"];

        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql(connectionString);

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
