using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CrossLangChat.Data;
using CrossLangChat.Models;

namespace CrossLangChat.Controllers
{
    public class MessagesController : Controller
    {
        private readonly CrossLangChatContext _context;

        public MessagesController(CrossLangChatContext context)
        {
            _context = context;
        }

        // GET: Messages
        public async Task<IActionResult> Index()
        {
            var crossLangChatContext = _context.Message.Include(m => m.ChatRoom).Include(m => m.Sender);
            return View(await crossLangChatContext.ToListAsync());
        }

        // GET: Messages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Message == null)
            {
                return NotFound();
            }

            var message = await _context.Message
                .Include(m => m.ChatRoom)
                .Include(m => m.Sender)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }

        // GET: Messages/Create
        public IActionResult Create()
        {
            ViewData["ChatRoomId"] = new SelectList(_context.ChatRoom, "Id", "RoomName");
            ViewData["SenderId"] = new SelectList(_context.User, "Id", "Password");
            return View();
        }

        // POST: Messages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Content,SenderId,ChatRoomId")] Message message)
        {
            if (ModelState.IsValid)
            {
                _context.Add(message);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ChatRoomId"] = new SelectList(_context.ChatRoom, "Id", "RoomName", message.ChatRoomId);
            ViewData["SenderId"] = new SelectList(_context.User, "Id", "Password", message.SenderId);
            return View(message);
        }

        // GET: Messages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Message == null)
            {
                return NotFound();
            }

            var message = await _context.Message.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }
            ViewData["ChatRoomId"] = new SelectList(_context.ChatRoom, "Id", "RoomName", message.ChatRoomId);
            ViewData["SenderId"] = new SelectList(_context.User, "Id", "Password", message.SenderId);
            return View(message);
        }

        // POST: Messages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Content,SenderId,ChatRoomId")] Message message)
        {
            if (id != message.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(message);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MessageExists(message.Id))
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
            ViewData["ChatRoomId"] = new SelectList(_context.ChatRoom, "Id", "RoomName", message.ChatRoomId);
            ViewData["SenderId"] = new SelectList(_context.User, "Id", "Password", message.SenderId);
            return View(message);
        }

        // GET: Messages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Message == null)
            {
                return NotFound();
            }

            var message = await _context.Message
                .Include(m => m.ChatRoom)
                .Include(m => m.Sender)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }

        // POST: Messages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Message == null)
            {
                return Problem("Entity set 'CrossLangChatContext.Message'  is null.");
            }
            var message = await _context.Message.FindAsync(id);
            if (message != null)
            {
                _context.Message.Remove(message);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MessageExists(int id)
        {
          return (_context.Message?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
