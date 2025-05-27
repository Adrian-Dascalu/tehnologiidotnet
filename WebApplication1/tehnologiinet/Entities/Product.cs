using System.Collections;

namespace tehnologiinet.Entities;

public class Product
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public long Price { get; set; }
    public long RecomendedPrice { get; set; }
    public long ReducedPrice { get; set; }
    public string? ImageUrl { get; set; }
    public int Stock { get; set; }
    public string Specification { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    
    public ICollection<Review> Reviews { get; set; }
    public Manufacturer Manufacturer { get; set; }
    public ICollection<Category> Categories { get; set; }
}