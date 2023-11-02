using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using Moq;
using CrossLangChat.Controllers;
using CrossLangChat.Models;
using CrossLangChat.Data;
using CrossLangChat.ViewModels;

namespace CrossLangChat.Test
{
    [TestFixture]
    public class AccountsControllerTest
    {
        private CrossLangChatContext ? _context;
        private Mock<HttpContext> _httpContextMock;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<CrossLangChatContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;

            _context = new CrossLangChatContext(options);

            _httpContextMock = new Mock<HttpContext>();
            _httpContextMock.Setup(c => c.Session).Returns(new Mock<ISession>().Object);
        }

        [TearDown]
        public void TearDown()
        {
            _context?.Database.EnsureDeleted();
            _context?.Dispose();
        }

        [Test]
        public async Task AccountsController_ValidLogin_ShouldReturnRedirect()
        {
            // Arrange
            var userController = new UsersController(_context!);
            var newUser = new User { Id = 1, Username = "testUser1", Password = "test1", Language = "EN" };
            await userController.Create(newUser);

            var accountController = new AccountsController(_context!);
            accountController.ControllerContext = new ControllerContext { HttpContext = _httpContextMock.Object };

            LoginViewModel model = new LoginViewModel { Username = "testUser1", Password = "test1" };

            var result = await accountController.Login(model) as RedirectToActionResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ActionName, Is.EqualTo("Index"));
            Assert.That(result.ControllerName, Is.EqualTo("Home"));
        }

        [Test]
        public async Task AccountsController_InvalidLogin_ShouldReturnView()
        {
            // Arrange
            var userController = new UsersController(_context!);
            var newUser = new User { Id = 1, Username = "testUser1", Password = "test1", Language = "EN" };
            await userController.Create(newUser);

            var accountController = new AccountsController(_context!);
            accountController.ControllerContext = new ControllerContext { HttpContext = _httpContextMock.Object };

            LoginViewModel model = new LoginViewModel { Username = "testUser2", Password = "test2" };

            // Act
            var result = await accountController.Login(model) as ViewResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ViewData["Error"], Is.EqualTo("Invalid name or password."));
        }
    }
}