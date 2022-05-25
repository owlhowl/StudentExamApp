using Microsoft.AspNetCore.Mvc;
using ModelsApi;
using StudentExamApi.DB;

namespace StudentExamApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswerController : ControllerBase
    {
        private readonly StudentExamDBContext _dbContext;

        public AnswerController(StudentExamDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IEnumerable<Answer> Get()
        {
            return _dbContext.Answers.Select(s => (Answer)s);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Answer>> Get(int id)
        {
            var answer = await _dbContext.Answers.FindAsync(id);
            if (answer == null)
                return NotFound();
            return Ok(answer);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post([FromBody] Answer answer)
        {
            var newAnswer = answer;
            _dbContext.Answers.Add(newAnswer);
            await _dbContext.SaveChangesAsync();
            return Ok(answer.Id);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Answer newAnswer)
        {
            var answer = await _dbContext.Answers.FindAsync(id);
            if (answer == null)
                return NotFound();
            _dbContext.Entry(answer).CurrentValues.SetValues(newAnswer);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
