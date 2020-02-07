using System;
using System.Collections.Generic;
using System.Text;
using CoreLMS.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CoreLMS.Data
{
    public class ApplicationDbContext : IdentityDbContext<LMSUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Module> Modules { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<LMSUserCourse> LMSUserCourses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Composite key
            builder.Entity<LMSUserCourse>()
                .HasKey(k => new
                {
                    k.LMSUserId,
                    k.CourseId
                });
        }
    }
}
