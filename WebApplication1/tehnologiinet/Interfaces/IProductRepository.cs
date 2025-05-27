using tehnologiinet.Entities;

namespace tehnologiinet.Interfaces;

public interface IProductRepository
{
    Product GetById(long Id);
    List<Product> GetAll();
    void Add(Product product);
    void Update(Product product);
}