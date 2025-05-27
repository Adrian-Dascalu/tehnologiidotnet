using tehnologiinet.Interfaces;
using tehnologiinet.Entities;

namespace tehnologiinet.Repositories;

public class CategoryRepository: ICategoryRepository
{
    public Category GetById(long id)
    {
        using (var db = new DatabaseContext())
        {
            return db.Categories.FirstOrDefault(x => x.Id == id);
        }
    }

    public List<Category> GetAll()
    {
        using (var db = new DatabaseContext())
        {
            return db.Categories.ToList();
        }
    }

    public void Add(Category category)
    {
        using (var db = new DatabaseContext())
        {
            db.Categories.Add(category);
            db.SaveChanges();
        }
    }

    public void Update(Category category)
    {
        using (var db = new DatabaseContext())
        {
            db.Categories.Update(category);
            db.SaveChanges();
        }
    }
}