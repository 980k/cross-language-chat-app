using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CrossLangChat.Data;
using CrossLangChat.Models;
using Microsoft.EntityFrameworkCore;


namespace CrossLangChat.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly CrossLangChatContext _context;


    public HomeController(ILogger<HomeController> logger, CrossLangChatContext context)
    {
        _logger = logger;
        _context = context;
    }

public async Task<IActionResult> Index()
{
    var Id = HttpContext.Session.GetInt32("Id");
    var Username = HttpContext.Session.GetString("Username");

    if (Id == null || Username == null)
    {
        return RedirectToAction("Login", "Accounts");
    }
    else
    {
        var user = await _context.User
            .Include(u => u.ChatRooms)
            .FirstOrDefaultAsync(u => u.Id == Id);

        if (user != null)
        {
            var chatRooms = user.ChatRooms.ToList();

            ViewData["ChatRooms"] = chatRooms; // Store chatRooms list in ViewData

            return View();
        }
        else
        {
            return Problem("User does not exist.");
        }
    }
}

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

                    return RedirectToAction("Index");
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

        



    // public IActionResult Index()
    // {

    //     var Id = HttpContext.Session.GetInt32("")




    //     ViewData["UserId"] = HttpContext.Session.GetInt32("Id");
    //     return View();
    // }
    

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
