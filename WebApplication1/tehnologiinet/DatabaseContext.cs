using Microsoft.EntityFrameworkCore;
using tehnologiinet.NewDirectory1;

namespace tehnologiinet;

public class DatabaseContext: DbContext
{
    public DatabaseContext()
    {
        
    }
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
        
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Host=localhost;Database=tehnologiinet;Username=postgres;Password=parkingshare");
    
    
    public DbSet<Student> Students { get; set; }
    public DbSet<Faculty> Faculties { get; set; }
    public DbSet<Specialization> Specializations { get; set; }
}
