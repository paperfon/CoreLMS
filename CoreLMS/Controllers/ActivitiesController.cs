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

    public class ActivitiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ActivitiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Activities
        public async Task<IActionResult> Index(string sortOrder)
        {
            IQueryable<Activity> activity = _context.Activities.Include(a => a.Module).Where(c => c.EndDate > DateTime.Now); ;
            activity = SortActivities(sortOrder, activity);

            return View(await activity.ToListAsync());
        }


        public async Task<IActionResult> PreviousActivities(int id, string sortOrder)
        {
            IQueryable<Activity> model = _context.Activities.Include(m => m.Module).Where(a => a.ModuleId == id && a.EndDate < DateTime.Now);
            @ViewBag.ModuleName = _context.Modules.Where(m => m.ModuleId == id).Select(m => m.ModuleName).FirstOrDefault();

            model = SortActivities(sortOrder, model);

            return View(await model.ToListAsync());
        }

        private IQueryable<Activity> SortActivities(string sortOrder, IQueryable<Activity> activity)
        {
            ViewBag.ActivityNameSortParm = String.IsNullOrEmpty(sortOrder) ? "ActivityName_desc" : "";
            ViewBag.StartDateSortParm = sortOrder == "StartDate" ? "StartDate_desc" : "StartDate";
            ViewBag.EndDateSortParm = sortOrder == "EndDate" ? "EndDate_desc" : "EndDate";
            ViewBag.DescriptionSortParm = sortOrder == "Description" ? "Description_desc" : "Description";
            ViewBag.ActivityTypeSortParm = sortOrder == "ActivityType" ? "ActivityType_desc" : "ActivityType";
            ViewBag.ModuleSortParm = sortOrder == "Module" ? "Module_desc" : "Module";



            switch (sortOrder)
            {
                case "ActivityName_desc":
                    activity = activity.OrderByDescending(s => s.ActivityName);
                    break;
                case "StartDate_desc":
                    activity = activity.OrderByDescending(s => s.StartDate);
                    break;
                case "StartDate":
                    activity = activity.OrderBy(s => s.StartDate);
                    break;
                case "EndDate_desc":
                    activity = activity.OrderByDescending(s => s.EndDate);
                    break;
                case "EndDate":
                    activity = activity.OrderBy(s => s.EndDate);
                    break;
                case "Description_desc":
                    activity = activity.OrderByDescending(s => s.Description);
                    break;
                case "Description":
                    activity = activity.OrderBy(s => s.Description);
                    break;
                case "ActivityType_desc":
                    activity = activity.OrderByDescending(s => s.ActivityType);
                    break;
                case "ActivityType":
                    activity = activity.OrderBy(s => s.ActivityType);
                    break;
                case "Module_desc":
                    activity = activity.OrderByDescending(s => s.Module);
                    break;
                case "Module":
                    activity = activity.OrderBy(s => s.Module);
                    break;
                default:
                    activity = activity.OrderBy(s => s.ActivityName);
                    break;
            }

            return activity;
        }

        // GET: Activities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _context.Activities
                .Include(a => a.Module)
                .FirstOrDefaultAsync(m => m.ActivityId == id);
            if (activity == null)
            {
                return NotFound();
            }

            return View(activity);
        }

        // GET: Activities/Create
        public IActionResult Create()
        {
            ViewData["ModuleId"] = new SelectList(_context.Modules, "ModuleId", "ModuleName");
            return View();
        }

        // POST: Activities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ActivityId,ActivityName,StartDate,EndDate,Description,ActivityType,ModuleId")] Activity activity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(activity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ModuleId"] = new SelectList(_context.Modules, "ModuleId", "ModuleName", activity.ModuleId);
            return View(activity);
        }

        // GET: Activities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _context.Activities.FindAsync(id);
            if (activity == null)
            {
                return NotFound();
            }
            ViewData["ModuleId"] = new SelectList(_context.Modules, "ModuleId", "ModuleName", activity.ModuleId);
            return View(activity);
        }

        // POST: Activities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ActivityId,ActivityName,StartDate,EndDate,Description,ActivityType,ModuleId")] Activity activity)
        {
            if (id != activity.ActivityId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(activity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActivityExists(activity.ActivityId))
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
            ViewData["ModuleId"] = new SelectList(_context.Modules, "ModuleId", "ModuleName", activity.ModuleId);
            return View(activity);
        }

        // GET: Activities/Delete/5
        private bool ActivityExists(int id)
        {
            return _context.Activities.Any(e => e.ActivityId == id);
        }

        // Filter
        public async Task<IActionResult> Filter(string activityname)
        {
            var filtermodel = string.IsNullOrWhiteSpace(activityname) ?
                await _context.Activities.ToListAsync() :
                await _context.Activities.Where(m => m.ActivityName.Contains(activityname)).ToListAsync();
            return View(nameof(Index), filtermodel);
        }

        // Date validations
        public IActionResult CheckActivitiesStartDate(DateTime startDate, int moduleId)
        {
            DateTime moduleStartDate = GetModuleStartDate(moduleId);
            DateTime moduleEndDate = GetModuleEndDate(moduleId);

            if (startDate < moduleStartDate | startDate > moduleEndDate)
            {
                return Json($"The activity has to start within the date of the module ({moduleStartDate.ToShortDateString()} to {moduleEndDate.ToShortDateString()})");
            }

            return Json(true);
        }

        public IActionResult CheckActivitiesEndDate(DateTime endDate, DateTime startDate, int moduleId)
        {
            DateTime moduleStartDate = GetModuleStartDate(moduleId);
            DateTime moduleEndDate = GetModuleEndDate(moduleId);

            if (endDate < startDate | endDate > moduleEndDate)
            {
                return Json($"The activity has to end within the date of the module ({moduleStartDate.ToShortDateString()} to {moduleEndDate.ToShortDateString()})");
            }

            return Json(true);
        }

        private DateTime GetModuleEndDate(int moduleId)
        {
            return _context.Modules
                .Where(c => c.ModuleId == moduleId)
                .Select(s => s.EndDate)
                .FirstOrDefault();
        }

        private DateTime GetModuleStartDate(int moduleId)
        {
            return _context.Modules
                .Where(c => c.ModuleId == moduleId)
                .Select(s => s.StartDate)
                .FirstOrDefault();
        }
    }
}
