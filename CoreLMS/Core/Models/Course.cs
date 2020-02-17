using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLMS.Core.Models
{
    public class Course
    {
        public int CourseId { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string CourseName { get; set; }
        public string Description { get; set; }

        [DataType(DataType.Date)]
        [Remote(action: "CheckCourseStartDate", controller: "Courses")]
        [Display(Name = "Start")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [Remote(action: "CheckCourseEndDate", controller: "Courses", AdditionalFields = "StartDate")]
        [Display(Name = "End")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Modules")]
        public ICollection<Module> CourseModules { get; set; }
        public ICollection<Document> CourseDocuments { get; set; }
    }
}
