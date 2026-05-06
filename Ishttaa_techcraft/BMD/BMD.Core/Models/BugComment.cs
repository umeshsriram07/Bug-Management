using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BMD.Core.Models
{
    public class BugComment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int BugId { get; set; }

        [Required]
        public int CommentedBy { get; set; }

        [Required]
        public string Comment { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        [ForeignKey("BugId")]
        public Bug? Bug { get; set; }

        [ForeignKey("CommentedBy")]
        public User? User { get; set; }
    }
}

