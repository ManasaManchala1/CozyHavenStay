using Cozy_Haven.Contexts;
using Cozy_Haven.Interfaces;
using Cozy_Haven.Models;
using Microsoft.EntityFrameworkCore;

namespace Cozy_Haven.Repository
{
    public class DestinationRepository : IRepository<int, Destination>
    {
        private readonly CozyHavenContext _context;

        public DestinationRepository(CozyHavenContext context)
        {
            _context = context;
        }
        public async Task<Destination> Add(Destination item)
        {
            _context.Destinations.Add(item);
            _context.SaveChanges();
            return item;

        }

        public async Task<Destination> Delete(int key)
        {
            var destination = await GetById(key);
            if (destination != null)
            {
                _context.Destinations.Remove(destination);
                _context.SaveChanges();
                return destination;
            }
            return null;
        }

        public async Task<List<Destination>> GetAll()
        {
            return _context.Destinations.ToList();
        }

        public async Task<Destination> GetById(int key)
        {
            var destination = _context.Destinations.FirstOrDefault(b => b.DestinationId == key);
            return destination;
        }

        public async Task<Destination> Update(Destination item)
        {
            var destination = await GetById(item.DestinationId);
            if (destination != null)
            {
                _context.Entry<Destination>(item).State = EntityState.Modified;
                _context.SaveChanges();
                return item;
            }
            return null;
        }
    }
}
