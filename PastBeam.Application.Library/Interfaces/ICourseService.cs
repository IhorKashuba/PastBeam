using PastBeam.Core.Library.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PastBeam.Application.Library.Interfaces
{
    public interface ICourseService
    {
        Task<IEnumerable<Course>> GetAllCoursesAsync();
        Task<Course?> GetCourseByIdAsync(int id);
        Task<bool> EnrollUserInCourseAsync(string userId, int courseId);
        Task<float?> GetCourseProgressAsync(string userId, int courseId);
        Task<Course?> UpdateCourseAsync(int id, string? title = null, string? description = null);
        Task<bool> CreateCourseAsync(Course course);
    }
}