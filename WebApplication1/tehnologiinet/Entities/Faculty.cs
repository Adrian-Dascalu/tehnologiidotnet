namespace tehnologiinet.NewDirectory1;

public class Faculty
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string? Address { get; set; }
    
    public ICollection<Specialization> Specializations { get; set; }
}