namespace HutechITEvent.Models
{
    public class ContestRegistration
    {
        public int Id { get; set; }
        
        public int ContestId { get; set; }
        
        public RegistrationType Type { get; set; }
        
        public string? TeamName { get; set; }
        
        public int LeaderId { get; set; }
        
        public string? ProjectName { get; set; }
        
        public string? ProjectDescription { get; set; }
        
        public string? SubmissionUrl { get; set; }
        
        public DateTime? SubmittedAt { get; set; }
        
        public ContestRegistrationStatus Status { get; set; }
        
        public decimal? Score { get; set; }
        
        public string? Rank { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        // Navigation properties
        public Contest Contest { get; set; } = null!;
        public Student Leader { get; set; } = null!;
        public ICollection<ContestMember> Members { get; set; } = new List<ContestMember>();
    }
    
    public enum RegistrationType
    {
        Individual,
        Team
    }
    
    public enum ContestRegistrationStatus
    {
        Registered,
        Submitted,
        UnderReview,
        Approved,
        Rejected,
        Winner
    }
}
