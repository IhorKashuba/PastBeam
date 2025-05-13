using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PastBeam.Application.Library.Dtos.CourseMaterials
{
    public class CreateCourseMaterialsDto
    {
        public string Title { get; set; }
        public string FilePath { get; set; }
        public int CourseId { get; set; }
    }

}
