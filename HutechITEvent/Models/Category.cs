namespace HutechITEvent.Models
{
    public class Category
    {
        public int Id { get; set; }
        
        public string Name { get; set; } = string.Empty;
        
        public string Description { get; set; } = string.Empty;
        
        public CategoryType Type { get; set; }
        
        // Navigation properties
        public ICollection<Event> Events { get; set; } = new List<Event>();
        public ICollection<Contest> Contests { get; set; } = new List<Contest>();
    }
    
    public enum CategoryType
    {
        Event,
        Contest
    }
}
