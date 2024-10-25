namespace LibraryApi.Dtos
{
    // Data Transfer Object (DTO) for representing a Book entity in the application
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Year { get; set; }
        public string AuthorName { get; set; } = string.Empty; 
        public string CategoryName { get; set; } = "Uncategorized"; 
    }
}