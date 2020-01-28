﻿using System;
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
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public string ActivityType { get; set; }

        [Required]
        public int ModuleId { get; set; }
        public Module Module { get; set; }

        public ICollection<Document> ActivityDocuments { get; set; }
    }
}