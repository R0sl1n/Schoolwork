namespace LibraryApi.Dtos
{
    // Data Transfer Object (DTO) for representing an Author entity in the application
    public class AuthorDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public List<string> BookTitles { get; set; } = new List<string>();
    }
}