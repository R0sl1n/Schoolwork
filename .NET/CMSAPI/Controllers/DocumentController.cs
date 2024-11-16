using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CMSAPI.DTOs;
using CMSAPI.Services.AuthServices;
using CMSAPI.Services.DocumentServices;
using CMSAPI.Services.FolderServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CMSAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService _documentService;
        private readonly IAuthService _authService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IFolderService _folderService;

        public DocumentController(
            IDocumentService documentService,
            IAuthService authService,
            UserManager<IdentityUser> userManager,
            IFolderService folderService)
        {
            _documentService = documentService;
            _authService = authService;
            _userManager = userManager;
            _folderService = folderService;
        }

        // Retrieves the ID of the logged in user
        private string GetCurrentUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        // Retrieves all documents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DocumentDto>>> GetAllDocuments()
        {
            var userId = GetCurrentUserId();
            var documents = await _documentService.GetAllDocumentsAsync(userId);

            // If no documents exist, include the root folder as a placeholder
            if (!documents.Any())
            {
                var rootFolder = await _folderService.GetUserRootFolder(userId);
                if (rootFolder != null)
                {
                    documents = new List<DocumentDto>
                    {
                        new DocumentDto
                        {
                            Id = 0,
                            Title = "Root Folder (no documents yet)",
                            Content = null,
                            ContentType = null,
                            CreatedDate = System.DateTime.UtcNow,
                            IdentityUserId = userId,
                            FolderId = rootFolder.Id,
                            FolderName = rootFolder.Name
                        }
                    };
                }
            }

            return Ok(documents);
        }

        // Retrieves a document by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<DocumentDto>> GetDocumentById(int id)
        {
            var userId = GetCurrentUserId();
            var document = await _documentService.GetDocumentByIdAsync(id, userId);

            if (document == null)
            {
                return NotFound($"Document with ID {id} not found for the current user.");
            }

            return Ok(document);
        }

        // Creates a new document for the logged in user.
        [HttpPost]
        public async Task<ActionResult<DocumentDto>> CreateDocument(CreateDocumentDto createDocumentDto)
        {
            var userId = GetCurrentUserId();
            createDocumentDto.IdentityUserId = userId;

            // Resolve FolderId based on FolderName if provided
            if (!createDocumentDto.FolderId.HasValue && !string.IsNullOrEmpty(createDocumentDto.FolderName))
            {
                var folder = await _folderService.GetFolderByRouteAsync(userId, createDocumentDto.FolderName);
                if (folder == null)
                {
                    return BadRequest($"Folder with name '{createDocumentDto.FolderName}' not found for the user.");
                }

                createDocumentDto.FolderId = folder.Id;
            }

            // Default to the users root folder if no FolderId or FolderName is provided
            if (!createDocumentDto.FolderId.HasValue)
            {
                var rootFolder = await _folderService.GetUserRootFolder(userId);
                if (rootFolder == null)
                {
                    return BadRequest("Root folder not found for the user.");
                }
                createDocumentDto.FolderId = rootFolder.Id;
            }

            var document = await _documentService.CreateDocumentAsync(createDocumentDto);
            return CreatedAtAction(nameof(GetDocumentById), new { id = document.Id }, document);
        }

        // Updates an existing document
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDocument(int id, UpdateDocumentDto updateDocumentDto)
        {
            var userId = GetCurrentUserId();

            // Resolve FolderId based on FolderName if provided
            if (!updateDocumentDto.FolderId.HasValue && !string.IsNullOrEmpty(updateDocumentDto.FolderName))
            {
                var folder = await _folderService.GetFolderByRouteAsync(userId, updateDocumentDto.FolderName);
                if (folder == null)
                {
                    return BadRequest($"Folder with name '{updateDocumentDto.FolderName}' not found for the user.");
                }

                updateDocumentDto.FolderId = folder.Id;
            }

            var result = await _documentService.UpdateDocumentAsync(id, updateDocumentDto, userId);

            if (!result)
            {
                return NotFound($"Document with ID {id} not found or not accessible by the current user.");
            }

            return NoContent();
        }

        // Deletes a document by ID 
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocument(int id)
        {
            var userId = GetCurrentUserId();
            var result = await _documentService.DeleteDocumentAsync(id, userId);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
