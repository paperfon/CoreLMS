using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoreLMS.Core.Models;
using CoreLMS.Data;
using Microsoft.AspNetCore.Authorization;

namespace CoreLMS.Controllers
{
    [Authorize]
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CoursesController(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> PreviousCourses(string sortOrder)
        {
            IQueryable<Course> course = _context.Courses.Where(c => c.EndDate < DateTime.Now);

            course = SortCourses(sortOrder, course);

            return View(await course.ToListAsync());
        }

        // GET: Courses
        public async Task<IActionResult> Index(string sortOrder)
        {
            IQueryable<Course> course = _context.Courses.Where(c => c.EndDate > DateTime.Now); ;

            course = SortCourses(sortOrder, course);

            if (this.User.IsInRole("Student"))
            {
                return RedirectToAction("Index", "Students");
            }

            return View(await course.ToListAsync());
        }

        private IQueryable<Course> SortCourses(string sortOrder, IQueryable<Course> course)
        {
            ViewBag.CourseNameSortParm = String.IsNullOrEmpty(sortOrder) ? "CourseName_desc" : "";
            ViewBag.StartDateSortParm = sortOrder == "StartDate" ? "StartDate_desc" : "StartDate";
            ViewBag.EndDateSortParm = sortOrder == "EndDate" ? "EndDate_desc" : "EndDate";
            ViewBag.DescriptionSortParm = sortOrder == "Description" ? "Description_desc" : "Description";

            switch (sortOrder)
            {
                case "CourseName_desc":
                    course = course.OrderByDescending(s => s.CourseName);
                    break;
                case "StartDate_desc":
                    course = course.OrderByDescending(s => s.StartDate);
                    break;
                case "StartDate":
                    course = course.OrderBy(s => s.StartDate);
                    break;
                case "EndDate_desc":
                    course = course.OrderByDescending(s => s.EndDate);
                    break;
                case "EndDate":
                    course = course.OrderBy(s => s.EndDate);
                    break;
                case "Description_desc":
                    course = course.OrderByDescending(s => s.Description);
                    break;
                case "Description":
                    course = course.OrderBy(s => s.Description);
                    break;
                default:
                    course = course.OrderBy(s => s.CourseName);
                    break;
            }

            return course;
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(c=>c.CourseModules)                
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseId,CourseName,StartDate,EndDate,Description")] Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CourseId,CourseName,StartDate,Description")] Course course)
        {
            if (id != course.CourseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.CourseId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.CourseId == id);
        }

        public async Task<IActionResult> CoursesModules(int id)
        {
            var model = await _context.Modules
                 .Include(c => c.Course)
                .Where(m => m.CourseId == id)
                .ToListAsync();

            return View(model);
        }

        public async Task<IActionResult> ModulesActivities(int id)
        {
            var model = await _context.Activities
                .Include(m=> m.Module)
                .Where(a => a.ModuleId == id)
                .ToListAsync();

            return View(model);
        }

        // Filter
        public async Task<IActionResult> Filter(string coursename)
        {


            var filtermodel = string.IsNullOrWhiteSpace(coursename) ?
                await _context.Courses.ToListAsync() :
                await _context.Courses.Where(m => m.CourseName.Contains(coursename)).ToListAsync();
            return View(nameof(Index), filtermodel);
        }

        // Date validations
        public IActionResult CheckCourseStartDate(DateTime startDate)
        {
            if (startDate.AddDays(1) < DateTime.UtcNow)
            {
                return Json($"{startDate} is not valid");
            }

            return Json(true);
        }

        public IActionResult CheckCourseEndDate(DateTime startDate, DateTime endDate)
        {
            if (endDate <= startDate)
            {
                return Json($"{endDate} is not valid");
            }

            return Json(true);
        }

    }
}
