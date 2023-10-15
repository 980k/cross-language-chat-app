using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using CrossLangChat.Controllers;
using CrossLangChat.Models;
using CrossLangChat.Data;

namespace CrossLangChat.Test
{
    public class UsersControllersTest
    {
        private CrossLangChatContext _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<CrossLangChatContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;

            _context = new CrossLangChatContext(options);
        }

        [Test]
        public async Task UsersController_Create_ValidModel_ShouldRedirectToIndex()
        {
            // Arrange
            var userController = new UsersController(_context);
            var newUser = new User { Id = 1, Username = "NewUser", Password = "NewPassword" };

            // Act
            var result = await userController.Create(newUser);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            if (result is RedirectToActionResult redirectResult)
            {
                Assert.AreEqual("Index", redirectResult.ActionName);
            }
            else
            {
                Assert.Fail("Expected RedirectToActionResult but got a different result.");
            }

            // Verify that the new user was added to the in-memory database
            var createdUser = _context.User.Find(1);
            Assert.IsNotNull(createdUser);
            Assert.AreEqual("NewUser", createdUser.Username);
            Assert.AreEqual("NewPassword", createdUser.Password);
        }

        [Test]
        public async Task UsersController_AddChatRoom_ShouldReturnChatrooms()
        {
            Assert.AreEqual(1, 2);
        }
    }
}