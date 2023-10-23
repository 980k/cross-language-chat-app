using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using CrossLangChat.Data;
using CrossLangChat.Models;
using NuGet.Versioning;

namespace CrossLangChat.Controllers
{
    public class ChatRoomsController : Controller
    {
        private readonly CrossLangChatContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ChatRoomsController(CrossLangChatContext context,  IHttpContextAccessor httpContextAccessor )
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: ChatRooms
        public async Task<IActionResult> Index()
        {
              return _context.ChatRoom != null ? 
                          View(await _context.ChatRoom.ToListAsync()) :
                          Problem("Entity set 'CrossLangChatContext.ChatRoom'  is null.");
        }

        // GET: ChatRooms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ChatRoom == null)
            {
                return NotFound();
            }

            var chatRoom = await _context.ChatRoom
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chatRoom == null)
            {
                return NotFound();
            }

            return View(chatRoom);
        }

        // GET: ChatRooms/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ChatRooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RoomName")] ChatRoom chatRoom)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chatRoom);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(chatRoom);
        }

        // GET: ChatRooms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ChatRoom == null)
            {
                return NotFound();
            }

            var chatRoom = await _context.ChatRoom.FindAsync(id);
            if (chatRoom == null)
            {
                return NotFound();
            }
            return View(chatRoom);
        }

        // POST: ChatRooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RoomName")] ChatRoom chatRoom)
        {
            if (id != chatRoom.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chatRoom);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChatRoomExists(chatRoom.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(chatRoom);
        }

        // GET: ChatRooms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ChatRoom == null)
            {
                return NotFound();
            }

            var chatRoom = await _context.ChatRoom
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chatRoom == null)
            {
                return NotFound();
            }

            return View(chatRoom);
        }

        // // POST: ChatRooms/Delete/5
        // [HttpPost, ActionName("Delete")]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> DeleteConfirmed(int id)
        // {
        //     if (_context.ChatRoom == null)
        //     {
        //         return Problem("Entity set 'CrossLangChatContext.ChatRoom'  is null.");
        //     }
        //     var chatRoom = await _context.ChatRoom.FindAsync(id);
        //     if (chatRoom != null)
        //     {
        //         _context.ChatRoom.Remove(chatRoom);
        //     }
            
        //     await _context.SaveChangesAsync();
        //     return RedirectToAction(nameof(Index));
        // }


        //         public async Task<IActionResult> Create([Bind("Id,RoomName")] ChatRoom chatRoom)
        // {
        //     if (ModelState.IsValid)
        //     {
        //         _context.Add(chatRoom);
        //         await _context.SaveChangesAsync();
        //         return RedirectToAction(nameof(Index));
        //     }
        //     return View(chatRoom);
        // }


        /*
        ***
        CUSTOM ROUTES
        ***
        */

        // POST: ChatRooms/CreateChatRoom
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateChatRoom([Bind("Id,RoomName")] ChatRoom chatRoom)
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("Id");
                var user = await _context.User.FindAsync(userId);

                if (user == null)
                {
                    return NotFound("User not found");
                }

                if (ModelState.IsValid)
                {
                    user.ChatRooms.Add(chatRoom);
                    chatRoom.Users?.Add(user);
                    _context.Add(chatRoom);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // POST: ChatRooms/Delete/{Id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteChatRoom(int id)
        {
            try 
            {
                if(_context.ChatRoom == null) 
                {
                    return Problem("Entity set 'CrossLangChatContext.ChatRoom'  is null.");
                }

                var chatRoom = await _context.ChatRoom.FindAsync(id);

                if(chatRoom != null) {
                    foreach (var user in chatRoom.Users!) 
                    {
                        user.ChatRooms.Remove(chatRoom);
                    }

                    _context.ChatRoom.Remove(chatRoom);

                    await _context.SaveChangesAsync();

                    return Ok("Chat room and associated users removed.");
                } 
                else 
                {
                    return NotFound("Chat room not found.");

                }
            } 
            catch 
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPost("ChatRooms/User/{userId}/Edit/{Id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditChatRoomAddUser(int userId, int Id) 
        {
            try 
            {
                var user = await _context.User.FindAsync(userId);
                var chatRoom = await _context.ChatRoom.FindAsync(Id);

                if (user == null || chatRoom == null) 
                {
                    return NotFound();
                }

                if (!chatRoom!.Users!.Contains(user))
                {
                    chatRoom.Users.Add(user);
                    user.ChatRooms.Add(chatRoom);
                    await _context.SaveChangesAsync();
                }

                return Ok("User added to chat room");
            }
            catch 
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // [HttpGet("ChatRooms/User/Create")]
        // public IActionResult CreateChatRoom(int userId)
        // {
        //     // You can perform any necessary logic here, such as loading data for the form.
        //     // For example, you might load additional data based on the userId parameter.
            
        //     // Assuming you have a view model, you can pass it to the view like this:
        //     // var viewModel = new YourViewModel();
        //     // return View(viewModel);
            
        //     // If you just want to display the form without any specific data, you can simply return the view.
        //     return View("CreateChat");
        // }

        private bool ChatRoomExists(int id)
        {
          return (_context.ChatRoom?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
