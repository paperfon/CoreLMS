using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLMS.Core.Models
{
    public class LMSUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public ICollection<Document> LMSUserDocuments { get; set; }
        public ICollection<LMSUserCourse> RegisteredCourses { get; set; }
    }
}
