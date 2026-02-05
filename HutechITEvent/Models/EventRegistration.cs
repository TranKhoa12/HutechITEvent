namespace HutechITEvent.Models
{
    public class EventRegistration
    {
        public int Id { get; set; }
        
        public int EventId { get; set; }
        
        public int StudentId { get; set; }
        
        public DateTime RegisteredAt { get; set; } = DateTime.Now;
        
        public RegistrationStatus Status { get; set; }
        
        public bool IsAttended { get; set; }
        
        public string? Notes { get; set; }
        
        // Navigation properties
        public Event Event { get; set; } = null!;
        public Student Student { get; set; } = null!;
    }
    
    public enum RegistrationStatus
    {
        Pending,
        Confirmed,
        Cancelled
    }
}
