using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using tehnologiinet.Repositories;

[Route("TicTacToe")]
[ApiController]
[EnableCors]
public class GamesController : ControllerBase
{
    private readonly IStudentsRepository _studentsRepository;

    // connect to database
    private readonly AppDbContext _context;

    // Injecting repository via constructor
    public GamesController(IStudentsRepository studentsRepository, AppDbContext context)
    {
        _studentsRepository = studentsRepository;
        _context = context;
    }

    [HttpGet]
    public IActionResult GetGamePage()
    {
        // Serve the firstpage.html file from wwwroot
        return PhysicalFile(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/tictactoe", "firstpage.html"), "text/html");
    }

    [HttpPost("update-result")]
    public IActionResult UpdateResult([FromBody] MatchResult result)
    {
        Console.WriteLine("Received update-result request");

        var student = _studentsRepository.GetStudentByUsername(result.Username!);
        
        //if (student == null)
        //{
            //return NotFound("Student not found");
        //}
        

        if (result.Win == 1)
        {
            //student.Wins++;

            // increase the win in the database User table
            var user = _context.Users.FirstOrDefault(u => u.UserName == result.Username);
            if (user != null)
            {
                user.Wins++;
                _context.SaveChanges();
            }
            else
            {
                //else add user by username
                var newUser = new User { Name = result.Username, UserName = result.Username, Loses = 0, Wins = 1 };
                var userRepository = new UserRepository(_context);
                userRepository.AddUser(newUser);
                _context.SaveChanges();
            }
        }
        else if (result.Lose == 1)
        {
            //student.Losses++;

            // increase the lose in the database User table
            var user = _context.Users.FirstOrDefault(u => u.UserName == result.Username);
            if (user != null)
            {
                user.Loses++;
                _context.SaveChanges();
            }
            else
            {
                //else add user by username
                var newUser = new User { Name = result.Username, UserName = result.Username, Loses = 1, Wins = 0 };
                var userRepository = new UserRepository(_context);
                userRepository.AddUser(newUser);
                _context.SaveChanges();
            }
        }

        //_studentsRepository.UpdateStudent(student); // Ensure this updates the data source

        return Ok(User);
    }

    // DTO to match the expected JSON body
    public class MatchResult
    {
        public string? Username { get; set; }
        public int Win { get; set; }
        public int Lose { get; set; }
    }
}