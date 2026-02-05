namespace HutechITEvent.Models
{
    public class EventSchedule
    {
        public int Id { get; set; }
        
        public int EventId { get; set; }
        
        public string Title { get; set; } = string.Empty;
        
        public string? Speaker { get; set; }
        
        public string? SpeakerBio { get; set; }
        
        public DateTime StartTime { get; set; }
        
        public DateTime EndTime { get; set; }
        
        public string? Location { get; set; }
        
        // Navigation property
        public Event Event { get; set; } = null!;
    }
}
