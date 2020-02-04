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
        public string ModuleName { get; set; }

        [Remote(action: "CheckModuleStartDate", controller: "Modules", AdditionalFields = "CourseId")]
        public DateTime StartDate { get; set; }

        [Remote(action: "CheckModuleEndDate", controller: "Modules")]
        public DateTime EndDate { get; set; }
        public string Description { get; set; }

        [Required]
        public int CourseId { get; set; }
        public Course Course { get; set; }

        public ICollection<Activity> ModuleActivities { get; set; }
        public ICollection<Document> ModuleDocuments { get; set; }
    }
}
