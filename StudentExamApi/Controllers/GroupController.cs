using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelsApi;
using StudentExamApi.DB;

namespace StudentExamApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly StudentExamDBContext _dbContext;

        public GroupController(StudentExamDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IEnumerable<Group> Get()
        {
            return _dbContext.Groups.Select(s => (Group)s);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Group>> Get(int id)
        {
            var subject = await _dbContext.Groups.FindAsync(id);
            if (subject == null)
                return NotFound();
            return Ok(subject);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post([FromBody] Group group)
        {
            var newGroup = (Group)group;
            _dbContext.Groups.Add(newGroup);
            await _dbContext.SaveChangesAsync();
            return Ok(group.Id);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Group newGroup)
        {
            var group = await _dbContext.Groups.FindAsync(id);
            if (group == null)
                return NotFound();
            _dbContext.Entry(group).CurrentValues.SetValues(newGroup);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
