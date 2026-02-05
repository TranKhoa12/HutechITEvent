namespace HutechITEvent.Models
{
    public class Event
    {
        public int Id { get; set; }
        
        public string Title { get; set; } = string.Empty;
        
        public string Description { get; set; } = string.Empty;
        
        public DateTime StartDate { get; set; }
        
        public DateTime EndDate { get; set; }
        
        public string Location { get; set; } = string.Empty;
        
        public int MaxParticipants { get; set; }
        
        public string? ThumbnailUrl { get; set; }
        
        public int CategoryId { get; set; }
        
        public int? OrganizerId { get; set; }
        
        public EventStatus Status { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        // Navigation properties
        public Category Category { get; set; } = null!;
        public User? Organizer { get; set; }
        public ICollection<EventImage> Images { get; set; } = new List<EventImage>();
        public ICollection<EventRegistration> Registrations { get; set; } = new List<EventRegistration>();
        public ICollection<EventSchedule> Schedules { get; set; } = new List<EventSchedule>();
    }
    
    public enum EventStatus
    {
        Draft,
        Published,
        Ongoing,
        Completed,
        Cancelled
    }
}
