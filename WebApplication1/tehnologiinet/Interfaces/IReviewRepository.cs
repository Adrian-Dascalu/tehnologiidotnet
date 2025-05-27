using tehnologiinet.Entities;

namespace tehnologiinet.Interfaces;

public interface IReviewRepository
{
    Review GetById(long Id);
    List<Review> GetAll();
    void Update (Review review);
    void Add(Review review);
    
}