using Microsoft.AspNetCore.Identity;

namespace HutechITEvent.Models
{
    public class User : IdentityUser<int>
    {
        // IdentityUser ?ã có s?n: Id, Email, PasswordHash, PhoneNumber, UserName
        
        public string FullName { get; set; } = string.Empty;
        
        public string? Avatar { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public DateTime? LastLogin { get; set; }
        
        // Navigation properties
        public Student? Student { get; set; }
        public ICollection<Event> OrganizedEvents { get; set; } = new List<Event>();
        public ICollection<Contest> OrganizedContests { get; set; } = new List<Contest>();
    }
}
