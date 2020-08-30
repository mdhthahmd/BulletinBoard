using System; // Needed for DateTime
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Bulletin
    {
        public Guid Id { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required]
        public string HeadingText { get; set; }
        [Required]
        public string Content { get; set; }
        public Status Status { get; set; }
    }
    
}