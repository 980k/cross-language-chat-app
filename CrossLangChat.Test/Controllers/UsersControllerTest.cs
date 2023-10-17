using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using CrossLangChat.Controllers;
using CrossLangChat.Models;
using CrossLangChat.Data;

namespace CrossLangChat.Test
{
    public class UsersControllerTest
    {
        private CrossLangChatContext ? _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<CrossLangChatContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;

            _context = new CrossLangChatContext(options);
        }

        [TearDown]
        public void TearDown()
        {
            _context?.Database.EnsureDeleted();
            _context?.Dispose();
        }

        [Test]
        public async Task UserController_UserUpdateLanguage_ShouldReturnUpdatedLanguage()
        {
            // Arrange
            var userController = new UsersController(_context!);
            var newUser = new User { Id = 1, Username = "testUser1", Password = "test1", Language = "English" };

            // Act
            await userController.Create(newUser);

            // Fetch the entity from the database
            var existingUser = _context?.User.Find(1);

            // Modify the language using the Edit method
            existingUser!.Language = "Chinese";
            await userController.Edit(1, existingUser);

            // Reload user from context after edit
            var updatedUser = _context?.User.Find(1);

            // Assert
            Assert.IsNotNull(updatedUser, "User with Id 1 should exist in the database after edit");
            Assert.AreEqual("Chinese", updatedUser?.Language, "User's language should be updated to Chinese");
        }
    }
}