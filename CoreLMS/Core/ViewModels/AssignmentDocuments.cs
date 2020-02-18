using CoreLMS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;

namespace CoreLMS.Core.ViewModels
{
    public class AssignmentDocuments
    {
        public ICollection<Document> AssignmentDocs { get; set; }
    }
}
