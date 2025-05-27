using System.ComponentModel.DataAnnotations;
namespace tehnologiinet.Entities;

public class Item
{
    [Key]
    public long Id { get; set; }
    public string Name { get; set; }
    public double Value { get; set; } //test // price is higher if the item is not in stock
    
    //public Recipe Recipe { get; set; } //   
    //public long? RecipeId { get; set; } //
}
