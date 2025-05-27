namespace tehnologiinet.Entities;

public class Review
{
    public long Id { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    
    public Product Product { get; set; }
   // public ICollection<User> Users { get; set; }
    
}