using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BloggersUnite.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Blog")]
        public int BlogId { get; set; }  // Foreign key to Blog

        [Required]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Content is required.")]
        public string? Content { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [ForeignKey("AspNetUsers")]
        public string? OwnerId { get; set; }

        public virtual Blog? Blog { get; set; }
        public virtual List<Comment> Comments { get; set; } = new List<Comment>();
    }
}