namespace HutechITEvent.Models
{
    public class EventImage
    {
        public int Id { get; set; }
        
        public int EventId { get; set; }
        
        public string ImageUrl { get; set; } = string.Empty;
        
        public string? Caption { get; set; }
        
        public int DisplayOrder { get; set; }
        
        // Navigation property
        public Event Event { get; set; } = null!;
    }
}
