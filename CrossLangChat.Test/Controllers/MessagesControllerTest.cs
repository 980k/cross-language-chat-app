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
    public class MessagesControllerTest
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
        public async Task ChatRoomsController_CreateMessage_ShouldCreateMessageAndRedirect()
        {
            var chatRoomController = new ChatRoomsController(_context!, _translationServiceMock.Object);

            var newChatRoom = new ChatRoom { Id = 5, RoomName = "Room 5" };

            await chatRoomController.Create(newChatRoom);

            var username = "testUser5";

            _httpContextMock.Object.Session.SetString("Username", username);

            var messageController = new MessagesController(_context!);

            var result = await messageController.CreateMessage(newChatRoom.Id, "Hello");

                Assert.That(result, Is.Not.InstanceOf<StatusCodeResult>());
                Assert.That(result, Is.InstanceOf<ObjectResult>());
        }


        
    }
}
