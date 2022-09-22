using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

using ImageSharingWithModel.Models;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace ImageSharingWithModel.DAL
{
    public class ApplicationDbInitializer 
    {
        private ApplicationDbContext db;
        private ILogger<ApplicationDbInitializer> logger;
        public ApplicationDbInitializer(ApplicationDbContext db, ILogger<ApplicationDbInitializer> logger)
        {
            this.db = db;
            this.logger = logger;
        }
        public void SeedDatabase()
        {
            // Ensure that the database has been migrated (tables created).

            db.Database.Migrate();

            db.RemoveRange(db.Images);
            db.RemoveRange(db.Tags);
            db.RemoveRange(db.Users);
            db.SaveChanges();

            User jfk = new User { Username = "jfk", ADA = false };
            db.Users.Add(jfk);
            User nixon = new User { Username = "nixon", ADA = false };
            db.Users.Add(nixon);
            User obama = new User { Username = "obama", ADA = false };
            db.Users.Add(obama);
            User washington = new User { Username = "washington", ADA = true };
            db.Users.Add(washington);

            Tag nature = new Tag { Name = "nature" };
            db.Tags.Add(nature);
            Tag sports = new Tag { Name = "sports" };
            db.Tags.Add(sports);
            Tag portrait = new Tag { Name = "portrait" };
            db.Tags.Add(portrait);
            Tag architecture = new Tag { Name = "architecture" };
            db.Tags.Add(architecture);

            db.SaveChanges();

        }
    }
}