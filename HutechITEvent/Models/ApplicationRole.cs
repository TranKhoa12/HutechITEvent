using Microsoft.AspNetCore.Identity;

namespace HutechITEvent.Models
{
    public class ApplicationRole : IdentityRole<int>
    {
        public string Description { get; set; } = string.Empty;
    }
    
    public static class RoleNames
    {
        public const string Admin = "Admin";
        public const string Lecturer = "Lecturer";
        public const string Student = "Student";
    }
}