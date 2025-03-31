using PastBeam.Core.Library.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PastBeam.Application.Library.Interfaces
{
    public interface ICourseService
    {
        Task<IEnumerable<Course>> GetAllCoursesAsync();

        Task<Course?> GetCourseByIdAsync(int id);

        Task<bool> EnrollUserInCourseAsync(int userId, int courseId);

        Task<Course?> UpdateCourseAsync(int id, string? title = null, string? description = null);
        Task<bool> CreateCourseAsync(Course course);

    }
}
