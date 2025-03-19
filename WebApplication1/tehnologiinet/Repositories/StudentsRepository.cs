using tehnologiinet.NewDirectory1;

namespace tehnologiinet.Repositories;

public class StudentsRepository: IStudentsRepository
{

    public List<Student> GetAllStudents()
    {
        return GetAllStudentsFromDb();
    }

    public Student GetStudentById(long Id)
    {
        return GetAllStudentsFromDb().FirstOrDefault(x => x.Id == Id);
    }

    public List<Student> FilterStudentsBySpecialization(string specialization)
    {
        return new List<Student>();
    }

    public List<Student> FilterStudentsByFaculty(string faculty)
    {
        return new List<Student>();
    }
    
    
     private List<Student> GetAllStudentsFromDb()
     {
         using (var db = new DatabaseContext())
         {
             return db.Students.ToList();
         }
     }
}