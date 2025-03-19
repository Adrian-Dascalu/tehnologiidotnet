using Microsoft.AspNetCore.Mvc;

namespace tehnologiinet.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class FacultyController: ControllerBase
{
    [HttpGet]
    public IActionResult GetFaculties()
    {
        using (var db = new DatabaseContext())
        {
            var faculties = db.Faculties.ToList();
            return Ok(faculties);
        }
    }
}