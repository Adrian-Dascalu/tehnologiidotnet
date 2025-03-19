using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tehnologiinet.Models;
using tehnologiinet.NewDirectory1;
using tehnologiinet.Repositories;

namespace tehnologiinet.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class StudentController: ControllerBase
{
    private readonly IStudentsRepository _studentsRepository;

    public StudentController(IStudentsRepository studentsRepository)
    {
        _studentsRepository = studentsRepository;
    }


    [HttpPost]
    public IActionResult CreateStudent([FromBody] StudentDto model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Modelul nu este valid!");
        }

        var student = new Student()
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            Year = model.Year
        };
        
        
        using (var db = new DatabaseContext())
        {
            db.Students.Add(student);
            db.SaveChanges();
        }

        return Ok();
    }


    [HttpGet]
    public IActionResult GetStudentsTest()
    {
        using (var db = new DatabaseContext())
        {
            return Ok(
                db.Students
                    .Include
                        (x =>
                            x.Specialization)
                    .Select(y => new StudentDemo()
            {
                FirstName = y.FirstName,
                LastName = y.LastName,
                SpecializationName = y.Specialization.Name
            }).ToList());
        }
    }
    
    
    [HttpGet]
    public IActionResult GetStudents()
    {
        
        return Ok(_studentsRepository.GetAllStudents());
    }

    [HttpGet]
    public IActionResult GetStudent([FromQuery] long Id)
    {
        // query
        var student = _studentsRepository.GetStudentById(Id);

        if (student == null)
        {
            return NotFound();
        }
        return Ok(student);
    }
    
    
    [HttpGet]
    public IActionResult FilterStudentsBySpecialization([FromQuery] string specialization)
    {
        var students = _studentsRepository.FilterStudentsBySpecialization(specialization);
        if (students.Count == 0)
        {
            return NotFound();
        }
        return Ok(students);
    }

    [HttpGet]
    public IActionResult FilterStudentsByFaculty([FromQuery] string faculty)
    {
        var students = _studentsRepository.FilterStudentsByFaculty(faculty);
        if (students.Count == 0)
        {
            return NotFound();
        }
        return Ok(students);
    }
    
   
}