using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models
{
    public class Activity
    {
        public int Id { get; set; }

        [Display(Name = "Name")]
        public string? Name { get; set; }

        public string? ActivityFile { get; set; }

        public IdentityUser? User { get; set; }

        public bool IsCompleted { get; set; } = false;
    }
}
