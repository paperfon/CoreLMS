using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CoreLMS.Core.Models;
using CoreLMS.Core.ViewModels;
using CoreLMS.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        [Authorize(Roles = "Student, Teacher, Admin")]
        public async Task<IActionResult> Participant()
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
                    Id = s.Id,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Email = s.Email,
                    Courses = s.RegisteredCourses.Select(r => r.Course).ToList()
                })
                .ToListAsync();

            return View(model);
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details (string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await userManager.Users
                .Include(r=>r.RegisteredCourses)
                .ThenInclude(c=>c.Course)
                .Where(s=>s.Id == id)
                .Select(s => new StudentListViewModel
                {
                    Id = s.Id,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Email = s.Email,
                    Courses = s.RegisteredCourses.Select(r => r.Course).ToList()
                }).FirstOrDefaultAsync();

            return View(model);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coursesList = _context.Courses.Select(i => new SelectListItem()
            {
                Text = i.CourseName,
                Value = i.CourseId.ToString()
            });

            var student = await userManager.Users
                .Include(r => r.RegisteredCourses)
                .ThenInclude(c => c.Course)
                .Where(s => s.Id == id)
                .Select(s => new StudentListViewModel
                {
                    Id = s.Id,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Email = s.Email,
                    CoursesList = coursesList,
                    CourseId = s.RegisteredCourses
                        .Where(u => u.LMSUserId == s.Id)
                        .Select(c=>c.CourseId)
                        .FirstOrDefault()
                    //Courses = s.RegisteredCourses
                    //    .Where(u=>u.LMSUserId == s.Id)
                    //    .Select(c=>c.Course)
                    //    .ToList()
                    //Select(r => r.Course).Where(c=>c. == s.Id).ToList()
                }).FirstOrDefaultAsync();

            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,FirstName,LastName,Email,CourseId")] StudentListViewModel student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            var courseId = student.CourseId;

            if (ModelState.IsValid)
            {
                var user = await userManager.FindByIdAsync(id);
                try
                {
                    //_context.Update(student);
                    //var setEmailResult = await userManager.SetEmailAsync(user, student.Email);
                    user.FirstName = student.FirstName;
                    user.LastName = student.LastName;
                    user.Email = student.Email;

                    // Replace composite key
                    // TODO: Should remove all courses not only the first
                    List<LMSUserCourse> previousCourses = _context.LMSUserCourses
                        .Where(u => u.LMSUserId == user.Id).ToList();
                    _context.RemoveRange(previousCourses);

                    var newCourse = new LMSUserCourse
                    {
                        CourseId = student.CourseId,
                        LMSUserId = student.Id
                    };
                    _context.Add(newCourse);
                    //user.RegisteredCourses = student.RegisteredCourses;
                    //user.RegisteredCourses = new LMSUserCourse
                    //{

                    //}
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Participant));
            }
            return View(student);
        }

        private bool StudentExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        public async Task<IActionResult> StudentPage()
        {
            var stu_id = userManager.GetUserId(User);
            var course_id = _context.LMSUserCourses.Where(s => s.LMSUserId == stu_id).Select(c => c.CourseId).FirstOrDefault();


            ViewBag.StudentName= userManager.GetUserName(User);

            ViewBag.Coursestudents = await _context.LMSUserCourses
                .Where(lc => lc.CourseId == course_id)
                .Include(ls => ls.LMSUser)
                .Select(s => new CourseStudents
                {
                    StudentId = s.LMSUser.Id,
                    FullName = s.LMSUser.FullName,
                    Email = s.LMSUser.Email
                }).ToListAsync();

            IEnumerable<StudenPageViewModel> model = await _context.LMSUserCourses
                .Include(c => c.Course)
                .ThenInclude(m => m.CourseModules)
                .Where(s => s.LMSUserId == stu_id)
                .Select(c => new StudenPageViewModel
                {
                    CourseName = c.Course.CourseName,
                    CourseDescription = c.Course.Description,
                    CourseId = c.Course.CourseId,
                    ModulesforActivities = c.Course.CourseModules.Select(ma => new ModulesActivitiesViewModel
                    {
                        ModuleNameforCourse = ma.ModuleName,
                        ModuleID = ma.ModuleId,
                        Activitiesformodule = ma.ModuleActivities
                    }).ToList()

                }).ToListAsync();

            return View(model); 
        }


        /******************************************************************************************************/
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Index()
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

            var course = await _context.Courses.Include(d=>d.CourseDocuments)
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

            foreach (var coursedocuments in cfs.course.CourseDocuments)
            {
                coursedocuments.DocumentPath = Path.GetFileName(coursedocuments.DocumentPath);
                var documentid = coursedocuments.DocumentId;
                var documents = _context.Documents.Include(c => c.Course).Include(m => m.Module).Include(a => a.Activity)
                               .Where(d => d.DocumentId == documentid)
                               .Select(d => new
                               {
                                   id = d.DocumentId,
                                   courseid = d.CourseId,
                                   coursename = d.Course.CourseName,
                                   moduleid = d.ModuleId,
                                   modulename = d.Module.ModuleName,
                                   activityid = d.ActivityId,
                                   activityname = d.Activity.ActivityName

                               }).FirstOrDefault();
                var entity = "";
                var entityname = "";
                if ((documents.activityid != null) && (documents.moduleid != null) && (documents.courseid != null))
                {
                    entity = "Activity";
                    entityname = documents.activityname;

                }
                else if((documents.activityid == null) && (documents.moduleid != null) && (documents.courseid != null))
                {
                    entity = "Module";
                    entityname = documents.modulename;
                }
                else if((documents.activityid == null) && (documents.moduleid == null) && (documents.courseid != null))
                {
                    entity = "Course";
                    entityname = documents.coursename;
                }
                coursedocuments.DocumentName = entity + ": " + entityname;
            }

            

            return View(cfs);
        }


        [Authorize(Roles = "Student")]
        public async Task<IActionResult> CourseParticipent()
        {
            var stu_id = userManager.GetUserId(User);
            var course_id = await _context.LMSUserCourses
                .Where(s => s.LMSUserId == stu_id)
                .Select(c => c.CourseId)
                .FirstOrDefaultAsync();

 
            var model = await _context.LMSUserCourses
                .Where(c => c.CourseId == course_id)
                .Select(s => new ParticipantsListViewModel
                {
                    FirstName = s.LMSUser.FirstName,
                    LastName = s.LMSUser.LastName,
                    Email = s.LMSUser.Email,
                    CourseName = s.Course.CourseName

                }).ToListAsync();


            return View(model);
        }


        }
    }