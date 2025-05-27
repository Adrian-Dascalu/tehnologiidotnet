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

namespace tehnologiinet.Controllers;

[Route("Factorio")]
[ApiController]
[EnableCors]
public class ConstructionController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IFactorioRepository _factorioRepository;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AccountController> _logger;

    
    // connect to database
    private readonly DatabaseContext _context;

    // Injecting repository via constructor
    public ConstructionController(IFactorioRepository factorioRepository, DatabaseContext context)
    {
        _factorioRepository = factorioRepository;
        _context = context;
    }

    [HttpPost("AddToConstruction/{id}")]
    [Authorize]
    public async Task<IActionResult> AddToConstruction(long id)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Unauthorized(new { success = false, message = "User not authenticated." });
        }

        var item = _factorioRepository.GetItemById(id);
        if (item == null)
        {
            return NotFound(new { success = false, message = "Item not found." });
        }

        // Check if the item is already in construction for this user
        var existingItem = _context.Constructions.FirstOrDefault(c => c.ItemId == id);
        if (existingItem != null)
        {
            return BadRequest(new { success = false, message = "Item is already in construction." });
        }
        
        // Create a new construction entry
        var construction = new Construction
        {
            ItemId = item.Id,
            Name = item.Name,
            TotalQuantity = 1
        };

        _context.Constructions.Add(construction);
        await _context.SaveChangesAsync();

        return Ok(new { success = true, message = "Item added to construction successfully." });
    }
}