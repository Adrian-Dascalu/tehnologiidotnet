using System.Drawing.Printing;
using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using tehnologiinet.Repositories;
using tehnologiinet.Entities;
//using tehnologiinet.Models;

namespace tehnologiinet.Controllers;

[Route("Factorio")]
[ApiController]
[EnableCors]
public class GamesController : ControllerBase
{
    private readonly IFactorioRepository _factorioRepository;

    // connect to database
    private readonly ApplicationDbContext _context;

    // Injecting repository via constructor
    public GamesController(IFactorioRepository factorioRepository, ApplicationDbContext context)
    {
        _factorioRepository = factorioRepository;
        _context = context;
    }

    [HttpPost("LoadProductionFromJson")]
    public IActionResult LoadProduction()
    {
        var productions = _factorioRepository.LoadProductionFromJson();
        
        using (var db = new DatabaseContext())
        {
            foreach (var production in productions)
            {
                var exists = db.Productions.Any(p => p.Id == production.Id);
                if (!exists)
                {
                    db.Productions.Add(production);
                }
            }

            db.SaveChanges();
        }

        return Ok(productions);
    }

    [HttpPost("LoadConsumtionFromJson")]
    public IActionResult LoadConsumption()
    {
        var consumptions = _factorioRepository.LoadConsumptionFromJson();
        
        using (var db = new DatabaseContext())
        {
            foreach (var consumption in consumptions)
            {
                var exists = db.Consumptions.Any(c => c.Id == consumption.Id);
                if (!exists)
                {
                    db.Consumptions.Add(consumption);
                }
            }

            db.SaveChanges();
        }

        return Ok(consumptions);
    }

    [HttpPost("LoadRecipesFromJson")]
    public IActionResult LoadRecipes()
    {
        var recipes = _factorioRepository.LoadRecipesFromJson();

        using (var db = new DatabaseContext())
        {
            foreach (var recipe in recipes)
            {
                var exists = db.Recipes.Any(r => r.Id == recipe.Id);
                if (!exists)
                {
                    db.Recipes.Add(recipe);
                }
            }

            db.SaveChanges();
        }

        return Ok(recipes);
    }

    [HttpPost("LoadItemsFromJson")]
    public IActionResult LoadItems()
    {
        using (var db = new DatabaseContext())
        {
            foreach (var item in _factorioRepository.LoadItemsFromJson())
            {
                db.Items.Add(item);
            }

            db.SaveChanges();
        }
        
        return Ok(_factorioRepository.LoadItemsFromJson());
    }

    [HttpPost("AddItem")]
    public IActionResult AddItem([FromBody] Item model)
    {
        if (model == null)
        {
            return BadRequest("Item data is null");
        }

        var item = new Item
        {
            Name = model.Name,
            Value = model.Value,
        };

        using (var db = new DatabaseContext())
        {
            db.Items.Add(item);
            db.SaveChanges();
        }

            return Ok(item);
    }

    [HttpDelete("DeleteItem")]
    public IActionResult DeleteItem([FromBody] Item model)
    {
        if (model == null || model.Id <= 0)
        {
            return BadRequest("Invalid item data");
        }

        using (var db = new DatabaseContext())
        {
            var item = db.Items.Find(model.Id);
            if (item == null)
            {
                item = db.Items.FirstOrDefault(i => i.Name == model.Name);
                if (item == null)
                {
                    return NotFound("Item not found");
                }
            }

            db.Items.Remove(item);
            db.SaveChanges();
        }

        return Ok("Item deleted successfully");
    }

    [HttpPut("UpdateItem")]
    public IActionResult UpdateItem([FromBody] Item model)
    {
        if (model == null || model.Id <= 0)
        {
            return BadRequest("Invalid item data");
        }

        using (var db = new DatabaseContext())
        {
            var item = db.Items.Find(model.Id);
            if (item == null)
            {
                item = db.Items.FirstOrDefault(i => i.Name == model.Name);
                if (item == null)
                {
                    return NotFound("Item not found");
                }
            }

            item.Value = model.Value;

            db.Items.Update(item);
            db.SaveChanges();
        }

        return Ok("Item updated successfully");
    }
    
    [HttpGet("GetAllProductions")]
    public IActionResult GetAllProductions()
    {
        var productions = _factorioRepository.GetAllProductions();
        if (productions.Count == 0)
        {
            return NotFound();
        }

        return Ok(productions); 
    }

    [HttpGet("GetNotNullProductions")]
    public IActionResult GetNotNullProductions()
    {
        var productions = _factorioRepository.GetAllProduction().Where(p => p.TotalQuantity > 0).ToList();
        if (productions.Count == 0)
        {
            return NotFound();
        }
        return Ok(productions);
    }


    /*[HttpGet]
    public IActionResult FilterItemsByType([FromQuery] string type)
    {
        var items = _factorioRepository.FilterItemsByType(type);
        if (items.Count == 0)
        {
            return NotFound();
        }
        return Ok(items);
    }*/

    //[HttpPost("update-result")]
    //public IActionResult UpdateResult([FromBody] MatchResult result)
    //{
    // Console.WriteLine("Received update-result request");

    // var factorio = _factorioRepository.GetFactorioByName(result.Name!);

    //if (student == null)
    //{
    //return NotFound("Student not found");
    //}

    //_studentsRepository.UpdateStudent(student); // Ensure this updates the data source

    // return Ok(User);
    //}

    // DTO to match the expected JSON body
    public class MatchResult
    {
        public string? Username { get; set; }
        public int Win { get; set; }
        public int Lose { get; set; }
    }
}