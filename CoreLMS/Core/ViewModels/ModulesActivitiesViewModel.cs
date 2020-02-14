using CoreLMS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLMS.Core.ViewModels
{
    public class ModulesActivitiesViewModel
    {
        public string  ModuleNameforCourse { get; set; }

        public int ModuleID { get; set; }

        public ICollection<Activity> Activitiesformodule { get; set; }
    }
}
