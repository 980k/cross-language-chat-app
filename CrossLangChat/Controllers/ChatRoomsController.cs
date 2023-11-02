using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CrossLangChat.Data;
using CrossLangChat.Models;
using CrossLangChat.Services;
using NuGet.Packaging;

namespace CrossLangChat.Controllers
{
    public class ChatRoomsController : Controller
    {
        private readonly CrossLangChatContext _context;

        private readonly DeepLTranslationService _translationService;

        public ChatRoomsController(CrossLangChatContext context, DeepLTranslationService translationService)
        {
            _context = context;
            _translationService = translationService;
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

        // POST: ChatRooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ChatRoom == null)
            {
                return Problem("Entity set 'CrossLangChatContext.ChatRoom'  is null.");
            }
            var chatRoom = await _context.ChatRoom.FindAsync(id);
            if (chatRoom != null)
            {
                _context.ChatRoom.Remove(chatRoom);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChatRoomExists(int id)
        {
          return (_context.ChatRoom?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        /********************
        *** CUSTOM ROUTES ***
        *********************/

        // GET: ChatRooms/{Id}
        [HttpGet, ActionName("GetChatRoom")]
        public async Task<IActionResult> GetChatRoom(int? id) 
        {
            var userId = HttpContext.Session.GetInt32("Id");
            var username = HttpContext.Session.GetString("Username");

            if(userId == null || username == null)
            {
                return RedirectToAction("Login", "Accounts");
            }

            var user = await _context.User
                .Include(u => u.ChatRooms)
                .FirstOrDefaultAsync(u => u.Id == userId);

            var chatRoom = await _context.ChatRoom
                .Include(c => c.Messages)
                .FirstOrDefaultAsync(c => c.Id == id);

            List<string> translatedMessages = new List<string>();

            if(chatRoom!.Messages!.Count > 0){
                var originalMessages = chatRoom!.Messages!.Select(m => m.Content).ToList();

                 string[] nonNullableOriginalMessages = originalMessages
                    .Where(message => message != null)
                    .Select(message => message!)
                    .ToArray();

                translatedMessages = await _translationService.TranslateAsync(nonNullableOriginalMessages, user!.Language!);
            }

            if (user != null && chatRoom != null)
            {
                var chatRooms = user.ChatRooms.ToList();
                ViewData["ChatRooms"] = chatRooms; 
                ViewData["Translations"] = translatedMessages;
                return View("ChatRoom", chatRoom);
            }

            return NotFound("Chat room not found.");
        }

        // GET : ChatRooms/CreateChatRoom
        [HttpGet, ActionName("Create")]
        public IActionResult CreateChatRoom() {
            return View("CreateChatRoom");
        }

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
        [HttpPost, ActionName("DeleteChatRoom")]
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

                    return RedirectToAction("Index", "Home");
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

        // POST: ChatRooms/AddUser
        [HttpPost, ActionName("AddUser")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditChatRoomAddUser(string username, int Id) 
        {
            try 
            {
                var user = await _context.User.FirstOrDefaultAsync(u => u.Username == username);
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

                return RedirectToAction("Index", "Home");
            }
            catch 
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
