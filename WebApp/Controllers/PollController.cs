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
            return View();
        }

        // POST: PollController/Delete/5
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
    }
}
