using AutoMapper;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Dtos;


namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly PiSudentPollingPlatContext _context;
        private readonly IMapper _mapper;

        public QuestionController(IConfiguration configuration, PiSudentPollingPlatContext context, IMapper mapper)
        {
            _configuration = configuration;
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<QuestionController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuestionDto>>> GetQuestions()
        {
            if (_context.Questions == null)
            {
                return NotFound();
            }
            var questions = await _context.Questions
                        .Include(q => q.Poll)
                        .ToListAsync();
            return _mapper.Map<List<QuestionDto>>(questions);
        }

        // GET api/<QuestionController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<QuestionDto>> GetQuestion(int id)
        {
            if (_context.Questions == null)
            {
                return NotFound();
            }
            var question = await _context.Questions.FindAsync(id);

            if (question == null)
            {
                return NotFound();
            }

            return _mapper.Map<QuestionDto>(question);
        }

        // GET Search

        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<QuestionDto>>> Search(int page, int size, string? orderBy, string? direction, string? filter)
        {
            var questions = await _context.Questions.ToListAsync();
            IEnumerable<Question> ordered;
            IEnumerable<Question> filteredQuestions;

            if (filter != null)
            {
                filteredQuestions = questions.Where(x =>
                    x.Tekst.Contains(filter, StringComparison.InvariantCultureIgnoreCase));
            }
            else
            {
                filteredQuestions = questions;
            }

            if (string.Compare(orderBy, "id", true) == 0)
            {
                ordered = filteredQuestions.OrderBy(x => x.Id);
            }
            else if (string.Compare(orderBy, "tekst", true) == 0)
            {
                ordered = filteredQuestions.OrderBy(x => x.Tekst);
            }
            else if (string.Compare(orderBy, "poll", true) == 0)
            {
                ordered = filteredQuestions.OrderBy(x => x.PollId);
            }
            else
            {
                ordered = filteredQuestions.OrderBy(x => x.Id);
            }

            if (string.Compare(direction, "desc", true) == 0)
            {
                ordered = ordered.Reverse();
            }


            var retVal = ordered.Skip((page - 1) * size).Take(size);


            return Ok(_mapper.Map<List<QuestionDto>>(retVal));
        }

        // POST api/<QuestionController>
        [HttpPost]
        public async Task<ActionResult<QuestionDto>> PostQuestion(QuestionDto questionDto)
        {
            var question = _mapper.Map<Question>(questionDto);

            if (_context.Questions == null)
            {
                return Problem("Entity set 'PiSudentPollingPlatContext.Questions'  is null.");
            }
            _context.Questions.Add(question);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetQuestion", new { id = question.Id }, question);
        }

        // PUT api/<QuestionController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuestion(int id, QuestionDto questionDto)
        {
            var question = _mapper.Map<Question>(questionDto);

            if (id != question.Id)
            {
                return BadRequest();
            }

            _context.Entry(question).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE api/<QuestionController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuestion(int id)
        {
            if (_context.Questions == null)
            {
                return NotFound();
            }

            var question = await _context.Questions.FindAsync(id);
            if (question == null)
            {
                return NotFound();
            }


            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool QuestionExists(int id)
        {
            return (_context.Questions?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
