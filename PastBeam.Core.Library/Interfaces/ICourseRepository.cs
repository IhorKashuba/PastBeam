using PastBeam.Core.Library.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PastBeam.Core.Library.Interfaces
{
    public interface ICourseRepository
    {
        Task<IEnumerable<Course>> GetAllAsync();
        Task<Course?> GetByIdAsync(int id);
        Task AddUserToCourseAsync(int userId, int courseId);
        Task<bool> IsUserEnrolledAsync(int userId, int courseId);
        Task<UserCourse?> GetUserCourseDetailsAsync(int userId, int courseId);
        Task UpdateCourseAsync(Course updatedCourse);
        Task<bool> CreateCourseAsync(Course course);
    }
}