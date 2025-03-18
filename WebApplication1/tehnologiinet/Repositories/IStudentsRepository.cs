using tehnologiinet.NewDirectory1;

namespace tehnologiinet.Repositories;

public interface IStudentsRepository
{
    List<Student> GetAllStudents();
    Student GetStudentById(long Id);
    Student GetStudentByUsername(string username);
    List<Student> FilterStudentsBySpecialization(string specialization);
    List<Student> FilterStudentsByFaculty(string faculty);
    void UpdateStudent(Student updatedStudent);
}