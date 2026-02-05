namespace HutechITEvent.Models
{
    public class ContestMember
    {
        public int Id { get; set; }
        
        public int ContestRegistrationId { get; set; }
        
        public int StudentId { get; set; }
        
        public string Role { get; set; } = "Member";
        
        public DateTime JoinedAt { get; set; } = DateTime.Now;
        
        // Navigation properties
        public ContestRegistration ContestRegistration { get; set; } = null!;
        public Student Student { get; set; } = null!;
    }
}
