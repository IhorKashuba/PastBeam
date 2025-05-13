using Microsoft.EntityFrameworkCore;
using PastBeam.Core.Library.Entities;
using PastBeam.Core.Library.Interfaces;
using PastBeam.Infrastructure.DataBase;

namespace PastBeam.Infrastructure.Library.Repositories
{
    public class CourseMaterialsRepository : ICourseMaterialsRepository
    {
        private readonly ApplicationDbContext _context;

        public CourseMaterialsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CourseMaterials>> GetByCourseIdAsync(Guid courseId)
        {
            return await _context.CourseMaterials
                                 .Where(cm => cm.CourseId == courseId)
                                 .ToListAsync();
        }

        public async Task<CourseMaterials?> GetByIdAsync(Guid id)
        {
            return await _context.CourseMaterials.FindAsync(id);
        }

        public async Task AddAsync(CourseMaterials material)
        {
            await _context.CourseMaterials.AddAsync(material);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var material = await _context.CourseMaterials.FindAsync(id);
            if (material != null)
            {
                _context.CourseMaterials.Remove(material);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(CourseMaterials material)
        {
            _context.CourseMaterials.Update(material);
            await _context.SaveChangesAsync();
        }
    }
}