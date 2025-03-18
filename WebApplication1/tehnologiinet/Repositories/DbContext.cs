using Microsoft.EntityFrameworkCore;

namespace tehnologiinet.Repositories;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
        //add john to database
        //var user = new User { Name = "John", UserName =  "john_doe" , Loses = 0, Wins = 0};
        //var userRepository = new UserRepository(this);
        //userRepository.AddUser(user);
    }
    
    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseNpgsql("Host=localhost;Database=postgres;Username=postgres;Password=postgres");
    }

    public class Post
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
    
    public DbSet<Post> Posts { get; set; }
    
}

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string UserName { get; set; }
    public int Wins { get; set; }
    public int Loses { get; set; }
}

//add user to database
public class UserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public void AddUser(User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
    }
}