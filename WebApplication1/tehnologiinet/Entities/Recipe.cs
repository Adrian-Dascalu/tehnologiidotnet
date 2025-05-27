using System.ComponentModel.DataAnnotations;
namespace tehnologiinet.Entities;

public class Recipe
{
    [Key]
    public long Id { get; set; }
    public long ItemId { get; set; }
    public Item Item { get; set; }
    public long Amount { get; set; } // amount of the item produced by this recipe
    public ICollection<Ingredient> Ingredients { get; set; }
}
