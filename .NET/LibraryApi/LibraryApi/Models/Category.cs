using System.ComponentModel.DataAnnotations;

namespace LibraryApi.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; }
        public ICollection<Book>? Books { get; set; }
    }
}