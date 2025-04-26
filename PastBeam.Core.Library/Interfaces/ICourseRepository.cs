using PastBeam.Core.Library.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PastBeam.Core.Library.Interfaces
{
    public interface ICourseRepository
    {
        Task<IEnumerable<Course>> GetAllAsync();
        Task<Course?> GetByIdAsync(int id);
        Task AddUserToCourseAsync(string userId, int courseId);
        Task<bool> IsUserEnrolledAsync(string userId, int courseId);
        Task<UserCourse?> GetUserCourseDetailsAsync(string userId, int courseId);
        Task UpdateCourseAsync(Course updatedCourse);
        Task<bool> CreateCourseAsync(Course course);
    }
}