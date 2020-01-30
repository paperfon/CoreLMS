using Bogus;
using CoreLMS.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLMS.Data
{
    public static class SeedData
    {
        internal static async Task InitializeAsync(IServiceProvider services, string adminPW)
        {
            var options = services.GetRequiredService<DbContextOptions<ApplicationDbContext>>();
            using (var context = new ApplicationDbContext(options))
            {
                var userManager = services.GetRequiredService<UserManager<LMSUser>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                // Adding the roles to the db
                var roleNames = new[] { "Admin", "Teacher", "Student" };

                foreach (var name in roleNames)
                {
                    // If the role exists just continue
                    if (await roleManager.RoleExistsAsync(name)) continue;

                    // Otherwise create the role
                    var role = new IdentityRole
                    {
                        Name = name
                    };
                    var result = await roleManager.CreateAsync(role);
                    if (!result.Succeeded)
                    {
                        throw new Exception(string.Join("\n", result.Errors));
                    }
                }

                // Creating admins
                var adminEmails = new[] { "admin@lms.se" };

                foreach (var email in adminEmails)
                {
                    var foundUser = await userManager.FindByEmailAsync(email);
                    if (foundUser != null) continue;
                    else
                    {
                        await NewUser(adminPW, userManager, email);
                    }
                }

                // Assigning roles for the admin users
                foreach (var email in adminEmails)
                {
                    var adminUser = await userManager.FindByEmailAsync(email);
                    var adminUserRole = await userManager.GetRolesAsync(adminUser);
                    if (adminUserRole.Count > 0) continue;
                    else
                    {
                        foreach (var role in roleNames)
                        {
                            var addToRoleResult = await userManager.AddToRoleAsync(adminUser, role);
                            if (!addToRoleResult.Succeeded)
                            {
                                throw new Exception(string.Join("\n", addToRoleResult.Errors));
                            }
                        }
                    }

                }

                // Create teacher
                var teacherEmails = new[] { "teacher@lms.se" };

                foreach (var email in teacherEmails)
                {
                    var foundUser = await userManager.FindByEmailAsync(email);
                    if (foundUser != null) continue;
                    else
                    {
                        await NewUser(adminPW, userManager, email);
                    }
                }

                // Assigning roles for the teachers users
                foreach (var email in teacherEmails)
                {
                    var teacherUser = await userManager.FindByEmailAsync(email);
                    var teacherUserRole = await userManager.GetRolesAsync(teacherUser);
                    if (teacherUserRole.Count > 0) continue;
                    else
                    {
                        var addToRoleResult = await userManager.AddToRoleAsync(teacherUser, "Teacher");
                        if (!addToRoleResult.Succeeded)
                        {
                            throw new Exception(string.Join("\n", addToRoleResult.Errors));
                        }
                    }

                }

                // Creating students
                var studentEmails = new List<string> { };

                for (int i = 1; i < 21; i++)
                {
                    var email = "student" + i + "@lms.se";
                    studentEmails.Add(email);
                }

                foreach (var email in studentEmails)
                {
                    var foundUser = await userManager.FindByEmailAsync(email);
                    if (foundUser != null) continue;
                    else { await NewUser(adminPW, userManager, email); }
                }

                // Assigning roles for the students users
                foreach (var email in studentEmails)
                    {
                        var studentUser = await userManager.FindByEmailAsync(email);
                        var studentUserRole = await userManager.GetRolesAsync(studentUser);
                        if (studentUserRole.Count > 0) continue;
                        else
                        {
                            var addToRoleResult = await userManager.AddToRoleAsync(studentUser, "Student");
                            if (!addToRoleResult.Succeeded)
                            {
                                throw new Exception(string.Join("\n", addToRoleResult.Errors));
                            }
                        }
                    }

                // Faking courses, modules and activities
                var fake = new Faker();
                var r = new Random();

                var courses = new List<Course>();
                for (int i = 0; i < 5; i++)
                {
                    var course = new Course
                    {
                        CourseName = fake.Company.CompanyName(),
                        StartDate = fake.Date.Future(),
                        Description = fake.Lorem.Paragraph()
                    };
                    courses.Add(course);
                }
                context.AddRange(courses);
                context.SaveChanges();

                var modules = new List<Module>();
                for (int i = 0; i < 40; i++)
                {
                    var getCoursesIds = context.Course.Select(v => v.CourseId).ToList();
                    var randomCourseId = getCoursesIds.OrderBy(x => r.Next()).Take(1).FirstOrDefault();

                    var module = new Module
                    {
                        ModuleName = fake.Company.CatchPhrase(),
                        StartDate = fake.Date.Future(),
                        EndDate = fake.Date.Future(),
                        Description = fake.Lorem.Sentences(),
                        CourseId = randomCourseId
                    };
                    modules.Add(module);
                }
                context.AddRange(modules);
                context.SaveChanges();

                var activities = new List<Activity>();
                for (int i = 0; i < 80; i++)
                {
                    var getModulesIds = context.Module.Select(v => v.ModuleId).ToList();
                    var randomModuleId = getModulesIds.OrderBy(x => r.Next()).Take(1).FirstOrDefault();

                    var activity = new Activity
                    {
                        ActivityName = fake.Hacker.Phrase(),
                        StartDate = fake.Date.Future(),
                        EndDate = fake.Date.Future(),
                        Description = fake.Lorem.Sentences(),
                        ActivityType = (ActivityType)fake.Random.Int(0, 3),
                        ModuleId = randomModuleId
                    };
                    activities.Add(activity);
                }
                context.AddRange(activities);
                context.SaveChanges();

            }
        }

        private static async Task NewUser(string adminPW, UserManager<LMSUser> userManager, string email)
        {
            var fake = new Faker();

            var user = new LMSUser
            {
                UserName = email,
                Email = email,
                FirstName = fake.Name.FirstName(),
                LastName = fake.Name.LastName()
            };

            var result = await userManager.CreateAsync(user, adminPW);
            if (!result.Succeeded)
            {
                throw new Exception(string.Join("\n", result.Errors));
            }
        }
    }
}
