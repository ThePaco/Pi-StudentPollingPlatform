using AutoMapper;
using DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class QuestionController : Controller
    {

        private readonly PiSudentPollingPlatContext _context;
        private readonly IMapper _mapper;

        public QuestionController(PiSudentPollingPlatContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        // GET: QuestionController
        public ActionResult Index(int id)
        {
            try
            {
                var questionsVms = _context.Questions.Select(x => new VMQuestion
                {
                    Id = x.Id,
                    Tekst = x.Tekst,
                    PollId = x.PollId,

                }).ToList().Where(x => x.PollId.Equals(id));

                return View(questionsVms);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // GET: QuestionController/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                var question = _context.Questions.FirstOrDefault(x => x.Id == id);
                var questionVM = new VMQuestion
                {
                    Id = question.Id,
                    Tekst = question.Tekst,
                    PollId = question.PollId
                };

                return View(questionVM);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        // GET: QuestionController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: QuestionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VMQuestion question)
        {
            try
            {
                var newQuestion = new Question
            {
                Id = question.Id,
                Tekst = question.Tekst,
                PollId = question.PollId
            };

            _context.Questions.Add(newQuestion);

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
            catch
            {
                return View();
    }
}


        // GET: QuestionController/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                var question = _context.Questions.FirstOrDefault(x => x.Id == id);
                var questionVM = new VMQuestion
                {
                    Id = question.Id,
                    Tekst = question.Tekst,
                    PollId = question.PollId
                };

                return View(questionVM);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // POST: QuestionController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, VMQuestion question)
        {
            try
            {
                var dbQuestion = _context.Questions.FirstOrDefault(x => x.Id == id);
                dbQuestion.Id = question.Id;
                dbQuestion.Tekst = question.Tekst;
                dbQuestion.PollId = question.PollId;

                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // GET: QuestionController/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                var question = _context.Questions.FirstOrDefault(x => x.Id == id);
                var questionVM = new VMQuestion
                {
                    Id = question.Id,
                    Tekst = question.Tekst,
                    PollId = question.PollId
                };

                return View(questionVM);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // POST: QuestionController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, VMQuestion quesiton)
        {
            try
            {
                var dbComment = _context.Questions.FirstOrDefault(x => x.Id == id);

                _context.Questions.Remove(dbComment);

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
