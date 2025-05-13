using PastBeam.Core.Library.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PastBeam.Application.Library.Interfaces
{
    public interface ICourseMaterialsService
    {
        Task<IEnumerable<CourseMaterials>> GetMaterialsByCourseAsync(Guid courseId);
        Task AddMaterialAsync(CourseMaterials material);
        Task DeleteMaterialAsync(Guid id);
        Task UpdateMaterialAsync(CourseMaterials material);
    }
}
