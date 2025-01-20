using AutoMapper;
using DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class UserController : Controller
    {
        private readonly PiSudentPollingPlatContext _context;
        private readonly IMapper _mapper;

        public UserController(PiSudentPollingPlatContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IActionResult ProfileDetails()
        {
            var username = _context.Users.FirstOrDefault().Username;

            var userDb = _context.Users.FirstOrDefault(x => x.Username == username);
            var userVm = new VMUser
            {
                Id = userDb.Id,
                Username = userDb.Username,
                FirstName = userDb.FirstName,
                LastName = userDb.LastName,
                Email = userDb.Email,
                Password = userDb.Password,

            };

            return View(userVm);
        }

        public IActionResult ProfileEdit(int id)
        {
            var userDb = _context.Users.First(x => x.Id == id);
            var userVm = new VMUser
            {
                Id = userDb.Id,
                Username = userDb.Username,
                FirstName = userDb.FirstName,
                LastName = userDb.LastName,
                Email = userDb.Email,
                Password = userDb.Password,

            };

            return View(userVm);
        }

        [HttpPost]
        public IActionResult ProfileEdit(int id, VMUser userVm)
        {
            var userDb = _context.Users.First(x => x.Id == id);
            userDb.FirstName = userVm.FirstName;
            userDb.LastName = userVm.LastName;
            userDb.Email = userVm.Email;
            userDb.Password = userVm.Password;


            _context.SaveChanges();

            return RedirectToAction("ProfileDetails");
        }
        // GET: UserController
        public ActionResult Index()
        {
            return View();
        }

        // GET: UserController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public JsonResult GetProfileData(int id)
        {
            var userDb = _context.Users.First(x => x.Id == id);
            return Json(new
            {
                userDb.FirstName,
                userDb.LastName,
                userDb.Email,
                userDb.Password

            });
        }


        [HttpPut]
        public IActionResult SetProfileData(int id, [FromBody] VMUser userVm)
        {
            var userDb = _context.Users.First(x => x.Id == id);
            userDb.FirstName = userVm.FirstName;
            userDb.LastName = userVm.LastName;
            userDb.Email = userVm.Email;
            userDb.Password = userVm.Password;


            _context.SaveChanges();

            return Ok();
        }
    }
}
