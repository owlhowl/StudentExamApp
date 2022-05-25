using Microsoft.AspNetCore.Mvc;
using ModelsApi;
using StudentExamApi.DB;

namespace StudentExamApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly StudentExamDBContext _dbContext;

        public SubjectController(StudentExamDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IEnumerable<Subject> Get()
        {
            return _dbContext.Subjects.Select(s => (Subject)s);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Subject>> Get(int id)
        {
            var subject = await _dbContext.Subjects.FindAsync(id);
            if (subject == null)
                return NotFound();
            return Ok(subject);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post([FromBody] Subject subject)
        {
            var newSubject = (Subject)subject;
            _dbContext.Subjects.Add(newSubject);
            await _dbContext.SaveChangesAsync();
            return Ok(subject.Id);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Subject newSubject)
        {
            var subject = await _dbContext.Subjects.FindAsync(id);
            if (subject == null)
                return NotFound();
            _dbContext.Entry(subject).CurrentValues.SetValues(newSubject);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
