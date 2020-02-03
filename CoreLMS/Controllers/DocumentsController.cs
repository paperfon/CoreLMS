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
                string FileName = null;
                string filePath = null;

                if (model.File != null)
                {

                    string projectDir = System.IO.Directory.GetCurrentDirectory();
                    var uploadsFolder = Path.Combine(projectDir, "Dox");
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
                    LMSUserId = user.Id

                };

                if (model.selectedentity == "Course")
                {
                    document.CourseId = model.selectedentityid;
                }
                else
               if (model.selectedentity == "Module")
                {
                    document.ModuleId = model.selectedentityid;
                }
                else
               if (model.selectedentity == "Activity")
                {
                    document.ActivityId = model.selectedentityid;
                }


                if (ModelState.IsValid)
                {
                    _context.Add(document);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }


            }

            return View();
        }



        [Route("Create/{id}/{name}")]
        [HttpGet]
        public ViewResult Create(int? id, string name)
        {
            List<Entity> Lmslentities = Enum.GetValues(typeof(Entity)).Cast<Entity>().ToList();
            ViewBag.lmsentitylist = new SelectList(Lmslentities);

           return View();
        }


        [Route("Create/{id}/{name}")]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(UploadFile model, int id, string name)
        {
            if (ModelState.IsValid)
            {
                string FileName = null;
                string filePath = null;

                if (model.File != null)
                {

                    string projectDir = System.IO.Directory.GetCurrentDirectory();
                    var uploadsFolder = Path.Combine(projectDir, "wwwroot/DOX");
                    FileName = Path.GetFileName(model.File.FileName); 
                    filePath = Path.Combine(uploadsFolder,FileName);
                    model.File.CopyTo(new FileStream(filePath, FileMode.Append));
                }


                var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                var document = new Document
                {

                    DocumentName = model.DocumentName,
                    UploadTime = DateTime.Now,
                    DocumentPath = filePath,
                    TypeOfDocument = model.TypeOfDocument,
                    LMSUserId = user.Id

                };

                if (name == "Course")
                {
                    document.CourseId = id;
                }
                else
               if (name == "Module")
                {
                    document.ModuleId = id;
                }
                else
               if (name == "Activity")
                {
                    document.ActivityId = id;
                }

                if (ModelState.IsValid)
                {
                    _context.Add(document);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

               
            }

            return View();
        }


        public ViewResult GetListBasedonSelectedEntity(string entityname)
        {
            if (entityname == "Course")
            {
                var Courselist = _context.Course.Select(c => c.CourseName).ToList();
                ViewBag.entitylist = Courselist;
            }
            else if (entityname == "Module")
            {

                var Modulelist = _context.Module.Select(m => m.ModuleName).ToList();
                ViewBag.entitylist = Modulelist;
            }
            else if (entityname == "Activity")
            {
                var Activitylist = _context.Activity.Select(a => a.ActivityName).ToList();
                ViewBag.entitylist = Activitylist;
            }

            return View();
        }

      

        // GET: Documents
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Document.Include(d => d.Activity).Include(d => d.Course).Include(d => d.LMSUser).Include(d => d.Module);


            foreach (var item in applicationDbContext)
            {
                ViewBag.file = item.DocumentPath;
                item.DocumentPath = Path.GetFileName(item.DocumentPath);
                
            }
            

            return View(await applicationDbContext.ToListAsync());
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

        // GET: Documents/Create
        //public IActionResult Create()
        //{
        //    ViewData["ActivityId"] = new SelectList(_context.Set<Activity>(), "ActivityId", "ActivityName");
        //    ViewData["CourseId"] = new SelectList(_context.Set<Course>(), "CourseId", "CourseName");
        //    ViewData["LMSUserId"] = new SelectList(_context.Set<LMSUser>(), "Id", "Id");
        //    ViewData["ModuleId"] = new SelectList(_context.Set<Module>(), "ModuleId", "ModuleName");
        //    return View();
        //}

        // POST: Documents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("DocumentId,DocumentName,UploadTime,DocumentPath,TypeOfDocument,LMSUserId,CourseId,ModuleId,ActivityId")] Document document)
        //{
        //    document.UploadTime = DateTime.Now;
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(document);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["ActivityId"] = new SelectList(_context.Set<Activity>(), "ActivityId", "ActivityName", document.ActivityId);
        //    ViewData["CourseId"] = new SelectList(_context.Set<Course>(), "CourseId", "CourseName", document.CourseId);
        //    ViewData["LMSUserId"] = new SelectList(_context.Set<LMSUser>(), "Id", "Id", document.LMSUser);
        //    ViewData["ModuleId"] = new SelectList(_context.Set<Module>(), "ModuleId", "ModuleName", document.ModuleId);
        //    return View(document);
        //}

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
            ViewData["ActivityId"] = new SelectList(_context.Set<Activity>(), "ActivityId", "ActivityName", document.ActivityId);
            ViewData["CourseId"] = new SelectList(_context.Set<Course>(), "CourseId", "CourseName", document.CourseId);
            ViewData["LMSUserId"] = new SelectList(_context.Set<LMSUser>(), "Id", "Id", document.LMSUserId);
            ViewData["ModuleId"] = new SelectList(_context.Set<Module>(), "ModuleId", "ModuleName", document.ModuleId);
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
            ViewData["ActivityId"] = new SelectList(_context.Set<Activity>(), "ActivityId", "ActivityName", document.ActivityId);
            ViewData["CourseId"] = new SelectList(_context.Set<Course>(), "CourseId", "CourseName", document.CourseId);
            ViewData["LMSUserId"] = new SelectList(_context.Set<LMSUser>(), "Id", "Id", document.LMSUserId);
            ViewData["ModuleId"] = new SelectList(_context.Set<Module>(), "ModuleId", "ModuleName", document.ModuleId);
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
