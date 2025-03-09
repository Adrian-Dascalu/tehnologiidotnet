using Microsoft.AspNetCore.Mvc;
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