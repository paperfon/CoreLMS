using CoreLMS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLMS.Core.ViewModels
{
    public class CourseForStudent
    {
        public int? activeModuleId { get; set; }
        public int? activeActivityId { get; set; }
        public Course course { get; set; }
    }
}
