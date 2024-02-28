using Cozy_Haven.Contexts;
using Cozy_Haven.Interfaces;
using Cozy_Haven.Models;
using Microsoft.EntityFrameworkCore;

namespace Cozy_Haven.Repository
{
    public class FavouriteRepository:IRepository<int,Favourite>
    {
        private readonly CozyHavenContext _context;

        public FavouriteRepository(CozyHavenContext context)
        {
            _context = context;
        }
        public async Task<Favourite> Add(Favourite item)
        {
            _context.Favourites.Add(item);
            _context.SaveChanges();
            return item;

        }

        public async Task<Favourite> Delete(int key)
        {
            var Favourite = await GetById(key);
            if (Favourite != null)
            {
                _context.Favourites.Remove(Favourite);
                _context.SaveChanges();
                return Favourite;
            }
            return null;
        }

        public async Task<List<Favourite>> GetAll()
        {
            return _context.Favourites.ToList();
        }

        public async Task<Favourite> GetById(int key)
        {
            var favourite = _context.Favourites.FirstOrDefault(b => b.FavouriteId == key);
            return favourite;
        }

        public async Task<Favourite> Update(Favourite item)
        {
            var favourite = await GetById(item.FavouriteId);
            if (favourite != null)
            {
                _context.Entry<Favourite>(item).State = EntityState.Modified;
                _context.SaveChanges();
                return item;
            }
            return null;
        }
    }
}
