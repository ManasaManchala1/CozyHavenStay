using Cozy_Haven.Models.DTOs;
using Cozy_Haven.Models;
using System.Security.Cryptography;
using System.Text;

namespace Cozy_Haven.Mappers
{
    public class RegisterToUser
    {
        User user;
        public RegisterToUser(RegisterUserDTO register)
        {
            user = new User();
            user.Username = register.Username;
            user.Role = register.Role;
            GetPassword(register.Password);
            user.Email = register.Email;
            user.FirstName = register.FirstName;
            user.LastName = register.LastName;
            user.ContactNumber = register.ContactNumber;
        }
        void GetPassword(string password)
        {
            HMACSHA512 hmac = new HMACSHA512();
            user.Key = hmac.Key;
            user.Password = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
        public User GetUser()
        {
            return user;
        }
    }
}
