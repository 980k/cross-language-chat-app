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
    public class ChatRoomsControllerTest
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
        public async Task ChatRoomsController_CreateChatRoom_ShouldAssociateUser()
        {
            var userController = new UsersController(_context!);
            var newUser = new User { Id = 2, Username = "testUser2", Password = "test2", Language = "English" };

            await userController.Create(newUser);

            var chatRoomController = new ChatRoomsController(_context!);
            var newChatRoom = new ChatRoom { Id = 3, RoomName = "testRoom3" };

            var existingUser = _context?.User.Find(2);

            Assert.That(existingUser?.ChatRooms.Count, Is.EqualTo(0));

            await chatRoomController.CreateChatRoom(existingUser!.Id, newChatRoom);

            var createdChatRoom = _context?.ChatRoom.Find(3);

            existingUser = _context?.User.Find(2);

            Assert.That(createdChatRoom?.Users?.Count, Is.EqualTo(1));
            Assert.That(existingUser?.ChatRooms.Count, Is.EqualTo(1));
            Assert.That(existingUser?.ChatRooms.FirstOrDefault(), Is.EqualTo(createdChatRoom));
            Assert.That(existingUser?.ChatRooms?.FirstOrDefault()?.RoomName, Is.EqualTo(createdChatRoom?.RoomName));
        }

        [Test]
        public async Task ChatRoomsController_DeleteChatRoom_ShouldReturnOneLess()
        {
            var userController = new UsersController(_context!);
            var newUser = new User{ Id = 5, Username = "testUser5", Password = "test5", Language = "Japanese" };

            await userController.Create(newUser);

            var ChatRoomController = new ChatRoomsController(_context!);
            var newChatRoom = new ChatRoom { Id = 10, RoomName = "testRoom10" };

            var existingUser = _context?.User.Find(5);

            await ChatRoomController.CreateChatRoom(existingUser!.Id, newChatRoom);

            Assert.That(existingUser?.ChatRooms.Count, Is.EqualTo(1));

            var existingChatRoom = _context?.ChatRoom.Find(10);

            await ChatRoomController.DeleteChatRoom(existingChatRoom!.Id);

            existingChatRoom = _context?.ChatRoom.Find(10);

            existingUser = _context?.User.Find(5);

            Assert.That(existingChatRoom, Is.Null);
            Assert.That(existingUser?.ChatRooms.Count, Is.EqualTo(0));
        }

        [Test]
        public async Task ChatRoomsController_AddMultipleUsersToChatRoom_ShouldReturnAllUsers()
        {
            var userController = new UsersController(_context!);
            var newUser1 = new User{ Id = 1, Username = "testUser1", Password = "test1", Language = "Spanish" };
            var newUser2 = new User{ Id = 2, Username = "testUser2", Password = "test2", Language = "Portuguese" };

            await userController.Create(newUser1);
            await userController.Create(newUser2);

            var ChatRoomController = new ChatRoomsController(_context!);
            var newChatRoom = new ChatRoom { Id = 1, RoomName = "testRoom1" };

            var existingUser1 = _context?.User.Find(1);

            await ChatRoomController.CreateChatRoom(existingUser1!.Id, newChatRoom);

            var existingChatRoom = _context?.ChatRoom.Find(1);

            Assert.That(existingChatRoom?.Users?.Count, Is.Not.Null);
            Assert.That(existingChatRoom?.Users?.Count, Is.EqualTo(1));
            Assert.That(existingChatRoom!.Users!.Contains(existingUser1), Is.True);

            var existingUser2 = _context?.User.Find(2);

            Assert.IsFalse(existingChatRoom!.Users!.Contains(existingUser2!));

            await ChatRoomController.EditChatRoomAddUser(existingUser2!.Id, existingChatRoom!.Id);

            var updatedChatRoom = _context?.ChatRoom.Find(1);

            var updatedUser1 = _context?.User.Find(1);
            var updatedUser2 = _context?.User.Find(2);

            Assert.That(updatedChatRoom?.Users, Is.Not.Null);
            Assert.That(updatedChatRoom?.Users!.Count, Is.EqualTo(2));
            Assert.That(updatedChatRoom!.Users!.Contains(existingUser1), Is.True);
            Assert.IsTrue(updatedChatRoom!.Users!.Contains(existingUser2));
            Assert.IsTrue(updatedUser1!.ChatRooms.Contains(updatedChatRoom));
            Assert.IsTrue(updatedUser2!.ChatRooms.Contains(updatedChatRoom));
        }
    }
}