﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PastBeam.Application.Library.Dtos.CourseMaterials
{
    public class UpdateCourseMaterialsDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string FilePath { get; set; }
    }
}
