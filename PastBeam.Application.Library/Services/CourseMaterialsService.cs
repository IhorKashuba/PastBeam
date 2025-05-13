using PastBeam.Application.Library.Interfaces;
using PastBeam.Core.Library.Entities;
using PastBeam.Core.Library.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PastBeam.Application.Library.Services
{
    public class CourseMaterialsService : ICourseMaterialsService
    {
        private readonly ICourseMaterialsRepository _repo;

        public CourseMaterialsService(ICourseMaterialsRepository repo)
        {
            _repo = repo;
        }

        public Task<IEnumerable<CourseMaterials>> GetMaterialsByCourseAsync(Guid courseId)
            => _repo.GetByCourseIdAsync(courseId);

        public Task AddMaterialAsync(CourseMaterials material)
            => _repo.AddAsync(material);

        public Task DeleteMaterialAsync(Guid id)
            => _repo.DeleteAsync(id);

        public Task UpdateMaterialAsync(CourseMaterials material)
            => _repo.UpdateAsync(material);
    }
}
