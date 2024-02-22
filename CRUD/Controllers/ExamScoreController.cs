using CRUD.Filter;
using CRUD.Helpers;
using CRUD.Models;
using CRUD.Repositories;
using CRUD.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamScoreController : ControllerBase
    {
        private readonly CRUDContext _dbContext;
        private readonly IUriService uriService;

        public ExamScoreController(CRUDContext dbContext, IUriService uriService)
        {
            _dbContext = dbContext;
            this.uriService = uriService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExamScore>>> GetBrands([FromQuery] PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var examscores = await _dbContext.ExamScores
                .OrderBy(e => e.Id)  // Order by exam score ID (you should replace this with your actual ordering criteria)
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .Include(e => e.Student)
                .Include(e => e.Course)
                .ToListAsync();

            var totalRecords = await _dbContext.ExamScores.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<ExamScore>(examscores, validFilter, totalRecords, uriService, route);
            //return Ok(new PagedResponse<List<ExamScore>>(examscores, validFilter.PageNumber, validFilter.PageSize));
            return Ok(pagedReponse);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ExamScore>> GetBrand(int id)
        {
            var examscore = await _dbContext.ExamScores
            .Include(e => e.Student)
            .Include(e => e.Course)
            .FirstOrDefaultAsync(e => e.Id == id);

            if (examscore == null)
            {
                return NotFound(); // 404 Not Found
            }
            return Ok(new Response<ExamScore>(examscore));
            //return Ok(examscore); // 200 OK
        }
        [HttpGet("GetExamScoresByCourse/{cId}")]
        public async Task<ActionResult<IEnumerable<ExamScoreWithStudent>>> GetExamScoresByCourse(int cId)
        {
            // Lấy danh sách ExamScores có cId tương ứng
            var examScores = await _dbContext.ExamScores
                .Where(e => e.CId == cId)
                .ToListAsync();

            if (examScores == null || examScores.Count == 0)
            {
                return NotFound(); // Trả về 404 Not Found nếu không tìm thấy kết quả
            }

            // Tạo danh sách ExamScoreWithStudent để chứa thông tin cần trả về
            var examScoresWithStudents = new List<ExamScoreWithStudent>();

            foreach (var examScore in examScores)
            {
                // Lấy thông tin sinh viên tương ứng dựa trên stId
                var student = await _dbContext.Student.FindAsync(examScore.StId);

                // Lấy thông tin khóa học tương ứng dựa trên CId
                var course = await _dbContext.Course.FindAsync(examScore.CId);

                // Tạo đối tượng ExamScoreWithStudent và gán thông tin
                var examScoreWithStudent = new ExamScoreWithStudent
                {
                    ID = examScore.StId,
                    Name = student?.Name,
                    Course = course?.NameCourse, // Thay thế bằng tên khóa học từ bảng Course
                    Score = examScore.Score,
                };

                examScoresWithStudents.Add(examScoreWithStudent);
            }

            return Ok(examScoresWithStudents);
        }
        [HttpGet("GetExamScoresByStudent/{stId}")]
        public async Task<ActionResult<IEnumerable<ExamScoreWithStudent>>> GetExamScoresByStudent(int stId)
        {
            // Lấy danh sách ExamScores có cId tương ứng
            var examScores = await _dbContext.ExamScores
                .Where(e => e.StId == stId)
                .ToListAsync();

            if (examScores == null || examScores.Count == 0)
            {
                return NotFound(); // Trả về 404 Not Found nếu không tìm thấy kết quả
            }

            // Tạo danh sách ExamScoreWithStudent để chứa thông tin cần trả về
            var examScoresWithStudents = new List<ExamScoreWithStudent>();

            foreach (var examScore in examScores)
            {
                // Lấy thông tin sinh viên tương ứng dựa trên stId
                var student = await _dbContext.Student.FindAsync(examScore.StId);

                // Lấy thông tin khóa học tương ứng dựa trên CId
                var course = await _dbContext.Course.FindAsync(examScore.CId);

                // Tạo đối tượng ExamScoreWithStudent và gán thông tin
                var examScoreWithStudent = new ExamScoreWithStudent
                {
                    ID = examScore.StId,
                    Name = student?.Name,
                    Course = course?.NameCourse, // Thay thế bằng tên khóa học từ bảng Course
                    Score = examScore.Score,
                };

                examScoresWithStudents.Add(examScoreWithStudent);
            }

            return Ok(examScoresWithStudents);
        }

        // [HttpPost]
        // public async Task<ActionResult<ExamScore>> PostExamScore(ExamScore examscore)
        // {
        //     if (examscore == null || examscore.Student == null || examscore.Course == null)
        //     {
        //         return BadRequest("Add failed !!"); // 400 Bad Request
        //     }

        //     var existingExamScore = _dbContext.ExamScores
        //         .FirstOrDefault(e => e.StId == examscore.StId && e.CId == examscore.CId);

        //     if (existingExamScore != null)
        //     {
        //         return Conflict("Duplicated ExamScore"); // Trả về lỗi Conflict nếu đã tồn tại bản ghi trùng
        //     }

        //     var newExamScore = new ExamScore
        //     {
        //         StId = examscore.StId,
        //         CId = examscore.CId,
        //         Score = examscore.Score
        //     };

        //     _dbContext.ExamScores.Add(newExamScore);

        //     try
        //     {
        //         await _dbContext.SaveChangesAsync();
        //         return Ok("Add successfully");
        //     }
        //     catch (DbUpdateException)
        //     {
        //         // Handle database-related exceptions
        //         return StatusCode(StatusCodes.Status500InternalServerError); // 500 Internal Server Error
        //     }
        // }
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ExamScore>> PostExamScore(ExamScore examscore)
{
    if (examscore == null)
    {
        return BadRequest("Invalid ExamScore data"); // 400 Bad Request
    }

    // Kiểm tra xem Student có tồn tại không
    var existingStudent = await _dbContext.Student.FindAsync(examscore.StId);
    if (existingStudent == null)
    {
        return BadRequest("Invalid Student ID"); // 400 Bad Request
    }

    // Kiểm tra xem Course có tồn tại không
    var existingCourse = await _dbContext.Course.FindAsync(examscore.CId);
    if (existingCourse == null)
    {
        return BadRequest("Invalid Course ID"); // 400 Bad Request
    }

    // Kiểm tra xem ExamScore có bị trùng lặp không
    var existingExamScore = await _dbContext.ExamScores
        .FirstOrDefaultAsync(e => e.StId == examscore.StId && e.CId == examscore.CId);

    if (existingExamScore != null)
    {
        return Conflict("Duplicated ExamScore"); // Trả về lỗi Conflict nếu đã tồn tại bản ghi trùng
    }

    var newExamScore = new ExamScore
    {
        StId = examscore.StId,
        CId = examscore.CId,
        Score = examscore.Score,
        Student = existingStudent,  // Gán Student đã tìm được
        Course = existingCourse     // Gán Course đã tìm được
    };

    _dbContext.ExamScores.Add(newExamScore);

    try
    {
        await _dbContext.SaveChangesAsync();
        return Ok("Add successfully");
    }
    catch (DbUpdateException)
    {
        // Handle database-related exceptions
        return StatusCode(StatusCodes.Status500InternalServerError); // 500 Internal Server Error
    }
}

        [HttpPut]
        [Authorize]
        public async Task<ActionResult> PutBrand(int id, ExamScore examscore)
        {
            if (id != examscore.Id)
            {
                return BadRequest("Không tìm thấy bản ghi");
            }

            // Kiểm tra xem bản ghi với id đã tồn tại trong cơ sở dữ liệu chưa
            var existingExamScore = await _dbContext.ExamScores.FindAsync(id);
            if (existingExamScore == null)
            {
                return NotFound("Không tìm thấy bản ghi để cập nhật");
            }

            // Cập nhật thông tin của bản ghi đã tồn tại
            existingExamScore.StId = examscore.StId;
            existingExamScore.CId = examscore.CId;
            existingExamScore.Score = examscore.Score;

            try
            {
                await _dbContext.SaveChangesAsync();
                return Ok("Cập nhật thành công");
            }
            catch (DbUpdateException)
            {
                // Xử lý các lỗi liên quan đến cơ sở dữ liệu
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteBand(int id)
        {
            if (_dbContext.ExamScores == null)
            {
                return NotFound();
            }
            var examscore = await _dbContext.ExamScores.FindAsync(id);
            if (examscore == null)
            {
                return NotFound();
            }
            _dbContext.ExamScores.Remove(examscore);
            await _dbContext.SaveChangesAsync();
            return Ok();

        }

        private bool BrandAvailable(int id)
        {
            return (_dbContext.ExamScores?.Any(x => x.Id == id)).GetValueOrDefault();
        }
    }
}
