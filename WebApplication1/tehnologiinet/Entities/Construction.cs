using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace tehnologiinet.Entities;

public class Construction
{
    [Key]
    public long Id { get; set; }
    public string Name { get; set; }
    public long ItemId { get; set; }
    public string UserId { get; set; }
    public double Quantity { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    [ForeignKey("ItemId")]
    public virtual Item Item { get; set; }
    // public ICollection<Consumption> Consumptions { get; set; } = new List<Consumption>();
    // public ICollection<Production> Productions { get; set; } = new List<Production>();
}