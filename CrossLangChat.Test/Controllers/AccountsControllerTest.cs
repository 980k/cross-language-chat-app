using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using CrossLangChat.Controllers;
using CrossLangChat.Models;
using CrossLangChat.Data;
using CrossLangChat.ViewModels;

namespace CrossLangChat.Test
{
    public class AccountsControllerTest
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

        // HTTP CONTEXT needs to be mocked to test this.
        // [Test]
        // public async Task AccountsController_ValidLogin_ShouldReturnRedirect()
        // {
        //     var userController = new UsersController(_context!);
        //     var newUser = new User { Id = 1, Username = "testUser1", Password = "test1", Language = "English" };

        //     await userController.Create(newUser);

        //     var accountController = new AccountsController(_context!);

        //     var result = await accountController.Login("testUser1", "test1") as RedirectToActionResult;

        //     Assert.IsNotNull(result);
        // }

        [Test]
        public async Task AccountsController_InvalidLogin_ShouldReturnView()
        {
            var userController = new UsersController(_context!);
            var newUser = new User { Id = 1, Username = "testUser1", Password = "test1", Language = "English" };

            await userController.Create(newUser);

            var accountController = new AccountsController(_context!);

            LoginViewModel model = new LoginViewModel { Username = "testUser2", Password = "test2" };

            var result = await accountController.Login(model) as ViewResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ViewData["Error"], Is.EqualTo("Invalid name or password."));
        }
    }
}