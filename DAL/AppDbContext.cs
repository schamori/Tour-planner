using Microsoft.EntityFrameworkCore;
using Models;
public class AppDbContext : DbContext
{
    public DbSet<TourLog> TourLogs { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
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
