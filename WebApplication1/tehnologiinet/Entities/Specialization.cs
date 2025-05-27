namespace tehnologiinet.Entities;

public class Specialization
{
    public long Id { get; set; }
    public string Name { get; set; }
    public int MaxStudents { get; set; }
    
    public Faculty Faculty { get; set; }
}