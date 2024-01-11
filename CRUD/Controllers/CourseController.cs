using CRUD.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly CRUDContext _dbContext;

        public CourseController(CRUDContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
        {
            var courses = await _dbContext.Course.ToListAsync();
            return Ok(courses);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourse(int id)
        {
            var course = await _dbContext.Course.FindAsync(id);

            if (course == null)
            {
                return NotFound(); // 404 Not Found
            }

            return Ok(course); // 200 OK
        }

        [HttpPost]
        [Authorize(Roles = "Writer")]
        public async Task<ActionResult<Course>> PostCourse(Course course)
        {
            if (course == null)
            {
                return BadRequest(); // 400 Bad Request
            }

            _dbContext.Course.Add(course);

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // Handle database-related exceptions
                return StatusCode(StatusCodes.Status500InternalServerError); // 500 Internal Server Error
            }

            return Ok("Add successfully"); // 201 Created
        }
        [HttpPut]
        [Authorize(Roles = "Writer")]
        public async Task<ActionResult> PutCourse(int id, Course course)
        {
            if (id != course.ID)
            {
                return BadRequest();
            }
            _dbContext.Entry(course).State = EntityState.Modified;
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (!CourseAvailable(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }

            }
            return Ok("Update successfully");
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Writer")]
        public async Task<ActionResult> DeleteBand(int id)
        {
            if (_dbContext.Course == null)
            {
                return NotFound();
            }
            var course = await _dbContext.Course.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            _dbContext.Course.Remove(course);
            await _dbContext.SaveChangesAsync();
            return Ok("Delete successfull");

        }

        private bool CourseAvailable(int id)
        {
            return (_dbContext.Course?.Any(x => x.ID == id)).GetValueOrDefault();
        }
    }
}
