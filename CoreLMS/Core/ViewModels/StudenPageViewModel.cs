using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLMS.Core.ViewModels
{
    public class StudenPageViewModel
    {
     
        public string CourseName { get; set; }
        public DateTime CourseStartDate { get; set; }
        public DateTime CourseEndDate { get; set; }

        public string ModuleName { get; set; }
        public DateTime ModuleStartDate { get; set; }
        public DateTime ModuleEndDate { get; set; }

        public string ActivityName { get; set; }
        public DateTime ActivityStartDate { get; set; }
        public DateTime ActivityEndDate { get; set; }

        public string ActivityType { get; set; }

       
    }
}
