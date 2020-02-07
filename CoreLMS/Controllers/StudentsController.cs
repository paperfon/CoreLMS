using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreLMS.Core.Models;
using CoreLMS.Core.ViewModels;
using CoreLMS.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoreLMS.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<LMSUser> userManager;

        public StudentsController(ApplicationDbContext context, UserManager<LMSUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }

        // GET: Students
        public async Task<IActionResult> Index()
        {

            var userId = userManager.GetUserId(User);
            var user = await userManager.FindByIdAsync(userId);
            var userRole = await userManager.GetRolesAsync(user);

            var model = await userManager.Users
                .Select(s => new StudentListViewModel
                {
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Email = s.Email
                }).ToListAsync();

            return View(model);
        }

        public async Task<IActionResult> StudentPage()
        {
            var stu_id = userManager.GetUserId(User);
            var course_id = await _context.LMSUserCourses
                .Where(s => s.LMSUserId == stu_id)
                .Select(c => c.CourseId)
                .FirstOrDefaultAsync();

            var moduleid = _context.Modules
                .Where(m => m.CourseId == course_id)
                .Select(m => m.ModuleId)
                .FirstOrDefault();

            var stu_activities = _context.Activities.Where(a => a.ModuleId == moduleid).ToList();

            var model = from c in _context.Courses
                        .Where(c => c.CourseId == course_id)
                        join m in _context.Modules
                        on c.CourseId equals m.CourseId
                        join a in _context.Activities
                        on m.ModuleId equals a.ModuleId into sp
                        from s in sp.DefaultIfEmpty()
                        select new StudenPageViewModel
                        {
                            CourseName = c.CourseName,
                            CourseStartDate = c.StartDate,
                            CourseEndDate = c.EndDate,
                            ModuleName = m.ModuleName,
                            ModuleStartDate = m.StartDate,
                            ModuleEndDate = m.EndDate,
                            ActivityName = s.ActivityName,
                            ActivityStartDate = s.StartDate,
                            ActivityEndDate = s.EndDate,
                            ActivityType = s.ActivityType.ToString()
                        };

            return View(model);
        }
    }
}