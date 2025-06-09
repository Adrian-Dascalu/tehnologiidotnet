using System.Drawing.Printing;
using tehnologiinet.Entities;
using System.Text.Json;
using System.Runtime.Versioning;
using Microsoft.AspNetCore.Http.Features;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc;


namespace tehnologiinet.Repositories;

public class FactorioRepository: IFactorioRepository
{
    public List<Production> GetAllProduction()
    {
        return GetAllProductionFromDb();
    }

    public List<Consumption> GetAllConsumption()
    {
        return GetAllConsumptionFromDb();
    }

    public Production GetProductionById(long Id)
    {
        UpdateProduction(new Factorio());
        return GetAllProductionFromDb().FirstOrDefault(x => x.Id == Id)!;
    }

    public Consumption GetConsumptionById(long Id)
    {
        return GetAllConsumptionFromDb().FirstOrDefault(x => x.Id == Id)!;
    }

    public Production GetProductionByItem(Item Item)
    {
        return GetAllProductionFromDb().FirstOrDefault(x => x.Item == Item)!;
    }

    public Item GetItemById(long Id)
    {
        return GetAllItemsFromDb().FirstOrDefault(x => x.Id == Id)!;
    }

    public void UpdateProduction(Factorio updatedProduction)
    {
        var productions = LoadProductionFromJson();
        var production = GetProductionById(updatedProduction.Id);
    }

    public List<Production> LoadProductionFromJson()
    {
        var file_path = "Factorio/production-nauvis-10min.json";
        
        if (!File.Exists(file_path))
        {
            return new List<Production>(); // Return empty list if file does not exist
        }

        var json = File.ReadAllText(file_path);
        var jsonData = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, int>>>(json);
        var productionData = new List<Production>();

        var itemChace = new Dictionary<string, Item>();

        foreach(var key in jsonData.Keys)
        {
            var dictionary_sample = jsonData[key];
            var total_value = dictionary_sample.Values.Sum();

            if (!itemChace.ContainsKey(key))
            {
                var item = GetItemByName(key);
                if (item == null)
                {
                    Console.WriteLine($"Item '{key}' not found in database.");
                    continue;
                }
                itemChace[key] = item;
            }

            var production = new Production
            {
                Name = key,
                ItemId = itemChace[key].Id,
                TotalQuantity = total_value
            };
            
            productionData.Add(production);
        }

        return productionData;
    }

    public List<Consumption> LoadConsumptionFromJson()
    {
        var file_path = "Factorio/consumption-nauvis-10min.json";
        
        if (!File.Exists(file_path))
        {
            return new List<Consumption>(); // Return empty list if file does not exist
        }

        var json = File.ReadAllText(file_path);
        var jsonData = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, int>>>(json);
        var consumptionData = new List<Consumption>();

        var itemChace = new Dictionary<string, Item>();

        foreach(var key in jsonData.Keys)
        {
            var dictionary_sample = jsonData[key];
            var total_value = dictionary_sample.Values.Sum();

            if (!itemChace.ContainsKey(key))
            {
                var item = GetItemByName(key);
                if (item == null)
                {
                    Console.WriteLine($"Item '{key}' not found in database.");
                    continue;
                }
                itemChace[key] = item;
            }

            var consumption = new Consumption
            {
                Name = key,
                ItemId = itemChace[key].Id,
                TotalQuantity = total_value
            };
            
            consumptionData.Add(consumption);
        }

        return consumptionData;
    }

    public List<Production> UpdateProductionFromJson()
    {
        var file_path = "Factorio/production-nauvis-10min.json";

        var json = File.ReadAllText(file_path);
        var jsonData = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, int>>>(json);

        var productionData = new List<Production>();
        //get every item from json (data from json is {item1:{sample1: 1, sample2:2}, item2:{sample1:1, sample2:2}})

        // test is a dictionary with the keys of the json data
        var keys = jsonData.Keys;

        //wooden chest, iron chest, steel chest, etc

        var productionId = 1;
        var itemChace = new Dictionary<string, Item>();

        foreach(var key in keys) // wooden chest, iron chest
        {
            //get the value of the item
            var dictionary_sample = jsonData[key];
            
            var samples_keys = dictionary_sample.Keys; //id = where item_name == key

            var total_value = 0;

            foreach(var sample_key in samples_keys)
            {
                //get the value of the key
                var val = dictionary_sample[sample_key];
                //add the value to the total value
                total_value += val;
            }

            //get the item by name
            if (!itemChace.ContainsKey(key))
            {
                var item = GetItemByName(key);
                if (item == null)
                {
                    Console.WriteLine($"Item '{key}' not found in database.");
                    continue;
                }
                itemChace[key] = item;
            }

            var itemId = itemChace[key].Id;
            
            //update the database with the new value
            using (var db = new DatabaseContext())
            {
                var production = db.Productions.FirstOrDefault(x => x.ItemId == itemId);
                if (production != null)
                {
                    production.TotalQuantity = total_value;
                    db.SaveChanges();
                }

                productionData.Add(production);
            }
        }

        return productionData;
    }

    public List<Item> LoadItemsFromJson()
    {
        var file_path = "Factorio/item.json";
        
        if (!File.Exists(file_path))
        {
            return new List<Item>(); // Return empty list if file does not exist
        }

        var json = File.ReadAllText(file_path);
        
        var jsonData = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, object>>>(json);

        var itemData = new List<Item>();

        var keys = jsonData.Keys;

        foreach(var key in keys)
        {
            var dictionary_sample = jsonData[key];
            var item = new Item();
            item.Name = key;
            item.Value = 1;
            //item.Recipe = null;
            itemData.Add(item);
        }

        return itemData;
    }

    public Item GetItemByName(string name)
    {
        return GetAllItemsFromDb().FirstOrDefault(x => x.Name == name)!;
    }

    public long GetItemIdByName(string name)
    {
        using (var db = new DatabaseContext())
        {
            var item = db.Items.FirstOrDefault(x => x.Name == name);
            if (item != null)
            {
                return item.Id;
            }
            else
            {
                // Handle the case where the item is not found
                return -1; // or throw an exception, or return a default value
            }
        }
    }

// private List<Recipe> LoadRecipesFromJson()
    public List<Recipe> LoadRecipesFromJson()
    {
        var file_path = "Factorio/recipe.json";
        
        if (!File.Exists(file_path))
        {
            //return string empty
            string empty = "empty";
            Recipe recipe = new Recipe();
            recipe.Id = 1;
            recipe.ItemId = 0;
            recipe.Ingredients = new List<Ingredient>();
            Ingredient ingredient = new Ingredient();
            ingredient.Id = 1;
            ingredient.Amount = 1;
            ingredient.ItemId = 1;
            ingredient.RecipeId = 1;
            //return new List<Production>(); // Return empty list if file does not exist
        }

        var json = File.ReadAllText(file_path);
        var jsonData = JObject.Parse(json);

        var recipeData = new List<Recipe>();
        //var ingredients = new List<Ingredient>();

        var recipeId = 1;
        var ingredientId = 1;

        var itemChace = new Dictionary<string, Item>();
        var recipeChace = new Dictionary<string, Recipe>();
        
        foreach (var topLevelKey in jsonData.Properties())
        {
            var recipeObject = topLevelKey.Value;
            var recipeName = topLevelKey.Name;

            var ingrToken = recipeObject["ingredients"];
            
            var ingredients = new List<Ingredient>(); // 👈 RESET ingredients for each recipe

            if (ingrToken is JArray ingrArray)
            {
                foreach (var ingr in ingrArray)
                {
                    var itemName = (string)ingr["name"];

                    if (!itemChace.ContainsKey(itemName))
                    {
                        var item = GetItemByName(itemName);
                        if (item == null)
                        {
                            Console.WriteLine($"Item '{itemName}' not found in database.");
                            continue;
                        }
                        itemChace[itemName] = item;
                    }

                    var itemId = itemChace[itemName].Id;

                    var ingredient = new Ingredient
                    {
                        Id = ingredientId++,
                        ItemId = itemId,
                        Amount = (int)ingr["amount"]
                    };

                    ingredients.Add(ingredient);
                }
            }

            var resultsToken = recipeObject["results"];
            if (resultsToken is JArray resultsArray)
            {
                foreach (var res in resultsArray)
                {
                    if (!itemChace.ContainsKey(recipeName))
                    {
                        var item = GetItemByName(recipeName);
                        if (item == null)
                        {
                            Console.WriteLine($"Item '{recipeName}' not found in database.");
                            continue;
                        }

                        itemChace[recipeName] = item;
                    }

                    var recipe = new Recipe
                    {
                        Id = recipeId++,
                        ItemId = itemChace[recipeName].Id,
                        Ingredients = ingredients,
                        Amount = (int)res["amount"]
                    };

                    foreach (var ingr in recipe.Ingredients)
                    {
                        ingr.RecipeId = recipe.Id;
                    }

                    recipeData.Add(recipe);
                }
            }
            

        }

        return recipeData;
    }

    public List<Production> GetAllProductions()
    {
        return GetAllProductionFromDb();
    }

    private List<Production> GetAllProductionFromDb()
    {
        using (var db = new DatabaseContext())
        {
            return db.Productions.ToList();
        }
    }

    private List<Consumption> GetAllConsumptionFromDb()
    {
        using(var db = new DatabaseContext())
        {
            return db.Consumptions.ToList();
        }
    }

    private List<Item> GetAllItemsFromDb()
    {
        using(var db = new DatabaseContext())
        {
            return db.Items.ToList();
        }
    }

    public Consumption GetConsumptionByItem(Item Item)
    {
        throw new NotImplementedException();
    }
}