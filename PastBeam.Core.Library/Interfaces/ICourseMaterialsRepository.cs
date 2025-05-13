using PastBeam.Core.Library.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PastBeam.Core.Library.Interfaces
{
    public interface ICourseMaterialsRepository
    {
        Task<IEnumerable<CourseMaterials>> GetByCourseIdAsync(Guid courseId);
        Task<CourseMaterials?> GetByIdAsync(Guid id);
        Task AddAsync(CourseMaterials material);
        Task DeleteAsync(Guid id);
        Task UpdateAsync(CourseMaterials material);
    }
}
