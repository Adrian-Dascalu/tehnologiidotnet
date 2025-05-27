using tehnologiinet.Interfaces;
using tehnologiinet.Entities;

namespace tehnologiinet.Repositories;

public class ProductRepository: IProductRepository
{
    public Product GetById(long Id)
    {
        using (var db = new DatabaseContext())
        {
            return db.Products.FirstOrDefault(x => x.Id == Id);
            
        }
    }

    public List<Product> GetAll()
    {
        using (var db = new DatabaseContext())
        {
            return db.Products.ToList();
        }
    }

    public void Add(Product product)
    {
        using (var db = new DatabaseContext())
        {
            db.Products.Add(product);
            db.SaveChanges();
        }
    }

    public void Update(Product product)
    {
        using (var db = new DatabaseContext())
        {
            db.Products.Update(product);
            db.SaveChanges();
        }
    }
}