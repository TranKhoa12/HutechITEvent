namespace HutechITEvent.Models
{
    public class Role
    {
        public int Id { get; set; }
        
        public string Name { get; set; } = string.Empty;
        
        public string Description { get; set; } = string.Empty;
        
        // Navigation properties
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
    
    public static class RoleNames
    {
        public const string Admin = "Admin";
        public const string Lecturer = "Lecturer";
        public const string Student = "Student";
    }
}
