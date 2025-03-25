using Microsoft.AspNetCore.Mvc;
using PastBeam.Application.Library.Services;
using PastBeam.Core.Library.Entities;
using PastBeam.Core.Library.Interfaces;

namespace PastBeam.Presentation.Controllers
{
    [Route("courses")]
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet]
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
        public async Task<IActionResult> Enroll(int userId, int courseId)
        {
            bool enrolled = await _courseService.EnrollUserInCourseAsync(userId, courseId);
            if (!enrolled) return BadRequest("User is already enrolled.");
            return Ok("User enrolled successfully.");
        }
    }
}
