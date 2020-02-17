using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLMS.Core.Models
{
    public class Document
    {
        public int DocumentId { get; set; }
        
        [Required]
        [Display(Name = "Name")]
        public string DocumentName { get; set; }

        [Display(Name = "Uploaded")]
        public DateTime UploadTime { get; set; }

        [Display(Name = "Path")]
        public string DocumentPath { get; set; }

        [Display(Name = "Type")]
        public TypeOfDoc TypeOfDocument { get; set; }

        [Required]
        public string LMSUserId { get; set; }
        
        [Display(Name = "Uploaded By")]
        public LMSUser LMSUser { get; set; }

        [Display(Name = "Course")]
        public int? CourseId { get; set; }
        public Course Course { get; set; }

        [Display(Name = "Module")]
        public int? ModuleId { get; set; }
        public Module Module { get; set; }

        [Display(Name = "Activity")]
        public int? ActivityId { get; set; }
        public Activity Activity { get; set; }
    }

    public enum TypeOfDoc
    {
        Assignment,
        Lecture
    }
}
