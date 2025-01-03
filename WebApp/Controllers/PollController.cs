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
        public async Task<IActionResult> Index(int? page, string? searchText, string? sortOrder)
        {
            int pageSize = 4;
            int pageNumber = page ?? 1;
            ViewData["CurrentSort"] = sortOrder;
            ViewData["DateSortParm"] = String.IsNullOrEmpty(sortOrder) ? "date_desc" : ""; 


            IQueryable<Poll> pollsQuery = _context.Polls
                .Include(p => p.Kolegij)
                .Include(p => p.Studij);

            if (!string.IsNullOrEmpty(searchText))
            {
                pollsQuery = pollsQuery.Where(p =>
                    p.Title.Contains(searchText) ||
                    (p.Kolegij != null && p.Kolegij.KolegijName.Contains(searchText)) ||
                    (p.Studij != null && p.Studij.StudijName.Contains(searchText))
                );
            }

            // Apply sorting
            switch (sortOrder)
            {
                case "date_desc":
                    pollsQuery = pollsQuery.OrderByDescending(p => p.PollDate);
                    break;
                default:
                    pollsQuery = pollsQuery.OrderBy(p => p.PollDate);
                    break;
            }

            var polls = await pollsQuery.ToListAsync();

            ViewData["pages"] = polls.Count / pageSize;
            Response.Cookies.Append("SearchText", searchText ?? "", new CookieOptions { Expires = DateTime.Now.AddDays(7) });
            ViewData["page"] = page;

            return View(polls.ToPagedList(pageNumber, pageSize));
        }

        // GET: PollController/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                var poll = _context.Polls.FirstOrDefault(x => x.Id == id);
                var pollVM = new VMPoll
                {
                    Id = poll.Id,
                    Title = poll.Title,
                    Tekst = poll.Tekst,
                    PollDate = poll.PollDate,
                    KolegijId = poll.KolegijId,
                    StudijId = poll.StudijId
                };

                return View(pollVM);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // GET: PollController/Create
        public ActionResult Create()
        {
            ViewBag.Kolegiji = new SelectList(_context.Kolegijs, "Idkolegij", "KolegijName");
            ViewBag.Studiji = new SelectList(_context.Studijs, "Idstudij", "StudijName");
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
                    ViewBag.Kolegiji = new SelectList(_context.Kolegijs, "Idkolegij", "KolegijName");
                    ViewBag.Studiji = new SelectList(_context.Studijs, "Idstudij", "StudijName");
                    return View(poll);
                }

                var existingPoll = await _context.Polls.FirstOrDefaultAsync(p => p.Title == poll.Title);
                if (existingPoll != null)
                {
                    ModelState.AddModelError("Name", "A poll with the same title already exists.");
                    ViewBag.Kolegiji = new SelectList(_context.Kolegijs, "Idkolegij", "KolegijName");
                    ViewBag.Studiji = new SelectList(_context.Studijs, "Idstudij", "StudijName");
                    return View(poll);
                }


                var newPoll = _mapper.Map<Poll>(poll);
                newPoll.PollDate = poll.PollDate;
                newPoll.KolegijId = poll.KolegijId;
                newPoll.StudijId = poll.StudijId;

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
        public async Task<IActionResult> Edit(int id)
        {

            var poll = await _context.Polls.FirstOrDefaultAsync(x => x.Id == id);
            if (poll == null)
            {
                return NotFound();
            }

            var pollVM = new VMPoll
            {
                Id = poll.Id,
                Title = poll.Title,
                Tekst = poll.Tekst,
                PollDate = poll.PollDate,
                KolegijId = poll.KolegijId,
                StudijId = poll.StudijId
            };

            ViewBag.Kolegiji = new SelectList(_context.Kolegijs, "Idkolegij", "KolegijName", poll.KolegijId);
            ViewBag.Studiji = new SelectList(_context.Studijs, "Idstudij", "StudijName", poll.StudijId);

            return View(pollVM);
        }

        // POST: PollController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, VMPoll pollVM)
        {
            if (id != pollVM.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Kolegiji = new SelectList(_context.Kolegijs, "Idkolegij", "KolegijName", pollVM.KolegijId);
                ViewBag.Studiji = new SelectList(_context.Studijs, "Idstudij", "StudijName", pollVM.StudijId);
                return View(pollVM);
            }

            var existingPoll = await _context.Polls.FirstOrDefaultAsync(p => p.Id != pollVM.Id && p.Title == pollVM.Title);
            if (existingPoll != null)
            {
                ModelState.AddModelError("Title", "A poll with the same title already exists.");
                ViewBag.Kolegiji = new SelectList(_context.Kolegijs, "Idkolegij", "KolegijName", pollVM.KolegijId);
                ViewBag.Studiji = new SelectList(_context.Studijs, "Idstudij", "StudijName", pollVM.StudijId);
                return View(pollVM);
            }

            var poll = await _context.Polls.FirstOrDefaultAsync(x => x.Id == id);
            if (poll == null)
            {
                return NotFound();
            }

            poll.Title = pollVM.Title;
            poll.Tekst = pollVM.Tekst;
            poll.PollDate = pollVM.PollDate;
            poll.KolegijId = pollVM.KolegijId;
            poll.StudijId = pollVM.StudijId;

            try
            {
                _context.Update(poll);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while updating the poll. Please try again.");
                ViewBag.Kolegiji = new SelectList(_context.Kolegijs, "Idkolegij", "KolegijName", pollVM.KolegijId);
                ViewBag.Studiji = new SelectList(_context.Studijs, "Idstudij", "StudijName", pollVM.StudijId);
                return View(pollVM);
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
