using Cozy_Haven.Contexts;
using Cozy_Haven.Interfaces;
using Cozy_Haven.Models;
using Cozy_Haven.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Cozy_Haven.Repository
{
    public class HotelRepository : IRepository<int, Hotel>
    {
        private readonly CozyHavenContext _context;

        public HotelRepository(CozyHavenContext context)
        {
            _context=context;
        }
        public async Task<Hotel> Add(Hotel item)
        {
            _context.Hotels.Add(item);
            _context.SaveChanges();
            return item;
        }

        public async Task<Hotel> Delete(int key)
        {
            var hotel=await GetById(key);
            if(hotel!=null)
            {
                _context.Hotels.Remove(hotel);
                _context.SaveChanges();
                return hotel;
            }
            return null;
        }

        public async Task<List<Hotel>> GetAll()
        {
            return _context.Hotels
                .Include(h => h.Rooms)
                .Include(h => h.Reviews)
                .Include(h => h.Favorites)
                .Include(h => h.Amenities)
                .ToList();
        }

        public async Task<Hotel> GetById(int key)
        {
            var hotel=_context.Hotels.Include(h => h.Rooms)
                .Include(h => h.Reviews)
                .Include(h => h.Favorites)
                .Include(h => h.Amenities).FirstOrDefault(h=>h.HotelId==key);
            return hotel;
            
        }

        public async Task<Hotel> Update(Hotel item)
        {
            var hotel=await GetById(item.HotelId);
            if(hotel!=null )
            {
                _context.Entry<Hotel>(item).State=EntityState.Modified;
                _context.SaveChanges();
                return item;
            }
            return null;
        }
    }
}
