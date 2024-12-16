using AutoMapper;
using DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.ViewModels;
using X.PagedList.Extensions;

namespace WebApp.Controllers
{
    public class PollController : Controller
    {
        private readonly PiSudentPollingPlatContext _context;
        private readonly IMapper _mapper;

        public PollController(PiSudentPollingPlatContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        // GET: PollController
        public async Task<IActionResult> Index(int? page, string? searchText)
        {
            int pageSize = 4;
            int pageNumber = page ?? 1;
            ViewData["pages"] = pageNumber;
            List<Poll> piSudentPollingPlatContextPaged = null;
            if (searchText != null)
            {
                piSudentPollingPlatContextPaged =
                    await _context.Polls
                    .Where(p => p.Title.Contains(searchText))
                    .OrderBy(p => p.Title)
                    .ToListAsync();



                ViewData["pages"] = piSudentPollingPlatContextPaged.Count() / pageSize;
                CookieOptions options = new CookieOptions();
                options.Expires = DateTime.Now.AddDays(7);
                Response.Cookies.Append("SearchText", searchText, options);
                ViewData["page"] = page;

                return View(piSudentPollingPlatContextPaged.ToPagedList(pageNumber, pageSize));
            }
            piSudentPollingPlatContextPaged =
                await _context.Polls
                .OrderBy(p => p.Title)
                .ToListAsync();

            Response.Cookies.Delete("SearchText");
            ViewData["pages"] = piSudentPollingPlatContextPaged.Count() / pageSize;
            ViewData["page"] = page;
            return View(piSudentPollingPlatContextPaged.ToPagedList(pageNumber, pageSize));
        }

        // GET: PollController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PollController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PollController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(VMPoll poll)
        {
            try
            { 
                if (!ModelState.IsValid)
                {


                    return View();
                }
            var existingPoll = await _context.Polls.FirstOrDefaultAsync(p => p.Title == poll.Title);

            if (existingPoll != null)
            {
                ModelState.AddModelError("Name", "A poll with the same title already exists.");
                return View(poll);
            }


                var newPoll = _mapper.Map<Poll>(poll);
                _context.Polls.Add(newPoll);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));

            }   
            catch (Exception ex)
            {

                throw ex;
            }
        }

        // GET: PollController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PollController/Edit/5
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

        // GET: PollController/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                var poll = _context.Polls.FirstOrDefault(x => x.Id == id);
                var pollVM = new VMPoll
                {
                    Id = poll.Id,
                    Title = poll.Title,
                    Tekst = poll.Tekst,

                };

                return View(pollVM);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // POST: PollController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, VMPoll poll)
        {
            try
            {
                var dbPoll = _context.Polls.FirstOrDefault(x => x.Id == id);

                _context.Polls.Remove(dbPoll);

                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
