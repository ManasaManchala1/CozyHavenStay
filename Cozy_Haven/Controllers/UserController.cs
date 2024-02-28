using Cozy_Haven.Exceptions;
using Cozy_Haven.Models.DTOs;
using Cozy_Haven.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Cozy_Haven.Models;
using Microsoft.AspNetCore.Authorization;
using System.Collections.ObjectModel;
using Cozy_Haven.Services;
using Microsoft.AspNetCore.Cors;

namespace Cozy_Haven.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("ReactPolicy")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService,ILogger<UserController> logger)
        {
            _userService=userService;
            _logger=logger;
            
        }
        [HttpPost("Register")]
        public async Task<ActionResult<LoginUserDTO>> Register(RegisterUserDTO user)
        {
            try
            {
                var result = await _userService.Register(user);
                return Ok(result); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while registering a user.");
                return StatusCode(500, "An error occurred while registering a user."); 
            }
        }
        [HttpPost("Login")]
        public async Task<ActionResult<LoginUserDTO>> Login(LoginUserDTO user)
        {
            try
            {
                var result = await _userService.Login(user);
                return Ok(result);
            }
            catch (InvalidUserException iuse)
            {
                _logger.LogCritical(iuse.Message);
                return Unauthorized("Invalid username or password");
            }
        }

        [HttpDelete("DeleteUser")]
        public async Task<ActionResult<User>> DeleteUser(string username)
        {
            try
            {
                var deletedUser = await _userService.DeleteUser(username);
                return Ok(deletedUser);
            }
            catch (UserNotFoundException ex)
            {
                _logger.LogError(ex, "User not found: {Username}", username);
                return NotFound(ex.Message);
            }
        }
        //[Authorize(Roles = "Admin")]
        [HttpGet("GetAllUsers")]
        public async Task<ActionResult<List<User>>> GetUsers()
        {
            try
            {
                var users = await _userService.GetAllUsers();
                return Ok(users);
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet("GetByUsername")]
        public async Task<ActionResult<User>> GetUserByUsername(string username)
        {
            try
            {
                var user=await _userService.GetUser(username);
                if (user == null) return NotFound("User not found.");
                return Ok(user);
            }
            catch (UserNotFoundException ex)
            {
                _logger.LogError(ex, "User not found: {Username}", username);
                return NotFound(ex.Message);
            }
        }
        [HttpPut("{username}/update-password")]
        public async Task<ActionResult<User>> UpdatePassword(string username, string password)
        {
            try
            {
                var updatedUser=await _userService.UpdatePassword(username, password);
                return Ok(updatedUser);
            }
            catch (UserNotFoundException ex)
            {
                _logger.LogError(ex, "User not found: {Username}", username);
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{username}/bookings")]
        public async Task<ActionResult<ICollection<Booking>>> GetUserBookings(string username)
        {
            try
            {
                var bookings=await _userService.GetUserBookings(username);
                return Ok(bookings);
            }
            catch (NoBookingFoundException ex)
            {
                _logger.LogError(ex, "No bookings found for user: {Username}", username);
                return NotFound(ex.Message);
            }
            catch (UserNotFoundException ex)
            {
                _logger.LogError(ex, "User not found: {Username}", username);
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{username}/reviews")]
        public async Task<ActionResult<ICollection<Review>>> GetUserReviews(string username)
        {
            try
            {
                var reviews=await _userService.GetUserReviews(username);
                return Ok(reviews);
            }
            catch (NoReviewFoundException ex)
            {
                _logger.LogError(ex, "No reviews found for user: {Username}", username);
                return NotFound(ex.Message);
            }
            catch (UserNotFoundException ex)
            {
                _logger.LogError(ex, "User not found: {Username}", username);
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{username}/favorites")]
        public async Task<ActionResult<ICollection<Favourite>>> GetUserFavorites(string username)
        {
            try
            {
                var favorites = await _userService.GetUserFavorites(username);
                return Ok(favorites);
            }
            catch (NoFavouriteFoundException ex)
            {
                _logger.LogError(ex, "No favorites found for user: {Username}", username);
                return NotFound(ex.Message);
            }
            catch (UserNotFoundException ex)
            {
                _logger.LogError(ex, "User not found: {Username}", username);
                return NotFound(ex.Message);
            }
            
        }
        [HttpPut("UpdateUserProfile/{username}")]
        public async Task<ActionResult> UpdateUserProfile(string username, [FromBody] UpdateUserDTO updateUserDto)
        {
            try
            {
                var user = await _userService.UpdateUserProfile(username, updateUserDto.FirstName, updateUserDto.LastName, updateUserDto.ContactNumber, updateUserDto.Email, updateUserDto.DateOfBirth);
                if (user != null)
                {
                    return Ok(user);
                }
                return NotFound($"User with username {username} not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating the user profile");
            }
        }

        [HttpGet("{usernameOrEmail}")]
        public async Task<ActionResult<User>> GetUserByUsernameOrEmail(string usernameOrEmail)
        {
            try
            {
                var user = await _userService.GetUserByUsernameOrEmail(usernameOrEmail);
                return Ok(user);
            }
            catch (UserNotFoundException ex)
            {
                _logger.LogError(ex, "User not found: {UsernameOrEmail}", usernameOrEmail);
                return NotFound(ex.Message);
            }
            
        }
        [Authorize(Roles ="Admin")]
        [HttpGet("GetHotelOwners")]
        public async Task<ActionResult<List<User>>> GetHotelOwners()
        {
            try
            {
                var hotelOwners = await _userService.GetHotelOwners();
                return Ok(hotelOwners);
            }
            catch (UserNotFoundException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


    }
}
