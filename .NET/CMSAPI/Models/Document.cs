using System;
using Microsoft.AspNetCore.Identity;

namespace CMSAPI.Models
{
    public class Document
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; } // Content (text or image URL)
        public string? ContentType { get; set; } // Type of content ("Text", "Url")
        public DateTime CreatedDate { get; set; }
        public string IdentityUserId { get; set; } // ID of the IdentityUser who owns the document
        public IdentityUser? IdentityUser { get; set; } // Navigation property to the IdentityUser
        public int? FolderId { get; set; } // ID of the folder (can be null)
        public Folder? Folder { get; set; } // Navigation property to the folder
    }
}