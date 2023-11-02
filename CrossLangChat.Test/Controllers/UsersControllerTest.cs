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
        public async Task UsersController_CreateNewUser_ShouldReturnUser()
        {
            var userController = new UsersController(_context!);
            var newUser = new User { Id = 2, Username = "testUser2", Password = "test2", Language = "ZH" };

            var result = await userController.Create(newUser) as RedirectToActionResult;

            Assert.NotNull(result);

            var existingUser = await _context!.User.FindAsync(2);

            Assert.That(existingUser?.Username, Is.EqualTo(newUser.Username));
            Assert.That(existingUser.Password, Is.EqualTo(newUser.Password));
        }

        [Test]
        public async Task UsersController_EditUserLanguage_ShouldReturnUser()
        {
            var userController = new UsersController(_context!);
            var newUser = new User { Id = 3, Username = "testUser3", Password = "test3", Language = "ES" };

            await userController.Create(newUser);

            var existingUser = await _context!.User.FindAsync(3);

            existingUser!.Language = "KO";

            var result = await userController.Edit(existingUser.Id, existingUser) as RedirectToActionResult;

            Assert.NotNull(result);

            var updatedUser = await _context.User.FindAsync(3);

            Assert.That(existingUser.Language, Is.EqualTo(updatedUser?.Language));
        }

        [Test]
        public async Task UsersController_DeleteUser_ShouldReturnNone()
        {
            var userController = new UsersController(_context!);
            var newUser = new User { Id = 4, Username = "testUser4", Password = "test4", Language = "DE" };

            await userController.Create(newUser);

            var existingUser = await _context!.User.FindAsync(4);

            Assert.NotNull(existingUser);
            Assert.That(newUser, Is.EqualTo(existingUser));

            var result = await userController.DeleteConfirmed(existingUser.Id) as RedirectToActionResult;

            Assert.NotNull(result);

            var deletedUser = await _context.User.FindAsync(4);

            Assert.Null(deletedUser);
        }
    }
}