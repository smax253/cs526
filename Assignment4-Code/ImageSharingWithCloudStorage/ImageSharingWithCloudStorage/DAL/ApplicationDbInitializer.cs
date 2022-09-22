using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

using ImageSharingWithCloudStorage.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ImageSharingWithCloudStorage.DAL
{
    public  class ApplicationDbInitializer
    {
        private ApplicationDbContext db;
        private ILogContext logs;
        private ILogger<ApplicationDbInitializer> logger;

        public ApplicationDbInitializer(ApplicationDbContext db, ILogContext logs, ILogger<ApplicationDbInitializer> logger)
        {
            this.db = db;
            this.logs = logs;
            this.logger = logger;
        }

        public async Task SeedDatabase(IServiceProvider serviceProvider)
        {
            /*
             * Create image views log it doesn't already exist
             */
            await logs.CreateTableAsync();

            db.Database.Migrate();
            db.RemoveRange(db.Images);
            db.RemoveRange(db.Tags);
            db.RemoveRange(db.Users);
            db.SaveChanges();

            logger.LogInformation("Adding role: User");
            var idResult = await CreateRole(serviceProvider, "User");
            if (!idResult.Succeeded)
            {
                logger.LogError("Failed to create User role!");
            }

            await CreateRole(serviceProvider, "Admin");
            await CreateRole(serviceProvider, "Approver");
            await CreateRole(serviceProvider, "Supervisor");

            Func<String, String, String, Task> addUser = async (email, password, role) =>
            {
                logger.LogDebug("Adding user: " + email);
                idResult = await CreateAccount(serviceProvider, email, password, role);
                if (!idResult.Succeeded)
                {
                    logger.LogDebug("Failed to create " + email + " user!");
                }
            };

            await addUser("jfk@example.org", "jfk123", "Admin");
            await addUser("nixon@example.org", "nixon123", "Approver");
            await addUser("max@example.org", "max123", "Supervisor");
            await addUser("glen@example.org", "glen123", "User");
            await addUser("pat@example.org", "pat123", "User");



            Tag portrait = new Tag { Name = "portrait" };
            db.Tags.Add(portrait);
            Tag architecture = new Tag { Name = "architecture" };
            db.Tags.Add(architecture);
            Tag nature = new Tag { Name = "nature" };
            db.Tags.Add(nature);
            Tag sports = new Tag { Name = "sports" };
            db.Tags.Add(sports);

            db.SaveChanges();

        }

        public static async Task<IdentityResult> CreateRole(IServiceProvider provider,
                                                            string role)
        {
            RoleManager<IdentityRole> roleManager = provider
                .GetRequiredService
                       <RoleManager<IdentityRole>>();
            var idResult = IdentityResult.Success;
            if (await roleManager.FindByNameAsync(role) == null)
            {
                idResult = await roleManager.CreateAsync(new IdentityRole(role));
            }
            return idResult;
        }

        public static async Task<IdentityResult> CreateAccount(IServiceProvider provider,
                                                               string email, 
                                                               string password,
                                                               string role)
        {
            UserManager<ApplicationUser> userManager = provider
                .GetRequiredService
                       <UserManager<ApplicationUser>>();
            var idResult = IdentityResult.Success;

            if (await userManager.FindByNameAsync(email) == null)
            {
                ApplicationUser user = new ApplicationUser { UserName = email, Email = email };
                idResult = await userManager.CreateAsync(user, password);

                if (idResult.Succeeded)
                {
                    idResult = await userManager.AddToRoleAsync(user, role);
                }
            }

            return idResult;
        }

    }
}