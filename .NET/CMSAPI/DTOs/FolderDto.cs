using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CMSAPI.Models;

namespace CMSAPI.DTOs {
    public class FolderDto {
        public int Id { get; set; }
        public string? Name { get; set; }
        public Folder? ParentFolder { get; set; }
        public List<DocumentDto>? Documents { get; set; }
        public string Url { get; set; }
    }


    public class CreateFolderDto {
        [Required(ErrorMessage = "Folder name required!")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Parent folder required!")]
        public int? ParentFolderId { get; set; } = null!;
    }

    public class UpdateFolderDto {

        [Required(ErrorMessage = "Folder name required!")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Parent folder required!")]
        public int ParentFolderId { get; set; }
    }

}