using System;
using System.Collections.Generic;
using System.Text;
using CoreLMS.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CoreLMS.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

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

        public DbSet<Course> Course { get; set; }
    }
}
