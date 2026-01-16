using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace ToDoList.Models
{
    public class ToDoListDbContext : DbContext
    {
        public ToDoListDbContext(DbContextOptions<ToDoListDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Activity> Activities { get; set; }
    }
}
