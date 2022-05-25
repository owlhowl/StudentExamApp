using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelsApi;
using StudentExamApi.DB;

namespace StudentExamApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly StudentExamDBContext _dbContext;

        public QuestionController(StudentExamDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IEnumerable<Question> Get()
        {
            return _dbContext.Questions.Select(s => (Question)s);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Question>> Get(int id)
        {
            var question = await _dbContext.Questions.FindAsync(id);
            if (question == null)
                return NotFound();
            return Ok(question);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post([FromBody] Question question)
        {
            var newQuestion = (Question)question;
            _dbContext.Questions.Add(newQuestion);
            await _dbContext.SaveChangesAsync();
            return Ok(question.Id);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Question newQuestion)
        {
            var question = await _dbContext.Questions.FindAsync(id);
            if (question == null)
                return NotFound();
            _dbContext.Entry(question).CurrentValues.SetValues(newQuestion);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
