using Microsoft.EntityFrameworkCore;
using Models;
public class AppDbContext : DbContext
{
    public DbSet<TourLog> TourLogs { get; set; }

    public AppDbContext() { }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=tour;Username=mpleyer;Password=admin");
        }

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TourLog>().ToTable("TourLogs");
        modelBuilder.Entity<TourLog>().HasKey(t => t.Id);
    }
    public void EnsureDatabase()
    {
        this.Database.EnsureCreated();
    }
}
