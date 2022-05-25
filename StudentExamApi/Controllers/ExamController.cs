using Microsoft.AspNetCore.Mvc;
using StudentExamApi.DB;
using ModelsApi;

namespace StudentExamApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamController : ControllerBase
    {
        private readonly StudentExamDBContext _dbContext;

        public ExamController(StudentExamDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        [HttpGet]
        public IEnumerable<Exam> Get()
        {
            return _dbContext.Exams.Select(s => (Exam)s);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Exam>> Get(int id)
        {
            var exam = await _dbContext.Exams.FindAsync(id);
            if (exam == null)
                return NotFound();
            return Ok(exam);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post([FromBody] Exam exam)
        {
            var newExam = (Exam)exam;
            _dbContext.Exams.Add(newExam);
            await _dbContext.SaveChangesAsync();
            return Ok(exam.Id);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Exam newExam)
        {
            var exam = await _dbContext.Exams.FindAsync(id);
            if (exam == null)
                return NotFound();
            _dbContext.Entry(exam).CurrentValues.SetValues(newExam);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
