using Microsoft.EntityFrameworkCore;
using PastBeam.Core.Library.Entities;
using PastBeam.Core.Library.Interfaces;
using PastBeam.Infrastructure.DataBase;

namespace PastBeam.Infrastructure.Library.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly ApplicationDbContext _context;

        public CourseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Course>> GetAllAsync()
        {
            return await _context.Courses.ToListAsync();
        }

        public async Task<Course?> GetByIdAsync(int id)
        {
            return await _context.Courses.FindAsync(id);
        }

        public async Task AddUserToCourseAsync(int userId, int courseId)
        {
            var isEnrolled = await IsUserEnrolledAsync(userId, courseId);
            if (!isEnrolled)
            {
                var userCourse = new UserCourse { UserId = userId, CourseId = courseId };
                _context.UserCourses.Add(userCourse);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> IsUserEnrolledAsync(int userId, int courseId)
        {
            return await _context.UserCourses.AnyAsync(uc => uc.UserId == userId && uc.CourseId == courseId);
        }
    }
}
