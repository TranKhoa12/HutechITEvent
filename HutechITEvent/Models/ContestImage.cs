namespace HutechITEvent.Models
{
    public class ContestImage
    {
        public int Id { get; set; }
        
        public int ContestId { get; set; }
        
        public string ImageUrl { get; set; } = string.Empty;
        
        public string? Caption { get; set; }
        
        public int DisplayOrder { get; set; }
        
        // Navigation property
        public Contest Contest { get; set; } = null!;
    }
}
