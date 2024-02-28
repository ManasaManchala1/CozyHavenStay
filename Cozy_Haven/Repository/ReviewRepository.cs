using Cozy_Haven.Contexts;
using Cozy_Haven.Interfaces;
using Cozy_Haven.Models;
using Microsoft.EntityFrameworkCore;

namespace Cozy_Haven.Repository
{
    public class ReviewRepository:IRepository<int,Review>
    {
        private readonly CozyHavenContext _context;

        public ReviewRepository(CozyHavenContext context) {
            _context=context;
        }

        public async Task<Review> Add(Review item)
        {
            _context.Reviews.Add(item);
            _context.SaveChanges();
            return item;
        }

        public async Task<Review> Delete(int key)
        {
            var review=await GetById(key);
            if(review != null) { 
                _context.Reviews.Remove(review);
                _context.SaveChanges();
                return review;
            }
            return null;
        }

        public async Task<List<Review>> GetAll()
        {
            return _context.Reviews.ToList();
        }

        public async Task<Review> GetById(int key)
        {
            var review= _context.Reviews.FirstOrDefault(r=>r.ReviewId==key);
            return review;
        }

        public async Task<Review> Update(Review item)
        {
            var review=await GetById(item.ReviewId);
            if(review != null)
            {
                _context.Entry<Review>(item).State=EntityState.Modified;
                return item;
            }
            return null;
        }
    }
}
