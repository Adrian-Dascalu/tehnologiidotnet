using tehnologiinet.Interfaces;
using tehnologiinet.NewDirectory1;

namespace tehnologiinet.Repositories;

public class ReviewRepository: IReviewRepository
{
    public Review GetById(long Id)
    {
        using (var db = new DatabaseContext())
        {
            return db.Reviews.FirstOrDefault(x => x.Id == Id);
        }
        
        
    }

    public List<Review> GetAll()
    {
        using (var db = new DatabaseContext())
        {
            return db.Reviews.ToList();
        }
    }

    public void Update(Review review)
    {
        using (var db = new DatabaseContext())
        {
            db.Reviews.Update(review);
            db.SaveChanges();
        }
    }

    public void Add(Review review)
    {
        using (var db = new DatabaseContext())
        {
            db.Reviews.Add(review);
            db.SaveChanges();
        }
    }
}