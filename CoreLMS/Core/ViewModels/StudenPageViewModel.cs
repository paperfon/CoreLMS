using CoreLMS.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoreLMS.Core.ViewModels
{
    public class StudenPageViewModel
    {
     
        public string CourseName { get; set; }

        public string Description { get; set; }

        public int CourseId { get; set; }

        [DataType(DataType.Date)]
        public DateTime Start{ get; set; }

        [DataType(DataType.Date)]
        public DateTime End { get; set; }

        public ICollection<ModulesActivitiesViewModel> ModulesforActivities { get; set; }

        //public ICollection<CourseStudents> Studentslist { get; set; }



    }
}
