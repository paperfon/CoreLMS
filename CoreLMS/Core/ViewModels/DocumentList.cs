using CoreLMS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLMS.Core.ViewModels
{
    public class DocumentList
    {

   
        public string DocumentName { get; set; }
        public DateTime UploadTime { get; set; }
        public string Document { get; set; }
        public TypeOfDoc TypeOfDocument { get; set; }

        public string LMSUserId { get; set; }
        public LMSUser LMSUser { get; set; }

        public int? CourseId { get; set; }
        public Course Course { get; set; }

        public int? ModuleId { get; set; }
        public Module Module { get; set; }

        public int? ActivityId { get; set; }
        public Activity Activity { get; set; }
    }
}
