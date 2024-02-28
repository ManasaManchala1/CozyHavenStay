using Cozy_Haven.Models;
using Cozy_Haven.Models.DTOs;

namespace Cozy_Haven.Interfaces
{
    public interface IUserService
    {
        public Task<LoginUserDTO> Login(LoginUserDTO user);
        public Task<LoginUserDTO> Register(RegisterUserDTO user);
        public Task<List<User>> GetAllUsers();
        public Task<User> GetUser(string username);
        //public Task<User> AddUser(User user);
        public Task<User> DeleteUser(string username);
        public Task<User> UpdatePassword(string username,string password);
        public Task<User> UpdateUserProfile(string username, string firstName, string lastName, string contactNumber, string email, DateTime dateofbirth);
        public Task<ICollection<Booking>> GetUserBookings(string username);
        public Task<ICollection<Review>> GetUserReviews(string username);
        public Task<ICollection<Favourite>> GetUserFavorites(string username);
        public Task<User> GetUserByUsernameOrEmail(string usernameOrEmail);
        public Task<List<User>> GetHotelOwners();
    }
}