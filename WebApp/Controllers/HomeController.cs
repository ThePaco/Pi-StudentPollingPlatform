using WebApp.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using static NuGet.Protocol.Core.Types.Repository;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using DAL.Models;
using WebApp.Models;



namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly PiSudentPollingPlatContext _context;

        public HomeController(ILogger<HomeController> logger, PiSudentPollingPlatContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
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

        [AllowAnonymous]
        public ActionResult Login()
        {

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(VMLogin login)
        {
            try
            {
                if (Authenticate(login.Username, login.Password))
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name,"Username"),
                        new Claim(ClaimTypes.Role, "Administrator")

                    };
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(5)
                    };

                    HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties).Wait();


                }
                return RedirectToAction("Index", "Poll");
            }
            catch
            {
                return View();
            }

        }

        private bool Authenticate(string name, string pass)
        {
            var username = _context.Users.FirstOrDefault(x => x.Username == name && x.Password == pass);

            if (username != null)
            {
                return true;
            }

            return false;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOut()
        {
            try
            {
                HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).Wait();
                return RedirectToAction(nameof(Login));
            }
            catch
            {
                return View();
            }
        }
    }
}
