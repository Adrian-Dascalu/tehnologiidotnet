using Microsoft.AspNetCore.Mvc;
using tehnologiinet.Repositories;

[Route("TicTacToe")]
[ApiController]
public class GamesController : ControllerBase
{
    private readonly IStudentsRepository _studentsRepository;

    // Injecting repository via constructor
    public GamesController(IStudentsRepository studentsRepository)
    {
        _studentsRepository = studentsRepository;
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
        if (result == null)
            return BadRequest("Invalid data.");

        var student = _studentsRepository.GetStudentById(1);
        if (student == null)
            return NotFound("Student not found.");

        if (result.Won)
            student.Losses += 1;
        else
            student.Wins += 1;

        _studentsRepository.UpdateStudent(student);

        return Ok(new { message = "Updated successfully", student });
    }

    // DTO to match the expected JSON body
    public class MatchResult
    {
        public int StudentId { get; set; }
        public bool Won { get; set; }
    }
}