using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLMS.Core.Models
{
    public class Module
    {
        public int ModuleId { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string ModuleName { get; set; }

        [DataType(DataType.Date)]
        [Remote(action: "CheckModuleStartDate", controller: "Modules", AdditionalFields = "CourseId")]
        [Display(Name = "Start")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [Remote(action: "CheckModuleEndDate", controller: "Modules", AdditionalFields = "StartDate, CourseId")]
        [Display(Name = "End")]
        public DateTime EndDate { get; set; }
        public string Description { get; set; }

        [Required]
        [Display(Name = "Course")]
        public int CourseId { get; set; }
        public Course Course { get; set; }

        [Display(Name = "Activities")]
        public ICollection<Activity> ModuleActivities { get; set; }
        public ICollection<Document> ModuleDocuments { get; set; }
    }
}
