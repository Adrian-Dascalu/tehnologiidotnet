using System.ComponentModel.DataAnnotations;
namespace tehnologiinet.Entities;

public class Factorio
{
    [Key]
    public long Id { get; set; }
    [Required]
    public string Item { get; set; }
    [Required]
    public double Value { get; set; }
    [Required]
    public string Unit { get; set; }
    [Required]
    public string Time { get; set; }
}