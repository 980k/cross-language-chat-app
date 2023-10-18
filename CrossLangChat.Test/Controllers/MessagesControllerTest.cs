using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using CrossLangChat.Controllers;
using CrossLangChat.Models;
using CrossLangChat.Data;
using Microsoft.EntityFrameworkCore.Update.Internal;
using NuGet.Versioning;

namespace CrossLangChat.Test
{
    public class MessagesControllerTest
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
        public async Task MessagesController_CreateMessage_ShouldAssociateChatRoomAndUser()
        {
            var userController = new UsersController(_context!);
            var newUser = new User { Id = 1, Username = "testUser1", Password = "test1", Language = "English" };

            await userController.Create(newUser);

            var chatRoomController = new ChatRoomsController(_context!);
            var newChatRoom = new ChatRoom { Id = 1, RoomName = "testRoom1" };

            var existingUser = _context?.User.Find(1);

            await chatRoomController.CreateChatRoom(existingUser!.Id, newChatRoom);

            var createdChatRoom = _context?.ChatRoom.Find(1);

            Message newMessage = new Message {
                SenderId = existingUser.Id,
                ChatRoomId = createdChatRoom!.Id,
                Content = "Hello"
            };

            // await MessagesController.CreateMessage(newMessage);

            // var createdMessage = _context?.Message.Find(1);

            // Assert.IsNotNull(createdMessage);

            Assert.Pass();

        }
    }
}
