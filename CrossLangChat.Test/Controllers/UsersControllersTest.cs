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
        private CrossLangChatContext ? _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<CrossLangChatContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;

            _context = new CrossLangChatContext(options);
        }

        [Test]
        public async Task UsersController_AddChatRoom_ShouldReturnChatrooms()
        {
            var userController = new UsersController(_context!);
            var newUser = new User { Id = 1, Username="testUser1", Password="test1" };

            var chatroomController = new ChatRoomsController(_context!);
            var newChatRoom = new ChatRoom { Id = 1, RoomName="testRoom1" };

            await userController.Create(newUser);
            await chatroomController.Create(newChatRoom);

            var createdChatRoom = _context?.ChatRoom.Find(1);

            var userChatRoomUpdate = await userController.AddChatRoomToUser(1, createdChatRoom!);
            
            var updatedUser = _context?.User.Find(1);

            Assert.AreNotEqual(null, updatedUser?.ChatRooms);
            Assert.AreEqual(1, updatedUser?.ChatRooms.Count);
            Assert.AreEqual(newChatRoom, updatedUser?.ChatRooms.FirstOrDefault());
        }
    }
}