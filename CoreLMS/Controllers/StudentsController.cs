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
            var model = await userManager.Users
                .Select(s => new StudentListViewModel
                {
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Email = s.Email
                }).ToListAsync();

            return View(model);
        }
    }
}