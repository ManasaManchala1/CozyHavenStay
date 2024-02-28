using Cozy_Haven.Contexts;
using Cozy_Haven.Interfaces;
using Cozy_Haven.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Cozy_Haven.Repository
{
    public class BookingRepository : IRepository<int, Booking>
    {
        private readonly CozyHavenContext _context;
        private readonly ILogger<BookingRepository> _logger;

        public BookingRepository(CozyHavenContext context, ILogger<BookingRepository> logger)
        {
            _context=context;
            _logger = logger;
        }
        public async Task<Booking> Add(Booking item)
        {
            _context.Bookings.Add(item);
            _context.SaveChanges();
            _logger.LogInformation("Booking added: {BookingId}", item.BookingId);
            return item;
            
        }

        public async Task<Booking> Delete(int key)
        {
            var booking=await GetById(key);
            if (booking!=null)
            {
                _context.Bookings.Remove(booking);
                _context.SaveChanges();
                _logger.LogInformation("Booking deleted: {BookingId}", key);
                return booking;
            }
            return null;
        }

        public async Task<List<Booking>> GetAll()
        {
            var bookings= _context.Bookings
                .Include(b => b.Room)
                .ToList();
            _logger.LogInformation("Retrieved all bookings.");
            return bookings;


        }

        public async Task<Booking> GetById(int key)
        {
            var booking = _context.Bookings
                .Include(b => b.Room)
                .FirstOrDefault(b => b.BookingId == key);
            if (booking != null)
            {
                _logger.LogInformation("Retrieved booking: {BookingId}", key);
            }
            return booking;

        }

        public async Task<Booking> Update(Booking item)
        {
            var booking=await GetById(item.BookingId);
            if (booking != null)
            {
                _context.Entry<Booking>(item).State=EntityState.Modified;
                _context.SaveChanges();
                _logger.LogInformation("Booking updated: {BookingId}", item.BookingId);
                return item;
            }
            return null;
        }
    }
}
