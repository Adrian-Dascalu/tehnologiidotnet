using tehnologiinet.Entities;

namespace tehnologiinet.Interfaces;

public interface ICategoryRepository
{
    Category GetById(long id);
    List<Category> GetAll();
    void Add(Category category);
    void Update(Category category);
}