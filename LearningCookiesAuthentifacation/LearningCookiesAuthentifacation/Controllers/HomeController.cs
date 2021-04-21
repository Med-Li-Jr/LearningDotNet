using LearningCookiesAuthentifacation.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LearningCookiesAuthentifacation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }


        public IActionResult Index()
        {
            var cook = HttpContext.Request.Cookies["UserLoginCookie"];
            var user = User.Claims.FirstOrDefault(x => x.Type == "user_id")?.Value;
             
            return View();
        }

        [Authorize]
        public ActionResult Users()
        {
            var uses = new Users();
            return View(uses.GetUsers());
        }


        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login([Bind] Users user)
        {
            // username = anet  
            var users = new Users();
            var allUsers = users.GetUsers().FirstOrDefault();
            if (users.GetUsers().Any(u => u.UserName == user.UserName))
            {
                var userClaims = new List<Claim>()
                {
                new Claim("user_id", "" + Guid.NewGuid()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, "anet@test.com"),
                new Claim("role", "admin"),
                 };

                var grandmaIdentity = new ClaimsIdentity(userClaims, "User Identity");

                var userPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity });
                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    IsPersistent = false,
                    ExpiresUtc = DateTime.UtcNow.AddSeconds(10)
                };
                HttpContext.SignInAsync(userPrincipal, authProperties);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.MessageError = "user not found";
            }

            return View(user);
        }





        public async Task<IActionResult> Privacy()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
