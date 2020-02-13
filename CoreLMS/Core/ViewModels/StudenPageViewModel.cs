using CoreLMS.Core.Models;
using System;
using System.Collections.Generic;

namespace CoreLMS.Core.ViewModels
{
    public class StudenPageViewModel
    {
     
        public string CourseName { get; set; }

        public string CourseDescription { get; set; }

        public int CourseId { get; set; }
 
        public ICollection<ModulesActivitiesViewModel> ModulesforActivities { get; set; }

        //public ICollection<CourseStudents> Studentslist { get; set; }



    }
}
