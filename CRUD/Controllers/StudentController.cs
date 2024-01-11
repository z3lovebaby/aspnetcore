using CRUD.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly CRUDContext _studentContext;

        public StudentController(CRUDContext studentContext)
        {
            _studentContext = studentContext;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudent()
        {
            if(_studentContext.Student == null)
            {
                return NotFound();
            }
            var students = await _studentContext.Student.ToListAsync();
            return Ok(students);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            var student = await _studentContext.Student.FindAsync(id);

            if (student == null)
            {
                return NotFound(); // 404 Not Found
            }

            return Ok(student); // 200 OK
        }

        [HttpPost]
        public async Task<ActionResult<Student>> PostBrand(Student student)
        {
            if (student == null)
            {
                return BadRequest(); // 400 Bad Request
            }

            _studentContext.Student.Add(student);

            try
            {
                await _studentContext.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // Handle database-related exceptions
                return StatusCode(StatusCodes.Status500InternalServerError); // 500 Internal Server Error
            }

            return Ok("Adding successfully"); // 201 Created
        }
        [HttpPut]
        public async Task<ActionResult> PutStudent(int id,Student student)
        {
            if (id != student.ID)
            {
                Console.WriteLine("Invalid ID. Returning BadRequest.");
                return BadRequest();
            }
            Console.WriteLine(id);
            Console.WriteLine(student.ID);
            _studentContext.Entry(student).State = EntityState.Modified;
            try
            {
                await _studentContext.SaveChangesAsync();
                Console.WriteLine("Student data has been successfully updated.");
            }
            catch (DbUpdateException)
            {
                if (!StudentAvailable(id))
                {
                    Console.WriteLine("Student not found. Returning NotFound.");
                    return NotFound();
                }
                else
                {
                    Console.WriteLine("An error occurred while updating student data.");
                    throw;
                }
            }
            Console.WriteLine("Update request has been successfully processed.");
            return Ok("Update Successfully");
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteStudent(int id)
        {
            if (_studentContext.Student == null)
            {
                return NotFound();
            }
            var student = await _studentContext.Student.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            _studentContext.Student.Remove(student);
            await _studentContext.SaveChangesAsync();
            return Ok();

        }

        private bool StudentAvailable(int id)
        {
            return (_studentContext.Student?.Any(x => x.ID == id)).GetValueOrDefault();
        }

    }

}
