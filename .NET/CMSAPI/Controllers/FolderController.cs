using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;
using CMSAPI.DTOs;
using CMSAPI.Models;
using CMSAPI.Services.AuthServices;
using CMSAPI.Services.FolderServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Web;
namespace CMSAPI.Controllers;


[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FolderController : ControllerBase {


    private readonly IFolderService _folderService;
    private readonly IAuthService _authService;
    private readonly UserManager<IdentityUser> _userManager;

    public FolderController(IFolderService folderService, IAuthService authService, UserManager<IdentityUser> userManager) {
        _folderService = folderService;
        _authService = authService;
        _userManager = userManager;
    }


    private async Task<string> GetCurrentUserId() {

        return await Task.FromResult(User.FindFirstValue(ClaimTypes.NameIdentifier));
    }

    [HttpGet]
    public async Task<ActionResult<List<FolderDto>>> GetAllFolders() {
        var userId = await GetCurrentUserId();

        var folders = await _folderService.GetAllFoldersAsync(userId);

        return folders == null ? NotFound() : Ok(folders);

    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<FolderDto>> GetFolderById(int id) {
        var userId = await GetCurrentUserId();

        var folder = await _folderService.GetFolderByIdAsync(userId, id);

        return folder == null ? NotFound($"Book with id: {id} not found.") : Ok(folder);
    }

    [HttpGet("{name}")]
    public async Task<ActionResult<FolderDto>> GetFolderByRoute(string name) {
        var userId = await GetCurrentUserId();

        var folder = await _folderService.GetFolderByRouteAsync(userId, HttpUtility.UrlDecode(name));

        return folder == null ? NotFound($"Book with id: {name} not found.") : Ok(folder);
    }

    [HttpPost]
    public async Task<ActionResult<FolderDto>> CreateFolder([FromBody] CreateFolderDto createFolderDto) {

        if (!ModelState.IsValid) { return BadRequest(ModelState); }

        if (createFolderDto.ParentFolderId == null) { return BadRequest($"Folder must have a parent folder."); }

        var userId = await GetCurrentUserId();
        var existingFolder = await _folderService.GetFolderByNameAndParentAsync(userId, createFolderDto.Name, createFolderDto.ParentFolderId);

        if (existingFolder != null) {
            return Conflict($"A folder with this name already exists at this route: {existingFolder.Url}");
        }

        var newFolder = await _folderService.SaveAsync(userId, new Folder() {
            Name = createFolderDto.Name,
            ParentFolderId = createFolderDto.ParentFolderId,
            IdentityUserId = userId
        });

        return CreatedAtAction("Get", new { id = newFolder.Id }, newFolder);

    }


    [HttpPut("{name}")]
    public async Task<IActionResult> UpdateFolder([FromRoute] string name, [FromBody] UpdateFolderDto updateFolderDto) {

        name = HttpUtility.UrlDecode(name);

        if (updateFolderDto.ParentFolderId == null || updateFolderDto.ParentFolderId == 0) {
            return BadRequest($"Parent folder Id is invalid.");
        }

        if (name == "Root") {
            return BadRequest("Root is not allowed to be updated");
        }

        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }
        var userId = await GetCurrentUserId();

        var existingFolder = await _folderService.GetFolderByRouteAsync(userId, name);

        if (existingFolder is null) {
            return NotFound($"No folder with route {name} has been found");
        }
        Console.WriteLine("Name: " + name);
        Console.WriteLine("existing Name: " + existingFolder.ParentFolder.Id);
        if (name != existingFolder.Url) {
            return BadRequest("Url route does not match url of object in body");
        }


        if (existingFolder.ParentFolder.Id != updateFolderDto.ParentFolderId) {
            var parentFolder = await _folderService.GetFolderByIdAsync(userId, existingFolder.Id);
            var targetParentFolder = await _folderService.GetFolderByIdAsync(userId, updateFolderDto.ParentFolderId);
            var fullUrl = parentFolder.Url + $"/{name}";

            if (targetParentFolder.Url.StartsWith(fullUrl)) {
                return BadRequest("Folder can not be moved to this location");
            }
        }

        var updatedFolder = new Folder() {
            Id = existingFolder.Id,
            Name = updateFolderDto.Name,
            ParentFolderId = updateFolderDto.ParentFolderId,
            IdentityUserId = userId
        };


        var finishedUpdate = await _folderService.SaveAsync(userId, updatedFolder);

        return Ok(new { Message = $"Folder {updateFolderDto.Name} has been updated successfully!", FolderDto = finishedUpdate });

    }


    [HttpDelete("{name}")]
    public async Task<IActionResult> DeleteFolder(string name) {

        if (name == "Root") {
            BadRequest("Root may not be deleted");
        }

        var userId = await GetCurrentUserId();
        var folder = await _folderService.GetFolderByRouteAsync(userId, HttpUtility.UrlDecode(name));

        Console.WriteLine(folder.Name);

        if (folder == null) {
            return NotFound($"Folder with {name} not found.");
        }

        foreach (var document in folder.Documents) {
            Console.WriteLine(document.Title);
            Console.WriteLine(folder.Documents.Count);
        }

        if (folder.Documents.Count > 0) {
            return BadRequest("Folder containing documents may not be deleted");
        }

        var children = await _folderService.GetFolderChildrenAsync(folder);

        if (children.Count > 0) {
            return BadRequest("Folder with children folders can not be deleted.");
        }


        await _folderService.DeleteAsync(userId, folder.Id);

        return NoContent();


    }


}
