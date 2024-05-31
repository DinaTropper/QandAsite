﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using QandAsite.Data;
using QandAsite.web.Models;
using System.Security.Claims;

namespace QandAsite.web.Controllers
{
    public class AccountController : Controller
    {
        private readonly string _connectionString;

        public AccountController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
        }

        public IActionResult Signup(User u)
        {
            UserViewModel vm = new()
            {
                User = u
            };
            return View(vm);
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Signup(User user, string password)
        {
            var db = new UserRepo(_connectionString);
            db.AddUser(user, password);
            return Redirect("/account/login");
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var repo = new UserRepo(_connectionString);
            var user = repo.Login(email, password);
            if (user == null)
            {
                TempData["Error"] = "Invalid login!";
                return Redirect("/account/login");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, email)
            };

            HttpContext.SignInAsync(new ClaimsPrincipal(
                new ClaimsIdentity(claims, "Cookies", ClaimTypes.Email, "role"))).Wait();

            return Redirect("/home/Index");
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync().Wait();
            return Redirect("/");
        }


    }

}

