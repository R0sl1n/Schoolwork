using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace BloggersUnite.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; } // Primary Key

        [ForeignKey("Post")]
        public int PostId { get; set; } // Foreign Key to Post

        // Navigational property to Post
        public virtual Post Post { get; set; } = null!; // Use virtual for lazy loading and ensure it's not null

        [Required]
        public string Content { get; set; } = null!; // Ensure the Content is never null

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [ForeignKey("AspNetUsers")] // Foreign Key to the user who created the comment
        public string? OwnerId { get; set; }

        // Navigational property to the Owner (user)
        public virtual IdentityUser Owner { get; set; } = null!; // Use virtual for lazy loading and ensure it's not null
    }
}