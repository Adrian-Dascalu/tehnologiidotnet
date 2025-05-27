using System.ComponentModel.DataAnnotations;
namespace tehnologiinet.Entities;

public class Construction
{
    [Key]
    public long Id { get; set; }
    public string Name { get; set; }
    public long ItemId { get; set; }
    public double TotalQuantity { get; set; }
    // public ICollection<Consumption> Consumptions { get; set; } = new List<Consumption>();
    // public ICollection<Production> Productions { get; set; } = new List<Production>();
}