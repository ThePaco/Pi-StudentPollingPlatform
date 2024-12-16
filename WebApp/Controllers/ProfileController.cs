using DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class ProfileController : Controller
    {

        private readonly PiSudentPollingPlatContext _context;


        public ProfileController(PiSudentPollingPlatContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var username = HttpContext.User.Identity.Name;

            var userDb = _context.Users.FirstOrDefault(x => x.Username == username);
            if (userDb == null)
            {
                return NotFound();
            }

            var userProfile = new VMUser
            {
                Id = userDb.Id,
                Username = userDb.Username,
                Email = userDb.Email,
                FirstName = userDb.FirstName,
                LastName = userDb.LastName,

            };

            return View(userProfile);
        }

        [HttpPost]
        public JsonResult UpdateProfile([FromBody] VMUser model)
        {
            if (ModelState.IsValid)
            {
                var userDb = _context.Users.FirstOrDefault(x => x.Id == model.Id);
                if (userDb == null)
                {
                    return Json(new { success = false, message = "User not found." });
                }

                // Ažuriranje korisničkih podataka
                userDb.FirstName = model.FirstName;
                userDb.LastName = model.LastName;
                userDb.Email = model.Email;


                _context.SaveChanges();
                return Json(new { success = true, message = "Profile updated successfully!" });
            }

            return Json(new { success = false, message = "Invalid input data." });
        }
    }
}
