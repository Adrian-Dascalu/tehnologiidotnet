using tehnologiinet.NewDirectory1;
using System.Text.Json;

namespace tehnologiinet.Repositories;

public class StudentsRepository: IStudentsRepository
{
    public List<Student> GetAllStudents()
    {
        return GetAllStudentsFromDb();
    }

    public Student GetStudentById(long Id)
    {
        return GetAllStudentsFromDb().FirstOrDefault(x => x.Id == Id)!;
    }

    public Student GetStudentByUsername(string username)
    {
        return GetAllStudentsFromDb().FirstOrDefault(x => x.Username == username)!;
    }

    public List<Student> FilterStudentsBySpecialization(string specialization)
    {
       return GetAllStudentsFromDb().Where(x => x.Specialization == specialization).ToList();
    }

    public List<Student> FilterStudentsByFaculty(string faculty)
    {
        return GetAllStudentsFromDb().Where(x => x.Faculty == faculty).ToList();
    }

    private void SaveStudentsToJson(List<Student> students)
    {
        var json = JsonSerializer.Serialize(students, new JsonSerializerOptions { WriteIndented = true });

        
        File.WriteAllText("students.json", json);
    }

    public void UpdateStudent(Student updatedStudent)
    {
        var students = LoadStudentsFromJson();
        var student = students.FirstOrDefault(s => s.Id == updatedStudent.Id);

        if (student != null)
        {
            student.Wins = updatedStudent.Wins;
            student.Losses = updatedStudent.Losses;
            SaveStudentsToJson(students); // ✅ Save changes to file
        }
    }

    private List<Student> LoadStudentsFromJson()
    {
        if (!File.Exists("students.json"))
        {
            return new List<Student>(); // Return empty list if file does not exist
        }

        var json = File.ReadAllText("students.json");
        return JsonSerializer.Deserialize<List<Student>>(json) ?? new List<Student>();
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
               Wins = 0,
               Losses = 0,
               Username = "john_doe",
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
               Wins = 0,
               Losses = 0,
               Username = "jane_doe",
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
               Wins = 0,
               Losses = 0,
               Username = "jane_popescu",
           };
           
            studentsRetrieved.Add(student1);
            studentsRetrieved.Add(student2); 
            studentsRetrieved.Add(student3);

            if (!File.Exists("students.json"))
                SaveStudentsToJson(studentsRetrieved);
            else
            {
                studentsRetrieved = LoadStudentsFromJson();
            }
            
            return studentsRetrieved;
        }
}