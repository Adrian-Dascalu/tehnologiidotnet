using System.Drawing.Printing;
using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using tehnologiinet.Repositories;
using tehnologiinet.Entities;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using tehnologiinet.Models;
using tehnologiinet.Services;

namespace tehnologiinet.Controllers;

[Route("Factorio")]
[ApiController]
[EnableCors]
[Authorize]
public class ConstructionController : ControllerBase
{
    private readonly IFactorioRepository _factorioRepository;
    private readonly ILogger<ConstructionController> _logger;
    private readonly DatabaseContext _context;
    private readonly IEmailService _emailService;

    public ConstructionController(
        IFactorioRepository factorioRepository, 
        DatabaseContext context,
        ILogger<ConstructionController> logger,
        IEmailService emailService)
    {
        _factorioRepository = factorioRepository;
        _context = context;
        _logger = logger;
        _emailService = emailService;
    }

    // Get the current user's construction list
    [HttpGet("GetConstructionList")]
    public async Task<ActionResult<IEnumerable<Construction>>> GetConstructionList()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User not authenticated");
        }

        var constructions = await _context.Constructions
            .Include(c => c.Item)
            .Where(c => c.UserId == userId && !c.IsCompleted)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();

        return Ok(constructions);
    }

    // Add an item to construction
    [HttpPost("AddToConstruction/{itemId}")]
    public async Task<IActionResult> AddToConstruction(long itemId, [FromQuery] double quantity = 1)
    {
        // Get the Authorization header
        var authHeader = Request.Headers["Authorization"].ToString();
        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            return Unauthorized("Invalid authorization header format. Expected: 'Bearer {token}'");
        }

        // Extract the token
        var token = authHeader.Substring("Bearer ".Length).Trim();
        if (string.IsNullOrEmpty(token))
        {
            return Unauthorized("Token is missing");
        }

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User not authenticated");
        }

        var item = await _context.Items.FindAsync(itemId);
        if (item == null)
        {
            return NotFound("Item not found");
        }

        // Check if the item is already in the user's construction list
        var existingConstruction = await _context.Constructions
            .FirstOrDefaultAsync(c => c.UserId == userId && c.ItemId == itemId && !c.IsCompleted);

        if (existingConstruction != null)
        {
            // Update quantity if item already exists
            existingConstruction.Quantity += quantity;
            existingConstruction.UpdatedAt = DateTime.UtcNow;
            _context.Constructions.Update(existingConstruction);
        }
        else
        {
            // Add new item to construction
            var construction = new Construction
            {
                ItemId = itemId,
                Name = item.Name,
                UserId = userId,
                Quantity = quantity,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsCompleted = false
            };
            _context.Constructions.Add(construction);
        }

        await _context.SaveChangesAsync();
        return Ok();
    }

    // Update quantity of an item in construction
    [HttpPut("UpdateConstructionQuantity/{constructionId}")]
    public async Task<IActionResult> UpdateConstructionQuantity(long constructionId, [FromQuery] double quantity)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User not authenticated");
        }

        var construction = await _context.Constructions
            .FirstOrDefaultAsync(c => c.Id == constructionId && c.UserId == userId && !c.IsCompleted);

        if (construction == null)
        {
            return NotFound("Construction item not found");
        }

        if (quantity <= 0)
        {
            // Remove item if quantity is 0 or negative
            _context.Constructions.Remove(construction);
        }
        else
        {
            construction.Quantity = quantity;
            construction.UpdatedAt = DateTime.UtcNow;
            _context.Constructions.Update(construction);
        }

        await _context.SaveChangesAsync();
        return Ok();
    }

    // Remove an item from construction
    [HttpDelete("RemoveFromConstruction/{constructionId}")]
    public async Task<IActionResult> RemoveFromConstruction(long constructionId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User not authenticated");
        }

        var construction = await _context.Constructions
            .FirstOrDefaultAsync(c => c.Id == constructionId && c.UserId == userId && !c.IsCompleted);

        if (construction == null)
        {
            return NotFound("Construction item not found");
        }

        _context.Constructions.Remove(construction);
        await _context.SaveChangesAsync();
        return Ok();
    }

    // Mark a construction as completed
    [HttpPut("CompleteConstruction/{constructionId}")]
    public async Task<IActionResult> CompleteConstruction(long constructionId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User not authenticated");
        }

        var construction = await _context.Constructions
            .Include(c => c.Item)
            .FirstOrDefaultAsync(c => c.Id == constructionId && c.UserId == userId && !c.IsCompleted);

        if (construction == null)
        {
            return NotFound("Construction item not found");
        }

        // Get production and consumption data for the item
        var production = await _context.Productions
            .FirstOrDefaultAsync(p => p.ItemId == construction.ItemId);
        var consumption = await _context.Consumptions
            .FirstOrDefaultAsync(c => c.ItemId == construction.ItemId);

        // Calculate available quantity (production - consumption)
        var productionQuantity = production?.TotalQuantity ?? 0;
        var consumptionQuantity = consumption?.TotalQuantity ?? 0;
        var availableQuantity = productionQuantity - consumptionQuantity;

        var totalCost = 0.0;
        var message = "";
        var status = "";

        if (availableQuantity < construction.Quantity)
        {
            // Not enough available, calculate crafting cost
            var recipe = await _context.Recipes
                .Include(r => r.Ingredients)
                .ThenInclude(i => i.Item)
                .FirstOrDefaultAsync(r => r.ItemId == construction.ItemId);

            if (recipe == null)
            {
                return BadRequest($"Not enough items available ({availableQuantity}/{construction.Quantity}) and no recipe found to craft {construction.Item.Name}.");
            }

            // Calculate how many times the recipe needs to run
            // Use Math.Ceiling to ensure enough items are crafted
            var recipeExecutions = Math.Ceiling(construction.Quantity / recipe.Amount);

            // Calculate total crafting cost
            foreach (var ingredient in recipe.Ingredients)
            {
                if (ingredient.Item != null)
                {
                    totalCost += ingredient.Item.Value * ingredient.Amount * (construction.Quantity - availableQuantity);
                }
            }

            totalCost += construction.Item.Value * availableQuantity;

            message = $"Not enough items available ({availableQuantity}/{construction.Quantity}). Crafted {construction.Quantity - availableQuantity} x {construction.Item.Name} for a total cost of {totalCost}.";
            status = "Crafted";

            // Format ingredients for email
            var ingredientsList = "Ingredients used for crafting:\n";
            foreach (var ingredient in recipe.Ingredients)
            {
                if (ingredient.Item != null)
                {
                    ingredientsList += $"{ingredient.Amount * (construction.Quantity - availableQuantity)} x {ingredient.Item.Name}\n";
                }
            }
            message += "\n" + ingredientsList;

            // Update consumption of ingredients
             foreach (var ingredient in recipe.Ingredients)
            {
                if (ingredient.Item != null)
                {   
                    var ingredientConsumption = await _context.Consumptions
                        .FirstOrDefaultAsync(c => c.ItemId == ingredient.ItemId);

                    if (ingredientConsumption == null)
                    {   
                        ingredientConsumption = new Consumption
                        {
                            ItemId = ingredient.ItemId,
                            Name = ingredient.Item.Name,
                            TotalQuantity = ingredient.Amount * (construction.Quantity / recipe.Amount)
                        };
                        _context.Consumptions.Add(ingredientConsumption);
                    }   
                    else
                    {   
                        ingredientConsumption.TotalQuantity += ingredient.Amount * (construction.Quantity / recipe.Amount);
                        _context.Consumptions.Update(ingredientConsumption);
                    }   
                }
            }
        }
        else
        {
            // Enough available, calculate cost based on item value
            totalCost = construction.Item.Value * construction.Quantity;
            message = $"Order for {construction.Quantity} x {construction.Item.Name} processed. Total cost: {totalCost}";
            status = "Processed";

            // Update consumption of the main item
            if (consumption == null)
            {
                consumption = new Consumption
                {
                    ItemId = construction.ItemId,
                    Name = construction.Item.Name,
                    TotalQuantity = construction.Quantity
                };
                _context.Consumptions.Add(consumption);
            }
            else
            {
                consumption.TotalQuantity += construction.Quantity;
                _context.Consumptions.Update(consumption);
            }
        }

        // Mark the construction as completed
        construction.IsCompleted = true;
        construction.UpdatedAt = DateTime.UtcNow;
        _context.Constructions.Update(construction);

        await _context.SaveChangesAsync();

        // Send email confirmation
        var subject = "Order Confirmation";
        var emailMessage = $"Your order for {construction.Quantity} x {construction.Item.Name} has been {status.ToLowerInvariant()}. {message}";
        var attachmentPath = "wwwroot/swagger-ui/a_generic_industrial-themed_character_holding_their_hand_in_an__OK__sign.png"; // Path to your image
        var recipientEmail = "valentincheznoiu@gmail.com"; // Replace with the recipient's email

        await _emailService.SendEmailAsync(recipientEmail, subject, emailMessage, attachmentPath);

        return Ok(new
        {
            Message = message,
            TotalCost = totalCost,
            Quantity = construction.Quantity,
            ItemName = construction.Item.Name,
            Status = status
        });
    }

    [HttpPut("CompleteAllConstructions")]
    public async Task<IActionResult> CompleteAllConstructions()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User not authenticated");
        }

        var constructions = await _context.Constructions
            .Include(c => c.Item)
            .Where(c => c.UserId == userId && !c.IsCompleted)
            .ToListAsync();

        if (!constructions.Any())
        {
            return NotFound("No constructions found to complete");
        }

        var results = new List<object>();
        var totalCost = 0.0;

        foreach (var construction in constructions)
        {
            // Get production and consumption data for the item
            var production = await _context.Productions
                .FirstOrDefaultAsync(p => p.ItemId == construction.ItemId);
            var consumption = await _context.Consumptions
                .FirstOrDefaultAsync(c => c.ItemId == construction.ItemId);

            // Calculate available quantity
            var productionQuantity = production?.TotalQuantity ?? 0;
            var consumptionQuantity = consumption?.TotalQuantity ?? 0;
            var availableQuantity = productionQuantity - consumptionQuantity;

            var itemCost = 0.0;
            var status = "";
            var message = "";

            if (availableQuantity < construction.Quantity)
            {
                // Not enough available, calculate crafting cost
                var recipe = await _context.Recipes
                    .Include(r => r.Ingredients)
                    .ThenInclude(i => i.Item)
                    .FirstOrDefaultAsync(r => r.ItemId == construction.ItemId);

                if (recipe == null)
                {
                    results.Add(new
                    {
                        ItemName = construction.Item.Name,
                        Status = "Failed",
                        Message = $"Not enough items available ({availableQuantity}/{construction.Quantity}) and no recipe found to craft."
                    });
                    continue;
                }

                // Calculate how many times the recipe needs to run
                var recipeExecutions = Math.Ceiling(construction.Quantity / recipe.Amount);

                // Calculate total crafting cost for this item
                foreach (var ingredient in recipe.Ingredients)
                {
                    if (ingredient.Item != null)
                    {
                        // Calculate cost for the quantity that needs crafting
                        itemCost += ingredient.Item.Value * ingredient.Amount * ((construction.Quantity - availableQuantity) / recipe.Amount);
                    }
                }

                // Add the value of the available quantity
                itemCost += construction.Item.Value * availableQuantity;

                var quantityCrafted = construction.Quantity - availableQuantity;
                status = "Crafted";
                message = $"Crafted {quantityCrafted} x {construction.Item.Name} for a cost of {itemCost}.";

                // Format ingredients for email
                var ingredientsList = "Ingredients used for crafting:\n";
                foreach (var ingredient in recipe.Ingredients)
                {
                    if (ingredient.Item != null)
                    {
                        ingredientsList += $"{ingredient.Amount * ((construction.Quantity - availableQuantity) / recipe.Amount)} x {ingredient.Item.Name}\n";
                    }
                }
                message += "\n" + ingredientsList;

                // Update consumption of ingredients
                foreach (var ingredient in recipe.Ingredients)
                {
                    if (ingredient.Item != null)
                    {
                        var ingredientConsumption = await _context.Consumptions
                            .FirstOrDefaultAsync(c => c.ItemId == ingredient.ItemId);

                        // Calculate consumption for the quantity that needs crafting
                        var consumptionAmount = ingredient.Amount * ((construction.Quantity - availableQuantity) / recipe.Amount);

                        if (ingredientConsumption == null)
                        {   
                            ingredientConsumption = new Consumption
                            {
                                ItemId = ingredient.ItemId,
                                Name = ingredient.Item.Name,
                                TotalQuantity = consumptionAmount
                            };
                            _context.Consumptions.Add(ingredientConsumption);
                        }   
                        else
                        {   
                            ingredientConsumption.TotalQuantity += consumptionAmount;
                            _context.Consumptions.Update(ingredientConsumption);
                        }   
                    }
                }
            }
            else
            {
                // Enough available, calculate cost based on item value
                itemCost = construction.Item.Value * construction.Quantity;
                status = "Processed";
                message = $"Processed {construction.Quantity} x {construction.Item.Name} for a cost of {itemCost}.";

                // Update consumption of the main item
                if (consumption == null)
                {
                    consumption = new Consumption
                    {
                        ItemId = construction.ItemId,
                        Name = construction.Item.Name,
                        TotalQuantity = construction.Quantity
                    };
                    _context.Consumptions.Add(consumption);
                }
                else
                {
                    consumption.TotalQuantity += construction.Quantity;
                    _context.Consumptions.Update(consumption);
                }
            }

            totalCost += itemCost;

            // Mark as completed
            construction.IsCompleted = true;
            construction.UpdatedAt = DateTime.UtcNow;

            results.Add(new
            {
                ItemName = construction.Item.Name,
                Status = status,
                Quantity = construction.Quantity,
                Cost = itemCost,
                Message = message
            });
        }

        _context.Constructions.UpdateRange(constructions);
        await _context.SaveChangesAsync();

        // Send email confirmation
        var subject = "Bulk Order Processing Results";
        var messageBody = "Bulk order processing complete. See details below:";
        var attachmentPath = "wwwroot/swagger-ui/a_generic_industrial-themed_character_holding_their_hand_in_an__OK__sign.png"; // Path to your image
        var recipientEmail = "valentincheznoiu@gmail.com"; // Replace with the recipient's email

        // You might want to format the results into the email body for bulk completion
        foreach (var result in results)
        {
            messageBody += $"\n{result}";
        }

        await _emailService.SendEmailAsync(recipientEmail, subject, messageBody, attachmentPath);

        return Ok(new
        {
            Message = "Processing completed",
            TotalCost = totalCost,
            Results = results
        });
    }

    // Clear all items from construction list
    [HttpDelete("ClearConstructionList")]
    public async Task<IActionResult> ClearConstructionList()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User not authenticated");
        }

        var constructions = await _context.Constructions
            .Where(c => c.UserId == userId && !c.IsCompleted)
            .ToListAsync();

        _context.Constructions.RemoveRange(constructions);
        await _context.SaveChangesAsync();
        return Ok();
    }
}