using PastBeam.Application.Library.Interfaces;
using PastBeam.Core.Library.Entities;
using PastBeam.Core.Library.Interfaces;
using PastBeam.Infrastructure.Library.Logger;

namespace PastBeam.Application.Library.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ILogger _logger;

        public CourseService(ICourseRepository courseRepository, ILogger logger)
        {
            _courseRepository = courseRepository;
            _logger = logger;
        }

        public async Task<float?> GetCourseProgressAsync(int userId, int courseId)
        {
            _logger.LogToFile($"Attempting to get progress for user {userId} on course {courseId}.");
            try
            {
                var userCourseDetails = await _courseRepository.GetUserCourseDetailsAsync(userId, courseId);

                if (userCourseDetails == null)
                {
                    _logger.LogToFile($"User {userId} is not enrolled in course {courseId}, cannot get progress.");
                    return null; // Return null if user is not enrolled or details not found
                }

                _logger.LogToFile($"Progress for user {userId} on course {courseId} is {userCourseDetails.Progress}.");
                return userCourseDetails.Progress;
            }
            catch (Exception ex)
            {
                _logger.LogToFile($"Error getting progress for user {userId} on course {courseId}: {ex.GetType().Name} - {ex.Message}");
                throw; // Re-throwing is often preferred for controller handling
            }
        }

        public async Task<IEnumerable<Course>> GetAllCoursesAsync()
        {
            return await _courseRepository.GetAllAsync();
        }

        public async Task<Course?> GetCourseByIdAsync(int id)
        {
            return await _courseRepository.GetByIdAsync(id);
        }

        public async Task<bool> EnrollUserInCourseAsync(int userId, int courseId)
        {
            try
            {
                if (await _courseRepository.IsUserEnrolledAsync(userId, courseId))
                {
                    return false; // Користувач уже записаний
                }

                await _courseRepository.AddUserToCourseAsync(userId, courseId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogToFile($"Error enrolling user {userId} to course {courseId}: {ex.Message}");
                throw;
            }
        }
    }
}
