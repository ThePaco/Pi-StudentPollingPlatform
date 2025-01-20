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
        public async Task<IActionResult> Index(int? page, string? searchText, string? sortOrder, int? godinaFilter, int? studijFilter, int? kolegijFilter)
        {
            int pageSize = 4;
            int pageNumber = page ?? 1;
            ViewData["CurrentSort"] = sortOrder;
            ViewData["DateSortParm"] = string.IsNullOrEmpty(sortOrder) ? "date_desc" : "";

            // Include related data
            IQueryable<Poll> pollsQuery = _context.Polls
                .Include(p => p.Kolegij)
                .Include(p => p.Studij)
                .Include(p => p.Godina);

            // Apply search filter
            if (!string.IsNullOrEmpty(searchText))
            {
                pollsQuery = pollsQuery.Where(p =>
                    p.Title.Contains(searchText) ||
                    (p.Kolegij != null && p.Kolegij.KolegijName.Contains(searchText)) ||
                    (p.Studij != null && p.Studij.StudijName.Contains(searchText)) ||
                    (p.Godina != null && p.Godina.BrojGodine.ToString().Contains(searchText))
                );
            }

            // Apply filters
            if (godinaFilter.HasValue)
            {
                pollsQuery = pollsQuery.Where(p => p.GodinaId == godinaFilter);
            }
            if (studijFilter.HasValue)
            {
                pollsQuery = pollsQuery.Where(p => p.StudijId == studijFilter);
            }
            if (kolegijFilter.HasValue)
            {
                pollsQuery = pollsQuery.Where(p => p.KolegijId == kolegijFilter);
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

            // Populate ViewBag for filters
            ViewBag.Godine = new SelectList(await _context.Godinas.ToListAsync(), "Idgodina", "BrojGodine");
            ViewBag.Studiji = new SelectList(await _context.Studijs.ToListAsync(), "Idstudij", "StudijName");
            ViewBag.Kolegiji = new SelectList(await _context.Kolegijs.ToListAsync(), "Idkolegij", "KolegijName");

            ViewData["pages"] = polls.Count / pageSize;
            Response.Cookies.Append("SearchText", searchText ?? "", new CookieOptions { Expires = DateTime.Now.AddDays(7) });
            ViewData["page"] = page;

            return View(polls.ToPagedList(pageNumber, pageSize));
        }

        // GET: PollController/Create
        public ActionResult Create()
        {
            ViewBag.Kolegiji = new SelectList(_context.Kolegijs, "Idkolegij", "KolegijName");
            ViewBag.Studiji = new SelectList(_context.Studijs, "Idstudij", "StudijName");
            ViewBag.Godine = new SelectList(_context.Godinas, "Idgodina", "BrojGodine"); // Populate Godina dropdown
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
                    ViewBag.Godine = new SelectList(_context.Godinas, "Idgodina", "BrojGodine");
                    return View(poll);
                }

                var existingPoll = await _context.Polls.FirstOrDefaultAsync(p => p.Title == poll.Title);
                if (existingPoll != null)
                {
                    ModelState.AddModelError("Name", "A poll with the same title already exists.");
                    ViewBag.Kolegiji = new SelectList(_context.Kolegijs, "Idkolegij", "KolegijName");
                    ViewBag.Studiji = new SelectList(_context.Studijs, "Idstudij", "StudijName");
                    ViewBag.Godine = new SelectList(_context.Godinas, "Idgodina", "BrojGodine");
                    return View(poll);
                }

                var newPoll = _mapper.Map<Poll>(poll);
                newPoll.PollDate = poll.PollDate;
                newPoll.KolegijId = poll.KolegijId;
                newPoll.StudijId = poll.StudijId;
                newPoll.GodinaId = poll.GodinaId;

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
