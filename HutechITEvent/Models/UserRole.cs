namespace HutechITEvent.Models
{
    public class UserRole
    {
        public int Id { get; set; }
        
        public int UserId { get; set; }
        
        public int RoleId { get; set; }
        
        public DateTime AssignedAt { get; set; } = DateTime.Now;
        
        // Navigation properties
        public User User { get; set; } = null!;
        public Role Role { get; set; } = null!;
    }
}
