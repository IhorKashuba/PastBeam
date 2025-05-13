using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PastBeam.Core.Library.Entities
{
    public class CourseMaterials
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public Guid CourseId { get; set; }
        public Course? Course { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}
