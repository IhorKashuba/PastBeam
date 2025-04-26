using Microsoft.AspNetCore.Mvc;
using PastBeam.Application.Library.Interfaces;
using System.Security.Claims;

namespace PastBeam.Presentation.Controllers
{
    [Route("courses")]
    public class CourseController : Controller
    {
        private ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> Index()
        {
            var courses = await _courseService.GetAllCoursesAsync();
            return View(courses);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var course = await _courseService.GetCourseByIdAsync(id);
            if (course == null) return NotFound();
            return View(course);
        }

        [HttpPost("enroll")]
        public async Task<IActionResult> Enroll(string userId, int courseId)
        {
            bool enrolled = await _courseService.EnrollUserInCourseAsync(userId, courseId);
            if (!enrolled) return BadRequest("User is already enrolled.");
            return Ok("User enrolled successfully.");
        }

        [HttpGet("{courseId:int}/progress")]
        public async Task<IActionResult> GetProgress(int courseId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //if (string.IsNullOrEmpty(userIdString) || !string.TryParse(userIdString, out var userId))
            //{
            //    return Unauthorized("User ID claim not found or invalid.");
            //}

            try
            {
                var progress = await _courseService.GetCourseProgressAsync(userId, courseId);

                if (progress == null)
                {
                    return NotFound(new { message = "Progress not found. User might not be enrolled in this course." });
                }

                return Ok(new { courseId = courseId, userId = userId, progress = progress });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An internal server error occurred while retrieving course progress.");
            }
        }
    }
}
