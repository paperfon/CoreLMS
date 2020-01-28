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
        public string DocumentName { get; set; }
        public DateTime UploadTime { get; set; }
        public string DocumentPath { get; set; }
        public TypeOfDoc TypeOfDocument { get; set; }

        [Required]
        public string LMSUserId { get; set; }
        public LMSUser LMSUser { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

        public int ModuleId { get; set; }
        public Module Module { get; set; }

        public int ActivityId { get; set; }
        public Activity Activity { get; set; }
    }

    public enum TypeOfDoc
    {
        Assignment,
        Lecture
    }
}
