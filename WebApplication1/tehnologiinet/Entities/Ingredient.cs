using System.ComponentModel.DataAnnotations;
namespace tehnologiinet.Entities;

public class Ingredient
{
    [Key]
    public long Id { get; set; }
    
    public long ItemId { get; set; }
    public Item Item { get; set; }
    
    public long RecipeId { get; set; }
    public Recipe Recipe { get; set; }
    
    public int Amount { get; set; }
}