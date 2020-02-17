using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLMS.Core.Models
{
    public class Activity
    {
        public int ActivityId { get; set; }
        
        [Required]
        [Display(Name = "Name")]
        public string ActivityName { get; set; }

        [DataType(DataType.Date)]
        [Remote(action: "CheckActivitiesStartDate", controller: "Activities", AdditionalFields ="ModuleId")]
        [Display(Name = "Start")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [Remote(action: "CheckActivitiesEndDate", controller: "Activities", AdditionalFields = "StartDate, ModuleId")]
        [Display(Name = "End")]
        public DateTime EndDate { get; set; }
        public string Description { get; set; }

        [Display(Name = "Type")]
        public ActivityType ActivityType { get; set; }

        [Required]
        [Display(Name = "Module")]
        public int ModuleId { get; set; }
        public Module Module { get; set; }

        public ICollection<Document> ActivityDocuments { get; set; }
    }

    public enum ActivityType
    {
        ELearning,
        Session,
        Homework,
        Assignment,
        Other
    }
}
