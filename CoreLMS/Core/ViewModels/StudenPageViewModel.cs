using CoreLMS.Core.Models;
using System;
using System.Collections.Generic;

namespace CoreLMS.Core.ViewModels
{
    public class StudenPageViewModel
    {
     
        public string CourseName { get; set; }

        public string CourseDescription { get; set; }
        //public DateTime CourseStartDate { get; set; }
        //public DateTime CourseEndDate { get; set; }

        public ICollection<ModulesActivitiesViewModel> ModulesforActivities { get; set; }

        //public string ModuleName { get; set; }
        //public DateTime ModuleStartDate { get; set; }
        //public DateTime ModuleEndDate { get; set; }

        //public string ActivityName { get; set; }
        //public DateTime ActivityStartDate { get; set; }
        //public DateTime ActivityEndDate { get; set; }

        //public string ActivityType { get; set; }

        

    }
}
