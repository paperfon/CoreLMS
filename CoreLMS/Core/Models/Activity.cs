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
        public string ActivityName { get; set; }

        [Remote(action: "CheckActivitesStartDate", controller: "Activities", AdditionalFields ="ModuleId")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy/mm/dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy/mm/dd}", ApplyFormatInEditMode = true)]
        [Remote(action: "CheckActivitesEndDate", controller: "Activities", AdditionalFields = "StartDate")]
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public ActivityType ActivityType { get; set; }

        [Required]
        public int ModuleId { get; set; }
        public Module Module { get; set; }

        public ICollection<Document> ActivityDocuments { get; set; }
    }

    public enum ActivityType
    {
        ELearning,
        Session,
        Homework,
        Other
    }
}
