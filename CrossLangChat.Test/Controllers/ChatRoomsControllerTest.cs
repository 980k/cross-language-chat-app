using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using CrossLangChat.Controllers;
using CrossLangChat.Models;
using CrossLangChat.Data;
using CrossLangChat.Services;
using Microsoft.EntityFrameworkCore.Update.Internal;
using NuGet.Versioning;
using Moq;
using Microsoft.AspNetCore.Http;

namespace CrossLangChat.Test
{
    public class ChatRoomsControllerTest
    {
        private CrossLangChatContext ? _context;
        private Mock<HttpContext> _httpContextMock;

        private Mock<DeepLTranslationService> _translationServiceMock;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<CrossLangChatContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;

            _context = new CrossLangChatContext(options);

            _httpContextMock = new Mock<HttpContext>();
            _httpContextMock.Setup(c => c.Session).Returns(new Mock<ISession>().Object);

            string apiKey = "PLACEHOLDER_API_KEY";

            _translationServiceMock = new Mock<DeepLTranslationService>(apiKey);
        }

        [TearDown]
        public void TearDown()
        {
            _context?.Database.EnsureDeleted();
            _context?.Dispose();
        }

        [Test]
        public async Task ChatRoomsController_CreateChatRoom_ShouldReturnChatRoom() {
            var chatRoomController = new ChatRoomsController(_context!, _translationServiceMock.Object);

            var newChatRoom = new ChatRoom { Id = 1, RoomName = "Room 1" };

            var result = await chatRoomController.Create(newChatRoom) as RedirectToActionResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());

            var existingChatRoom = await _context!.ChatRoom.FindAsync(1);

            Assert.That(existingChatRoom, Is.EqualTo(newChatRoom));
        }

        [Test]
        public async Task ChatRoomsController_AddUser_ShouldReturnUpdatedChatRoom() {
            var chatRoomController = new ChatRoomsController(_context!, _translationServiceMock.Object);

            var newChatRoom = new ChatRoom { Id = 2, RoomName = "Room 2" };

            await chatRoomController.Create(newChatRoom);

            var userController = new UsersController(_context!);

            var newUser = new User { Id = 3, Username = "testUser3", Password = "test3", Language = "JA" };

            await userController.Create(newUser);

            var result = await chatRoomController.EditChatRoomAddUser(newUser.Username, newChatRoom.Id) as RedirectToActionResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.That(result.ActionName, Is.EqualTo("Index"));
            Assert.That(result.ControllerName, Is.EqualTo("Home"));

            var existingChatRoom = await _context!.ChatRoom.FindAsync(2);

            Assert.That(existingChatRoom, Is.Not.Null);
            Assert.That(existingChatRoom.Users, Is.Not.Null);
            Assert.That(existingChatRoom.Users, Has.Count.EqualTo(1));

            CollectionAssert.Contains(existingChatRoom.Users, newUser);
        }

        [Test]
        public async Task ChatRoomsController_DeleteChatRoom_ShouldReturnNone() {
            var chatRoomController = new ChatRoomsController(_context!, _translationServiceMock.Object);

            var newChatRoom = new ChatRoom { Id = 3, RoomName = "Room 3" };

            await chatRoomController.Create(newChatRoom);

             var existingChatRoom = await _context!.ChatRoom.FindAsync(3);

            Assert.That(existingChatRoom, Is.Not.Null);
            Assert.That(existingChatRoom, Is.EqualTo(newChatRoom));

            var result = await chatRoomController.DeleteChatRoom(existingChatRoom.Id) as RedirectToActionResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
            Assert.That(result.ActionName, Is.EqualTo("Index"));
            Assert.That(result.ControllerName, Is.EqualTo("Home"));

            var deletedChatRoom = await _context!.ChatRoom.FindAsync(3);

            Assert.That(deletedChatRoom, Is.Null);
        } 
    }
}