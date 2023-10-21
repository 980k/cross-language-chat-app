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
            Assert.That(updatedUser, Is.Not.Null, "User with Id 1 should exist in the database after edit");
            Assert.That(updatedUser?.Language, Is.EqualTo("Chinese"), "User's language should be updated to Chinese");
        }

        [Test]
        public async Task UsersController_GetUserChatRooms_ShouldReturnChatRooms()
        {
            var userController = new UsersController(_context!);
            var newUser = new User { Id = 7, Username = "testUser7", Password = "test7", Language = "German" };

            await userController.Create(newUser);

            var chatRoomController = new ChatRoomsController(_context!);
            var newChatRoom1 = new ChatRoom { Id = 2, RoomName = "testUser7's Room 1" };
            var newChatRoom2 = new ChatRoom { Id = 3, RoomName = "testUser7's Room 2" };

            var existingUser = _context?.User.Find(7);

            await chatRoomController.CreateChatRoom(existingUser!.Id, newChatRoom1);
            await chatRoomController.CreateChatRoom(existingUser!.Id, newChatRoom2);

            var ChatRooms = await userController.GetUserChatRooms(existingUser.Id) as OkObjectResult;

            Assert.That(ChatRooms, Is.Not.Null);

            var result = ChatRooms!.Value as List<ChatRoom>;

            var existingChatRoom1 = _context!.ChatRoom.Find(2);
            var existingChatRoom2 = _context!.ChatRoom.Find(3);


            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Count.EqualTo(2));
            Assert.Multiple(() =>
            {
                Assert.That(result[0], Is.EqualTo(existingChatRoom1));
                Assert.That(result[1], Is.EqualTo(existingChatRoom2));
            });
        }

        [Test]
        public async Task UserControllers_GetUserChatRooms_ShouldReturnNone()
        {
            var userController = new UsersController(_context!);

            var ChatRooms = await userController.GetUserChatRooms(5) as OkObjectResult;

            Assert.That(ChatRooms, Is.Null);
        }
    }
}