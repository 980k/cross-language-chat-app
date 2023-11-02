using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CrossLangChat.Data;
using CrossLangChat.Models;
using CrossLangChat.ViewModels;
using CrossLangChat.Services;
using Microsoft.EntityFrameworkCore;

namespace CrossLangChat.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly CrossLangChatContext _context;
    private readonly DeepLTranslationService _translationService;


    public HomeController(ILogger<HomeController> logger, CrossLangChatContext context, DeepLTranslationService translationService)
    {
        _logger = logger;
        _context = context;
        _translationService = translationService;
    }

    public async Task<IActionResult> Index()
    {
        var Id = HttpContext.Session.GetInt32("Id");
        var Username = HttpContext.Session.GetString("Username");

        if (Id == null || Username == null)
        {
            return RedirectToAction("Login", "Accounts");
        }

        var user = await _context.User
            .Include(u => u.ChatRooms)
            .FirstOrDefaultAsync(u => u.Id == Id);

        if (user != null)
        {
            var chatRooms = user.ChatRooms.ToList();

            ViewData["ChatRooms"] = chatRooms;

            return View();
        }
        else
        {
            return Problem("User does not exist.");
        }
    }

        [HttpGet]
        public IActionResult TranslateTest()
        {
            return View(new TranslationViewModel());
        }
        
        [HttpPost]
        public async Task<IActionResult> TranslateTest(TranslationViewModel model)
        {
            if (ModelState.IsValid && model.Text != null && model.TargetLanguage != null)
            {
                try
                {
                    // Call the TranslateAsync method with non-nullable string arrays
                    model.TranslatedTexts = await _translationService.TranslateAsync(new[] { model.Text! }, model.TargetLanguage!);
                }
                catch (Exception ex)
                {
                    // Handle translation service exceptions (e.g., API errors) here
                    ModelState.AddModelError(string.Empty, $"Translation failed: {ex.Message}");
                }
            }
            else
            {
                // Handle null input values, if necessary
                ModelState.AddModelError(string.Empty, "Invalid input values.");
            }

    return View(model);
}


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
