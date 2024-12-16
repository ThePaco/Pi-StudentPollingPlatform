using AutoMapper;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Dtos;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PollController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly PiSudentPollingPlatContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<PollController> _logger;

        public PollController(IConfiguration configuration, PiSudentPollingPlatContext context, IMapper mapper, ILogger<PollController> logger)
        {
            _configuration = configuration;
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        private async Task LogAsync(string level, string message)
        {
            var log = new Log
            {
                Timestamp = DateTime.UtcNow,
                Level = level,
                Message = message
            };

            _context.Logs.Add(log);
            await _context.SaveChangesAsync();
        }

        // GET: api/<PollController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PollDto>>> GetPolls()
        {
            _logger.LogInformation("Fetching all polls.");
            await LogAsync("Information", "Fetching all polls.");
            if (_context.Polls == null)
            {
                return NotFound();
            }
            var polls = await _context.Polls
                        .Include(p => p.Questions)
                        .ToListAsync();
            return _mapper.Map<List<PollDto>>(polls);
        }

        // GET api/<PollController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PollDto>> GetPoll(int id)
        {
            _logger.LogInformation($"Fetching poll with ID: {id}");
            await LogAsync("Information", $"Fetching poll with ID: {id}");
            if (_context.Polls == null)
            {
                return NotFound();
            }
            var poll = await _context.Polls.FindAsync(id);

            if (poll == null)
            {
                return NotFound();
            }

            return _mapper.Map<PollDto>(poll);
        }

        // GET Search

        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<PollDto>>> Search(int page, int size, string? orderBy, string? direction, string? filter)
        {
            var polls = await _context.Polls.ToListAsync();
            IEnumerable<Poll> ordered;
            IEnumerable<Poll> filteredPolls;

            if (filter != null)
            {
                filteredPolls = polls.Where(x =>
                    x.Title.Contains(filter, StringComparison.InvariantCultureIgnoreCase));
            }
            else
            {
                filteredPolls = polls;
            }

            if (string.Compare(orderBy, "id", true) == 0)
            {
                ordered = filteredPolls.OrderBy(x => x.Id);
            }
            else if (string.Compare(orderBy, "title", true) == 0)
            {
                ordered = filteredPolls.OrderBy(x => x.Title);
            }
            else if (string.Compare(orderBy, "tekst", true) == 0)
            {
                ordered = filteredPolls.OrderBy(x => x.Tekst);
            }
            else
            {
                ordered = filteredPolls.OrderBy(x => x.Id);
            }

            if (string.Compare(direction, "desc", true) == 0)
            {
                ordered = ordered.Reverse();
            }


            var retVal = ordered.Skip((page - 1) * size).Take(size);


            return Ok(_mapper.Map<List<PollDto>>(retVal));
        }

        // POST api/<PollController>
        [HttpPost]
        public async Task<ActionResult<PollDto>> PostPoll(PollDto poll)
        {
            if (_context.Polls == null)
            {
                return Problem("Entity set 'PiSudentPollingPlatContext.Polls'  is null.");
            }

            var newPoll = _mapper.Map<Poll>(poll);
            _context.Polls.Add(newPoll);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Created a new poll with ID: {newPoll.Id}");
            await LogAsync("Information", $"Created a new poll with ID: {newPoll.Id}");


            return CreatedAtAction("GetPoll", new { id = newPoll.Id }, _mapper.Map<PollDto>(newPoll));
        }

        // PUT api/<PollController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPoll(int id, PollDto pollDto)
        {
            Poll poll = _mapper.Map<Poll>(pollDto);
            if (id != poll.Id)
            {
                _logger.LogWarning($"Poll ID mismatch. Request ID: {id}, Poll ID: {poll.Id}");
                await LogAsync("Warning", $"Poll ID mismatch. Request ID: {id}, Poll ID: {poll.Id}");
                return BadRequest();
            }

            _context.Entry(poll).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Updated poll with ID: {poll.Id}");
                await LogAsync("Information", $"Updated poll with ID: {poll.Id}");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PollExists(id))
                {
                    _logger.LogWarning($"Poll with ID {id} not found during update.");
                    await LogAsync("Warning", $"Poll with ID {id} not found during update.");
                    return NotFound();
                }
                else
                {
                    _logger.LogError($"Concurrency error while updating poll with ID {id}");
                    await LogAsync("Error", $"Concurrency error while updating poll with ID {id}");
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE api/<PollController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePoll(int id)
        {
            if (_context.Polls == null)
            {
                return NotFound();
            }
            var poll = await _context.Polls.FindAsync(id);
            if (poll == null)
            {
                _logger.LogWarning($"Poll with ID {id} not found during deletion.");
                await LogAsync("Warning", $"Poll with ID {id} not found during deletion.");
                return NotFound();
            }

            var hasDependentQuestions = await _context.Questions.AnyAsync(p => p.PollId == id);
            if (hasDependentQuestions)
            {
                return BadRequest("Cannot delete this poll because it is referenced by questions.");
            }

            _context.Polls.Remove(poll);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Deleted poll with ID: {id}");
            await LogAsync("Information", $"Deleted poll with ID: {id}");

            return NoContent();
        }

        // GET: api/logs/get/N
        [HttpGet("logs/get/{n?}")]
        public async Task<ActionResult<IEnumerable<Log>>> GetLogs(int n = 10)
        {
            var logs = await _context.Logs
                .OrderByDescending(l => l.Timestamp)
                .Take(n)
                .ToListAsync();

            return Ok(logs);
        }

        // GET: api/logs/count
        [HttpGet("logs/count")]
        public async Task<ActionResult<int>> GetLogCount()
        {
            var count = await _context.Logs.CountAsync();
            return Ok(count);
        }

        private bool PollExists(int id)
        {
            return (_context.Polls?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
