using Microsoft.AspNetCore.Mvc;
using PastBeam.Application.Library.Interfaces;
using PastBeam.Core.Library.Entities;

namespace PastBeam.Presentation.Controllers
{
    public class CourseMaterialsController : Controller
    {
        private readonly ICourseMaterialsService _service;

        public CourseMaterialsController(ICourseMaterialsService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index(Guid courseId)
        {
            var materials = await _service.GetMaterialsByCourseAsync(courseId);
            ViewBag.CourseId = courseId;
            return View(materials);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(Guid courseId, IFormFile file, string title)
        {
            if (file != null && file.Length > 0)
            {
                var path = Path.Combine("wwwroot/materials", file.FileName);
                using var stream = new FileStream(path, FileMode.Create);
                await file.CopyToAsync(stream);

                var material = new CourseMaterials
                {
                    Title = title,
                    FilePath = "/materials/" + file.FileName,
                    CourseId = courseId
                };

                await _service.AddMaterialAsync(material);
            }

            return RedirectToAction("Index", new { courseId });
        }
    }
}
