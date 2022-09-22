using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;

using ImageSharingWithModel.Models;

namespace ImageSharingWithModel.DAL
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Image> Images { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Tag> Tags { get; set; }
    }
}