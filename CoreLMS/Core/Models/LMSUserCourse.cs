using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLMS.Core.Models
{
    public class LMSUserCourse
    {
        public int CourseId { get; set; }
        public Course Course { get; set; }

        public string LMSUserId { get; set; }
        public LMSUser LMSUser { get; set; }
    }
}
