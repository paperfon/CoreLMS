using CoreLMS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLMS.Core.ViewModels
{
    public class CourseDocuments
    {
        public ICollection<Document> CourseDocs { get; set; }

        public ICollection<Document> moduleDocs { get; set; }
        public ICollection<Document> ActivityDocs { get; set; }
    }
}
