using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoreLMS.Core.Models;
using CoreLMS.Data;
using Microsoft.AspNetCore.Http;
using System.IO;
using CoreLMS.Core.ViewModels;
using Microsoft.AspNetCore.Hosting;

namespace CoreLMS.Controllers
{
    public class DocumentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        

        public DocumentsController(ApplicationDbContext context)
        {
            _context = context;
            
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(UploadFile model)
        {
            if (ModelState.IsValid)
            {
                string FileName = null;
                string filePath = null;

                if (model.File != null)
                {

                    string projectDir = System.IO.Directory.GetCurrentDirectory();
                    var uploadsFolder = Path.Combine(projectDir, "DOX");
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
                    TypeOfDocument=model.TypeOfDocument,
                    LMSUserId = user.Id

                };


                if (ModelState.IsValid)
                {
                    _context.Add(document);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

               
            }

            return View();
        }













        // GET: Documents
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Document.Include(d => d.Activity).Include(d => d.Course).Include(d => d.LMSUser).Include(d => d.Module);
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
