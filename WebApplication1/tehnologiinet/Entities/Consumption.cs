using System.ComponentModel.DataAnnotations;
namespace tehnologiinet.Entities;

public class Consumption
{
    [Key]
    public long Id { get; set; }
    public string Name { get; set; }
    public Item Item { get; set; }
    public long ItemId { get; set; }
    public double TotalQuantity { get; set; }
}