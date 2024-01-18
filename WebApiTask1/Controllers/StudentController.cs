using Microsoft.AspNetCore.Mvc;
using WebApiTask1.Dtos;
using WebApiTask1.Entities;
using WebApiTask1.Services.Abstract;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiTask1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {

        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        // GET: api/<StudentController>
        [HttpGet]
        public IEnumerable<StudentDto> Get()
        {
            var items = _studentService.GetAll();
            var dataToReturn = items.Select(s=>
            {
                return new StudentDto
                {
                    Id = s.Id,
                    FullName = s.FullName,
                    Score = s.Score,
                    Age = s.Age,
                    SeriaNo = s.SeriaNo,
                };
            });
            return dataToReturn;
        }

        // GET api/<StudentController>/5
        [HttpGet("{id}")]
        public StudentDto Get(int id)
        {
            var item = _studentService.Get(s=>s.Id==id);
            return new StudentDto
            {
                Age = item.Age,
                FullName = item.FullName,
                Score = item.Score,
                Id = item.Id,
                SeriaNo = item.SeriaNo

            };
        }

        // POST api/<StudentController>
        [HttpPost]
        public IActionResult Post([FromBody] StudentAddDto value)
        {
            try
            {
                var entity = new Student
                {
                    Age = value.Age,
                    FullName = value.FullName,
                    Score = value.Score,
                    SeriaNo = value.SeriaNo
                };
                _studentService.Add(entity);
                return Ok(entity);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<StudentController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<StudentController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
