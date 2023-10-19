using Microsoft.AspNetCore.Identity;
using CrossLangChat.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNetCore.Mvc;

namespace CrossLangChat.Controllers
{
    public class AccountsController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountsController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        
    }
}