using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelsApi;
using StudentExamApi.DB;

namespace StudentExamApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly StudentExamDBContext _dbContext;

        public StudentController(StudentExamDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IEnumerable<Student> Get()
        {
            return _dbContext.Students.Select(s => (Student)s);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> Get(int id)
        {
            var student = await _dbContext.Students.FindAsync(id);
            if (student == null)
                return NotFound();
            return Ok(student);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post([FromBody] Student student)
        {
            var newStudent = (Student)student;
            _dbContext.Students.Add(newStudent);
            await _dbContext.SaveChangesAsync();
            return Ok(student.Id);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Student newStudent)
        {
            var student = await _dbContext.Students.FindAsync(id);
            if (student == null)
                return NotFound();
            _dbContext.Entry(student).CurrentValues.SetValues(newStudent);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
