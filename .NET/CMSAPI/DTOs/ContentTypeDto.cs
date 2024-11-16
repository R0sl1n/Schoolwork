namespace CMSAPI.DTOs
{
    public class ContentTypeDto
    {
        public int Id { get; set; }
        public string? Type { get; set; }
    }

    public class CreateContentTypeDto
    {
        public string? Type { get; set; }
    }

    public class UpdateContentTypeDto
    {
        public string? Type { get; set; }
    }
}