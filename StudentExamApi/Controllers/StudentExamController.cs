using Microsoft.AspNetCore.Mvc;
using ModelsApi;
using StudentExamApi.DB;

namespace StudentExamApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentExamController : ControllerBase
    {
        private readonly StudentExamDBContext _dbContext;

        public StudentExamController(StudentExamDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IEnumerable<ModelsApi.StudentExam> Get()
        {
            return _dbContext.StudentExams.Select(s => (ModelsApi.StudentExam)s);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ModelsApi.StudentExam>> Get(int id)
        {
            var studentExam = await _dbContext.StudentExams.FindAsync(id);
            if (studentExam == null)
                return NotFound();
            return Ok(studentExam);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post([FromBody] ModelsApi.StudentExam studentExam)
        {
            var newStudentExam = (StudentExam)studentExam;
            _dbContext.StudentExams.Add(newStudentExam);
            await _dbContext.SaveChangesAsync();
            return Ok(studentExam.Id);
        }
    }
}
