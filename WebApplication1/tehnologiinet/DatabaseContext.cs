using Microsoft.EntityFrameworkCore;
using tehnologiinet.Entities;

namespace tehnologiinet;

public class DatabaseContext : DbContext
{
    public DatabaseContext()
    {

    }
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {

    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Host=localhost;Database=factorio;Username=postgres;Password=postgres");


    public DbSet<Student> Students { get; set; }
    public DbSet<Faculty> Faculties { get; set; }
    public DbSet<Specialization> Specializations { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Production> Productions { get; set; }
    public DbSet<Consumption> Consumptions { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<Construction> Constructions { get; set; }
}
