using Microsoft.EntityFrameworkCore;
using PastBeam.Core.Library.Entities;
using PastBeam.Core.Library.Interfaces;
using PastBeam.Infrastructure.DataBase;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        public async Task AddUserToCourseAsync(string userId, int courseId)
        {
            var isEnrolled = await IsUserEnrolledAsync(userId, courseId);
            if (!isEnrolled)
            {
                var userCourse = new UserCourse { UserId = userId, CourseId = courseId };
                _context.UserCourses.Add(userCourse);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> IsUserEnrolledAsync(string userId, int courseId)
        {
            return await _context.UserCourses.AnyAsync(uc => uc.UserId == userId && uc.CourseId == courseId);
        }

        public async Task<UserCourse?> GetUserCourseDetailsAsync(string userId, int courseId)
        {
            return await _context.UserCourses
                                 .FirstOrDefaultAsync(uc => uc.UserId == userId && uc.CourseId == courseId);
        }

        public async Task UpdateCourseAsync(Course updatedCourse)
        {
            _context.Courses.Update(updatedCourse);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> CreateCourseAsync(Course course)
        {
            var existingCourse = await _context.Courses.FirstOrDefaultAsync(c => c.Title == course.Title);
            if (existingCourse == null)
            {
                _context.Courses.Add(course);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}