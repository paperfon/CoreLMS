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
    public class ModulesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ModulesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult CheckModuleStartDate(DateTime startdate, int courseid)
        {
     
            var coursestartdate = _context.Course
                .Where(c => c.CourseId == courseid)
                .Select(s => s.StartDate)
                .FirstOrDefault();
            if (startdate >= coursestartdate)
            {
                return Json($"{startdate} is not valid");
            }

            return Json(true);
        }

        public IActionResult CheckModuleEndDate(DateTime enddate, DateTime startdate)
        {

            if (enddate < startdate )
            {
                return Json($"{enddate} is not valid");
            }

            return Json(true);
        }

        public async Task<IActionResult> Filter(string modulename)
        {


            var filtermodel = string.IsNullOrWhiteSpace(modulename) ?
                await _context.Module.ToListAsync() :
                await _context.Module.Where(m => m.ModuleName.Contains(modulename)).ToListAsync();
            return View(nameof(Index), filtermodel);
        }


        // GET: Modules
        public async Task<IActionResult> Index(string sortOrder)
        {

            ViewBag.ModuleNameSortParm = String.IsNullOrEmpty(sortOrder) ? "ModuleName_desc" : "";
            ViewBag.StartDateSortParm = sortOrder == "StartDate" ? "StartDate_desc" : "StartDate";
            ViewBag.EndDateSortParm = sortOrder == "EndDate" ? "EndDate_desc" : "EndDate";
            ViewBag.DescriptionSortParm = sortOrder == "Description" ? "Description_desc" : "Description";
            ViewBag.CourseSortParm = sortOrder == "Course" ? "Course_desc" : "Course";


            IQueryable<Module> module = _context.Module.Include(c => c.Course);

            switch (sortOrder)
            {
                case "ModuleName_desc":
                    module = module.OrderByDescending(s => s.ModuleName);
                    break;
                case "StartDate_desc":
                    module = module.OrderByDescending(s => s.StartDate);
                    break;
                case "StartDate":
                    module = module.OrderBy(s => s.StartDate);
                    break;
                case "EndDate_desc":
                    module = module.OrderByDescending(s => s.EndDate);
                    break;
                case "EndDate":
                    module = module.OrderBy(s => s.EndDate);
                    break;
                case "Description_desc":
                    module = module.OrderByDescending(s => s.Description);
                    break;
                case "Description":
                    module = module.OrderBy(s => s.Description);
                    break;
                case "Course_desc":
                    module = module.OrderByDescending(s => s.Course);
                    break;
                case "Course":
                    module = module.OrderBy(s => s.Course);
                    break;
                default:
                    module = module.OrderBy(s => s.ModuleName);
                    break;
            }

            return View(await module.ToListAsync());
        }

        // GET: Modules/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var module = await _context.Module
                .Include(c => c.Course)
                .Include(m => m.ModuleActivities)
                .FirstOrDefaultAsync(m => m.ModuleId == id);
            if (module == null)
            {
                return NotFound();
            }

            return View(module);
        }

        // GET: Modules/Create
        public IActionResult Create()
        {
            ViewData["CourseId"] = new SelectList(_context.Set<Course>(), "CourseId", "CourseName");
            return View();
        }

        // POST: Modules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ModuleId,ModuleName,StartDate,EndDate,Description,CourseId")] Module module)
        {
            if (ModelState.IsValid)
            {
                _context.Add(module);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Set<Course>(), "CourseId", "CourseName", @module.CourseId);
            return View(module);
        }

        // GET: Modules/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var module = await _context.Module.FindAsync(id);
            if (module == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(_context.Set<Course>(), "CourseId", "CourseName", module.CourseId);
            return View(module);
        }

        // POST: Modules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ModuleId,ModuleName,StartDate,EndDate,Description,CourseId")] Module module)
        {
            if (id != module.ModuleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(module);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModuleExists(@module.ModuleId))
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
            ViewData["CourseId"] = new SelectList(_context.Set<Course>(), "CourseId", "CourseName", module.CourseId);
            return View(module);
        }

        // GET: Modules/Delete/5
        private bool ModuleExists(int id)
        {
            return _context.Module.Any(e => e.ModuleId == id);
        }

        //public async Task<IActionResult> GetCourseName()
        //{
        //    return (await _context.Course.FirstOrDefaultAsync(c=>c.CourseName).ToString);
        //}
     
    }
}
