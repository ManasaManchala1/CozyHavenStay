using Cozy_Haven.Exceptions;
using Cozy_Haven.Interfaces;
using Cozy_Haven.Models;
using Cozy_Haven.Models.DTOs;
using Cozy_Haven.Repository;
using Cozy_Haven.Services;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cozy_Haven_Testing
{
    public class User_Testing
    {
        private IUserService _userService;
        private Mock<IRepository<string, User>> _mockRepo;
        private Mock<ILogger<UserService>> _mockLogger;
        private Mock<ITokenService> _mockTokenService;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IRepository<string, User>>();
            _mockLogger = new Mock<ILogger<UserService>>();
            _mockTokenService = new Mock<ITokenService>();
            _userService = new UserService(_mockRepo.Object, _mockLogger.Object, _mockTokenService.Object);
        }

        [Test]
        public async Task LoginSuccessTest()
        {
            // Arrange
            var loginDto = new LoginUserDTO { Username = "user1", Password = "pass" };
            var user = new User { Username = "user1", Password = new byte[0], Key = new byte[0], Role = "User" };

            _mockRepo.Setup(x => x.GetById(loginDto.Username)).ReturnsAsync(user);
            _mockTokenService.Setup(x => x.GenerateToken(It.IsAny<LoginUserDTO>())).ReturnsAsync("token");

            // Act
            var result = await _userService.Login(loginDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("User", result.Role);
            Assert.AreEqual("token", result.Token);
        }

        [Test]
        public async Task RegisterSuccessTest()
        {
            // Arrange
            var registerDto = new RegisterUserDTO { Username = "newUser", Password = "newPass" };
            var user = new User { Username = "newUser", Role = "User" };

            _mockRepo.Setup(x => x.Add(It.IsAny<User>())).ReturnsAsync(user);

            // Act
            var result = await _userService.Register(registerDto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("newUser", result.Username);
            Assert.AreEqual("User", result.Role);
        }

        [Test]
        public async Task GetAllUsersTest()
        {
            // Arrange
            var users = new List<User> { new User { Username = "user1" }, new User { Username = "user2" } };
            _mockRepo.Setup(x => x.GetAll()).ReturnsAsync(users);

            // Act
            var result = await _userService.GetAllUsers();

            // Assert
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public async Task GetUserTest()
        {
            // Arrange
            var username = "user1";
            var user = new User { Username = "user1" };
            _mockRepo.Setup(x => x.GetById(username)).ReturnsAsync(user);

            // Act
            var result = await _userService.GetUser(username);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("user1", result.Username);
        }

        [Test]
        public async Task DeleteUserTest()
        {
            // Arrange
            var username = "userToDelete";
            var user = new User { Username = "userToDelete" };
            _mockRepo.Setup(x => x.GetById(username)).ReturnsAsync(user);
            _mockRepo.Setup(x => x.Delete(username)).ReturnsAsync(user);

            // Act
            var result = await _userService.DeleteUser(username);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("userToDelete", result.Username);
        }


        [Test]
        public async Task UpdatePasswordTest()
        {
            // Arrange
            var username = "userToUpdate";
            var newPassword = "newPass";
            var user = new User { Username = "userToUpdate", Password = new byte[0], Key = new byte[0] };
            _mockRepo.Setup(x => x.GetById(username)).ReturnsAsync(user);
            _mockRepo.Setup(x => x.Update(It.IsAny<User>())).ReturnsAsync((User u) => u);

            // Act
            var result = await _userService.UpdatePassword(username, newPassword);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("userToUpdate", result.Username);
        }

    }
}
