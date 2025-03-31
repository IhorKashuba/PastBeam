using PastBeam.Core.Library.Entities;

namespace PastBeam.Core.Library.Interfaces
{
    public interface ICourseRepository
    {
        Task<IEnumerable<Course>> GetAllAsync();
        Task<Course?> GetByIdAsync(int id);
        Task AddUserToCourseAsync(int userId, int courseId);
        Task<bool> IsUserEnrolledAsync(int userId, int courseId);
        Task UpdateCourseAsync(Course updatedCourse);
        Task<Course?> CreateCourseAsync(Course course);

    }
}