using Microsoft.EntityFrameworkCore;
using Models;
public class AppDbContext : DbContext
{
    public DbSet<TourLog> TourLogs { get; set; }
    public DbSet<Route> Routes { get; set; }
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

        modelBuilder.Entity<Route>().ToTable("Routes");
        modelBuilder.Entity<Route>().HasKey(t => t.Id);

        modelBuilder.Entity<TourLog>()
        .HasOne<Route>() // Specify the principal entity type but not the navigation property
        .WithMany(r => r.TourLogs) // Specify the collection navigation property in Route
        .HasForeignKey(t => t.TourId) // Define the foreign key in the dependent entity
        .OnDelete(DeleteBehavior.Cascade);
    }
    public void EnsureDatabase()
    {
        this.Database.EnsureCreated();
    }
}
