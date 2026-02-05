using System.ComponentModel.DataAnnotations;

namespace HutechITEvent.Models
{
    public class User
    {
        public int Id { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        
        [Required]
        public string FullName { get; set; } = string.Empty;
        
        public string? PhoneNumber { get; set; }
        
        public string? Avatar { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public DateTime? LastLogin { get; set; }
        
        // Navigation properties
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public Student? Student { get; set; }
        public ICollection<Event> OrganizedEvents { get; set; } = new List<Event>();
        public ICollection<Contest> OrganizedContests { get; set; } = new List<Contest>();
    }
}
