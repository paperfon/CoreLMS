﻿using CoreLMS.Core.Models;
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
                var studentEmails = new[] { "student1@lms.se", "student2@lms.se" };

                foreach (var email in studentEmails)
                {
                    var foundUser = await userManager.FindByEmailAsync(email);
                    if (foundUser != null) continue;
                    else
                    {
                        await NewUser(adminPW, userManager, email);
                    }
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
            }
        }

        private static async Task NewUser(string adminPW, UserManager<LMSUser> userManager, string email)
        {
            var user = new LMSUser
            {
                UserName = email,
                Email = email
            };

            var result = await userManager.CreateAsync(user, adminPW);
            if (!result.Succeeded)
            {
                throw new Exception(string.Join("\n", result.Errors));
            }
        }
    }
}