using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebServer.Models
{
    public class User : IBase
    {
        public string Time { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public int ID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string RegisterData { get; set; }
        public int Sex { get; set; }
        public string BD { get; set; }

    }
    public static class Role
    {
        public static string Admin = "Admin";
        public static string User = "User";
        public static string Guest = "Guest";

    }
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<EventModel> AllEvent { get; set; }
        public DbSet<ArticleModel> Articles { get; set; }
        public DbSet<CommentModel> Comments { get; set; }
        public DbSet<MessageModel> Messages { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }

}
