namespace LibraryApi.Dtos
{
    // Data Transfer Object (DTO) for representing a Category entity in the application
    public class CategoryDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public List<string>? BookTitles { get; set; }
    }
}