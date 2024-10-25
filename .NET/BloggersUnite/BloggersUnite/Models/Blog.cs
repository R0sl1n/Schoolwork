using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BloggersUnite.Models
{
    public class Blog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Title { get; set; }

        public string? Description { get; set; }

        public DateTime CreatedDateTime { get; set; } = DateTime.Now;

        public bool IsOpen { get; set; } // Controls comments

        public bool AllowPosts { get; set; } // Controls new posts

        [ForeignKey("AspNetUsers")]
        public string? OwnerId { get; set; }

        public List<Post> Posts { get; set; } = new List<Post>(); // Initialize Posts
    }
}