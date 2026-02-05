namespace HutechITEvent.Models
{
    public class Student
    {
        public int Id { get; set; }
        
        public string StudentId { get; set; } = string.Empty;
        
        public int? UserId { get; set; }
        
        public string FullName { get; set; } = string.Empty;
        
        public string Email { get; set; } = string.Empty;
        
        public string PhoneNumber { get; set; } = string.Empty;
        
        public string Class { get; set; } = string.Empty;
        
        public string Faculty { get; set; } = "Công ngh? Thông tin";
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        
        // Navigation properties
        public User? User { get; set; }
        public ICollection<EventRegistration> EventRegistrations { get; set; } = new List<EventRegistration>();
        public ICollection<ContestMember> ContestMembers { get; set; } = new List<ContestMember>();
        public ICollection<ContestRegistration> LedContestRegistrations { get; set; } = new List<ContestRegistration>();
    }
}
