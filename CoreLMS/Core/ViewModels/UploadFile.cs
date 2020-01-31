using CoreLMS.Core.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLMS.Core.ViewModels
{
    public class UploadFile
    {
        [Required]
        public string DocumentName { get; set; }
        public DateTime UploadTime { get; set; }
        public string DocumentPath { get; set; }
        public TypeOfDoc TypeOfDocument { get; set; }

        public string LMSUserId { get; set; }

        public IFormFile File { get; set; }

        public Entity LMSEntity { get; set; }
        
    }

    public enum Entity
    {
        Course,
        Module,
        Activity
    }
}
