namespace HutechITEvent.Models
{
    public class Contest
    {
        public int Id { get; set; }
        
        public string Title { get; set; } = string.Empty;
        
        public string Description { get; set; } = string.Empty;
        
        public DateTime StartDate { get; set; }
        
        public DateTime EndDate { get; set; }
        
        public DateTime SubmissionDeadline { get; set; }
        
        public string Rules { get; set; } = string.Empty;
        
        public string Prizes { get; set; } = string.Empty;
        
        public bool AllowIndividual { get; set; } = true;
        
        public bool AllowTeam { get; set; } = true;
        
        public int MinTeamSize { get; set; } = 1;
        
        public int MaxTeamSize { get; set; } = 5;
        
        public string? ThumbnailUrl { get; set; }
        
        public int CategoryId { get; set; }
        
        public int? OrganizerId { get; set; }
        
        public ContestStatus Status { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        // Navigation properties
        public Category Category { get; set; } = null!;
        public User? Organizer { get; set; }
        public ICollection<ContestImage> Images { get; set; } = new List<ContestImage>();
        public ICollection<ContestRegistration> ContestRegistrations { get; set; } = new List<ContestRegistration>();
    }
    
    public enum ContestStatus
    {
        Draft,
        Published,
        RegistrationOpen,
        InProgress,
        Judging,
        Completed,
        Cancelled
    }
}
