using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoreLMS.Core.Models;
using CoreLMS.Core.ViewModels;
using CoreLMS.Data;
using System.IO;

namespace CoreLMS.Controllers
{
    public class DocumentsController : Controller
    {
        private readonly ApplicationDbContext _context;


        public DocumentsController(ApplicationDbContext context)
        {
            _context = context;

        }


        public JsonResult GetEntityNamelist(string entityname)
        {


            if (entityname == "Course")
            {
                return Json(_context.Course.Select(c => new SelectListItem { Value = c.CourseId.ToString(), Text = c.CourseName }).ToList());
            }
            else if (entityname == "Module")
            {
                return Json(_context.Module.Select(m => new SelectListItem { Value = m.ModuleId.ToString(), Text = m.ModuleName }).ToList());
            }
            else if (entityname == "Activity")
            {
                return Json(_context.Activity.Select(a => new SelectListItem { Value = a.ActivityId.ToString(), Text = a.ActivityName }).ToList());
            }
            else
            {
                return Json("error");
            }


        }


        public JsonResult GetModules(int courseid)
        {
            var modulelist = _context.Module.Where(m => m.CourseId == courseid).Select(m => new SelectListItem { Value = m.ModuleId.ToString(), Text = m.ModuleName }).ToList();

            if (modulelist.Count == 0)
            {
                return Json("No Modules for this course");
            }
            else
            {
                return Json(modulelist);
            }

        }
        public JsonResult GetActivities(int moduleid)
        {
            var activitylist = _context.Activity.Where(a => a.ModuleId == moduleid).Select(a => new SelectListItem { Value = a.ActivityId.ToString(), Text = a.ActivityName }).ToList();

           if (activitylist.Count==0)
            {
                return Json("-1" , "No Activities for this course");
            }
           else
            {
                return Json(activitylist);
            }


        }


        [HttpGet]
        public ViewResult UploadDocument()
        {
            List<Entity> Lmslentities = Enum.GetValues(typeof(Entity)).Cast<Entity>().ToList();
            ViewBag.lmsentity = new SelectList(Lmslentities);



            List<TypeOfDoc> typeofdoc = Enum.GetValues(typeof(TypeOfDoc)).Cast<TypeOfDoc>().ToList();
            ViewBag.typeOfDoclist = new SelectList(typeofdoc);

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> UploadDocumentAsync(UploadFile model)
        {

            if (ModelState.IsValid)
            {
                Document document = Fileupload(model);

                if (ModelState.IsValid)
                {
                    _context.Add(document);
                    await _context.SaveChangesAsync();
                    TempData["AlertMessage"] = "Uploaded Successfully !!";
                    return RedirectToAction(nameof(Index));
                }


            }

            return View();
        }

        [Route("Create/{id}/{name}")]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(UploadFile model, int id, string name)
        {
            if (ModelState.IsValid)
            {
                model.selectedentity = name;
                model.selectedentityid = id;
                Document document = Fileupload(model);

                if (ModelState.IsValid)
                {
                    _context.Add(document);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }


            }

            return View();
        }
        private Document Fileupload(UploadFile model)
        {
            string FileName = null;
            string filePath = null;

            if (model.File != null)
            {

                string projectDir = System.IO.Directory.GetCurrentDirectory();
                var uploadsFolder = Path.Combine(projectDir, "wwwroot/Dox");
                FileName = Path.GetFileName(model.File.FileName);
                filePath = Path.Combine(uploadsFolder, FileName);
                model.File.CopyTo(new FileStream(filePath, FileMode.Create));

            }

            var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            var document = new Document
            {

                DocumentName = model.DocumentName,
                UploadTime = DateTime.Now,
                DocumentPath = filePath,
                TypeOfDocument = model.TypeOfDocument,
                LMSUserId = user.Id,

            };

            if (model.selectedentity == "Course")
            {
                document.CourseId = model.selectedentityid;

            }
            else
           if (model.selectedentity == "Module")
            {
                document.ModuleId = model.selectedentityid;
                document.CourseId = _context.Module.FirstOrDefault(m => m.ModuleId == document.ModuleId).CourseId;

            }
            else
           if (model.selectedentity == "Activity")
            {
                document.ActivityId = model.selectedentityid;
                document.ModuleId = _context.Activity.FirstOrDefault(a => a.ActivityId == document.ActivityId).ModuleId;
                document.CourseId = _context.Module.FirstOrDefault(m => m.ModuleId == document.ModuleId).CourseId;

            }

            return document;
        }


        [Route("Create/{id}/{name}")]
        [HttpGet]
        public ViewResult Create(int? id, string name)
        {

            ViewBag.typeOfDoclist = new SelectList(Enum.GetValues(typeof(TypeOfDoc)).Cast<TypeOfDoc>().ToList());

            return View();
        }





        public ViewResult GetListBasedonSelectedEntity(string entityname)
        {
            if (entityname == "Course")
            {
                ViewBag.entitylist = _context.Course.Select(c => c.CourseName).ToList(); ;
            }
            else if (entityname == "Module")
            {

                ViewBag.entitylist = _context.Module.Select(m => m.ModuleName).ToList();
            }
            else if (entityname == "Activity")
            {
                ViewBag.entitylist = _context.Activity.Select(a => a.ActivityName).ToList();
            }

            return View();
        }



        // GET: Documents
        //public async Task<IActionResult> Index()
        //{



        //    var applicationDbContext = _context.Document.Include(d => d.Activity).Include(d => d.Course).Include(d => d.LMSUser).Include(d => d.Module);


        //    foreach (var item in applicationDbContext)
        //    {
        //        ViewBag.file = item.DocumentPath;
        //        item.DocumentPath = Path.GetFileName(item.DocumentPath);

        //    }

        //    return View(await applicationDbContext.ToListAsync());
        //}



        public ActionResult Index(string sortOrder)
        {
            ViewBag.DocumentNameSortParm = String.IsNullOrEmpty(sortOrder) ? "DocumentName_desc" : "";
            ViewBag.UploadTimeSortParm = sortOrder == "UploadTime" ? "UploadTime_desc" : "UploadTime";
            ViewBag.TypeOfDocumentSortParm = sortOrder == "TypeOfDocument" ? "TypeOfDocument_desc" : "TypeOfDocument";
            ViewBag.LMSUserSortParm = sortOrder == "LMSUser" ? "LMSUser_desc" : "LMSUser";
            ViewBag.CourseSortParm = sortOrder == "Course" ? "Course_desc" : "Course";
            ViewBag.ModuleSortParm = sortOrder == "Module" ? "Module_desc" : "Module";
            ViewBag.ActivitySortParm = sortOrder == "Activity" ? "Activity_desc" : "Activity";
            ViewBag.DocumentPathSortParm = sortOrder == "DocumentPath" ? "DocumentPath_desc" : "DocumentPath";

            IQueryable<Document> documents = _context.Document.Include(d => d.Activity).Include(d => d.Course).Include(d => d.LMSUser).Include(d => d.Module) ;

            foreach (var item in documents)
            {
                ViewBag.file = item.DocumentPath;
                item.DocumentPath = Path.GetFileName(item.DocumentPath);
            }
            switch (sortOrder)
            {
                case "DocumentName_desc":
                    documents = documents.OrderByDescending(d=>d.DocumentName);
                    break;
                case "UploadTime_desc":
                    documents = documents.OrderByDescending(d => d.UploadTime);
                    break;
                case "UploadTime":
                    documents = documents.OrderBy(d => d.UploadTime);
                    break;
                case "TypeOfDocument_desc":
                    documents = documents.OrderByDescending(d => d.TypeOfDocument);
                    break;
                case "TypeOfDocument":
                    documents = documents.OrderBy(d => d.TypeOfDocument);
                    break;
                case "LMSUser_desc":
                    documents = documents.OrderByDescending(d => d.LMSUser);
                    break;
                case "LMSUser":
                    documents = documents.OrderBy(d => d.LMSUser);
                    break;
                case "Course_desc":
                    documents = documents.OrderByDescending(d => d.TypeOfDocument);
                    break;
                case "Course":
                    documents = documents.OrderBy(d => d.Course.CourseName);
                    break;
                case "Module_desc":
                    documents = documents.OrderByDescending(d => d.Course.CourseName);
                    break;
                case "Module":
                    documents = documents.OrderBy(d => d.Module.ModuleName);
                    break;
                case "Activity_desc":
                    documents = documents.OrderByDescending(d => d.Activity.ActivityName);
                    break;
                case "Activity":
                    documents = documents.OrderBy(d => d.Activity.ActivityName);
                    break;
                case "DocumentPath_desc":
                    documents = documents.OrderByDescending(d => d.DocumentPath);
                    break;
                case "DocumentPath":
                    documents = documents.OrderBy(d => d.DocumentPath);
                    break;
                default:
                    documents = documents.OrderBy(d => d.DocumentName);
                    break;
            }
            return View(documents.ToList());
        }

        // GET: Documents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.Document
                .Include(d => d.Activity)
                .Include(d => d.Course)
                .Include(d => d.LMSUser)
                .Include(d => d.Module)
                .FirstOrDefaultAsync(m => m.DocumentId == id);

            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        // GET: Documents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.Document.FindAsync(id);
            if (document == null)
            {
                return NotFound();
            }
            document.LMSUserId = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).Id;
            

            ViewData["CourseId"] = new SelectList(_context.Set<Course>(), "CourseId", "CourseName", document.CourseId);
            ViewData["ModuleId"] = new SelectList(_context.Set<Module>(), "ModuleId", "ModuleName", document.ModuleId);
            ViewData["ActivityId"] = new SelectList(_context.Set<Activity>(), "ActivityId", "ActivityName", document.ActivityId);
            return View(document);
        }

        // POST: Documents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DocumentId,DocumentName,UploadTime,DocumentPath,TypeOfDocument,LMSUserId,CourseId,ModuleId,ActivityId")] Document document)
        {
            if (id != document.DocumentId)
            {
                return NotFound();
            }

            

            if (ModelState.IsValid)
            {
                try
                {
                    document.UploadTime = DateTime.Now;
                    _context.Update(document);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocumentExists(document.DocumentId))
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
           
            ViewData["CourseId"] = new SelectList(_context.Set<Course>(), "CourseId", "CourseName", document.CourseId);
            //ViewData["ModuleId"] = new SelectList(_context.Set<Module>(), "ModuleId", "ModuleName", document.ModuleId);
            //ViewData["ActivityId"] = new SelectList(_context.Set<Activity>(), "ActivityId", "ActivityName", document.ActivityId);
            return View(document);
        }

        // GET: Documents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.Document
                .Include(d => d.Activity)
                .Include(d => d.Course)
                .Include(d => d.LMSUser)
                .Include(d => d.Module)
                .FirstOrDefaultAsync(m => m.DocumentId == id);
            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        // POST: Documents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var document = await _context.Document.FindAsync(id);
            _context.Document.Remove(document);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool DocumentExists(int id)
        {
            return _context.Document.Any(e => e.DocumentId == id);
        }
    }
}
