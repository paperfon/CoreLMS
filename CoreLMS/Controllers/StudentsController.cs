﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreLMS.Core.Models;
using CoreLMS.Core.ViewModels;
using CoreLMS.Data;
using Microsoft.AspNetCore.Authorization;
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
            // Get a list of users with a particular role
            var roleId = _context.Roles.FirstOrDefault(r => r.Name == "Student").Id;
            var userRoleList = _context.UserRoles.Where(x => x.RoleId == roleId).Select(c=>c.UserId).ToList();

            var model = await userManager.Users
                .Include(r => r.RegisteredCourses)
                .ThenInclude(c => c.Course)
                .Where(u => userRoleList.Any(c=> c == u.Id))
                .Select(s => new StudentListViewModel
                {
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Email = s.Email,
                    Courses = s.RegisteredCourses.Select(r => r.Course).ToList()
                })
                .ToListAsync();

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


        /******************************************************************************************************/
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> DetailsForStudent()
        {
            var stu_id = userManager.GetUserId(User);
            var course_id = await _context.LMSUserCourses
                .Where(s => s.LMSUserId == stu_id)
                .Select(c => c.CourseId)
                .FirstOrDefaultAsync();

            //if (id == null)
            //{
            //    return NotFound();
            //}

            var course = await _context.Courses
                .Include(m => m.CourseModules)
                .ThenInclude(a => a.ModuleActivities)
                .FirstOrDefaultAsync(c => c.CourseId == course_id);
            if (course == null)
            {
                return NotFound();
            }

            course.CourseModules = course.CourseModules
                .OrderBy(m => m.StartDate.Date)
                .ThenBy(m => m.EndDate)
                .ToList();

            foreach (Module m in course.CourseModules)
            {
                m.ModuleActivities = m.ModuleActivities
                    .OrderBy(a => a.StartDate.Date)
                    .ThenBy(a => a.EndDate).ToList();
            }

            var user = await userManager.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            int? currentModuleId = null;
            foreach (var mod in course.CourseModules)
            {
                foreach (Activity act in mod.ModuleActivities)
                {

                    currentModuleId = act.ModuleId;
                    break;

                }
                if (currentModuleId != null) break;
            }

            var currentActivityId = _context.Activities.Where(a => a.ModuleId == currentModuleId);


            var cfs = new CourseForStudent() { activeModuleId = currentModuleId, course = course };

            return View(cfs);
        }
    }
}