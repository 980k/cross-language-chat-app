using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CrossLangChat.Data;
using CrossLangChat.Models;
using CrossLangChat.ViewModels;

namespace CrossLangChat.Controllers
{
    public class AccountsController : Controller
    {
        private readonly CrossLangChatContext _context;

        public AccountsController(CrossLangChatContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View("Login");
        }

        // POST: Accounts/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Username,Password")] LoginViewModel model)
        {
            var user = await _context.User
            .FirstOrDefaultAsync(m => m.Username == model.Username && m.Password == model.Password);

            if(user == null) {
                ViewData["Error"] = "Invalid name or password.";
                return View();
            }

            HttpContext.Session.SetInt32("Id", user.Id);
            HttpContext.Session.SetString("Username", user.Username!);

            return RedirectToAction("Index", "Home");
        }
    }
}
