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
       return GetAllStudentsFromDb().Where(x => x.Specialization == specialization).ToList();
    }

    public List<Student> FilterStudentsByFaculty(string faculty)
    {
        return GetAllStudentsFromDb().Where(x => x.Faculty == faculty).ToList();
    }
    
    
     private List<Student> GetAllStudentsFromDb()
        {
           List<Student> studentsRetrieved = new List<Student>();
    
           var student1 = new Student
           {
               Id = 1,
               FirstName = "John",
               LastName = "Doe",
               CNP = "1234567890123",
               Email = "student@student.ro",
               PhoneNumber = "0722222222",
               Address = "Str. Studentilor",
               City = "Iasi",
               Country = "Romania",
               PostalCode = "700000",
               University = "Universitatea Tehnica",
               Faculty = "Automatica si Calculatoare",
               Specialization = "Calculatoare",
           };
           
           var student2 = new Student
           {
               Id = 2,
               FirstName = "Jane",
               LastName = "Doe",
               CNP = "1234567890123",
               Email = "jane.doe@student.ucv.ro",
               PhoneNumber = "0000000000",
               Address = "Str. Studentilor nr. 1",
               City = "Craiova",
               Country = "Romania",
               PostalCode = "700000",
               University = "Universitatea Tehnica",
               Faculty = "Automatica si Calculatoare",
               Specialization = "Calculatoare",
           };
           
           var student3 = new Student
           {
               Id = 3,
               FirstName = "Jane",
               LastName = "Popescu",
               CNP = "1234567890123",
               Email = "jane.popescu@student.ucv.ro",
               PhoneNumber = "0000000000",
               Address = "Str. Studentilor nr. 3",
               City = "Craiova",
               Country = "Romania",
               PostalCode = "700000",
               University = "Universitatea Tehnica",
               Faculty = "Automatica si Calculatoare",
               Specialization = "Automatica si Informatica Aplicata",
           };
           
           
            studentsRetrieved.Add(student1);
            studentsRetrieved.Add(student2); 
            studentsRetrieved.Add(student3);
            return studentsRetrieved;
        }
}