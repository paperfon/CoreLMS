﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLMS.Core.Models
{
    public class Course
    {
        public int CourseId { get; set; }
        
        [Required]
        public string CourseName { get; set; }
        public DateTime StartDate { get; set; }
        public string Description { get; set; }

        public ICollection<Module> CourseModules { get; set; }
        public ICollection<Document> CourseDocuments { get; set; }
    }
}