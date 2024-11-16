namespace CMSAPI.Models {
    public class Folder {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? ParentFolderId { get; set; }
        public string IdentityUserId { get; set; } = null!; // ID of the IdentityUser who owns the folder
    }
}