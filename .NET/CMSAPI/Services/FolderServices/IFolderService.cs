using System.Collections.Generic;
using System.Threading.Tasks;
using CMSAPI.DTOs;
using CMSAPI.Models;

namespace CMSAPI.Services.FolderServices;
public interface IFolderService {
    Task<List<FolderDto>> GetAllFoldersAsync(string userId);

    Task<FolderDto?> GetFolderByRouteAsync(string userId, string path);
    Task<FolderDto?> GetFolderByNameAndParentAsync(string userId, string name, int? ParentId);

    Task<FolderDto?> GetFolderByIdAsync(string userId, int id);

    Task<Folder> SaveAsync(string userId, Folder folder);

    Task DeleteAsync(string userId, int id);

    Task CreateRootFolder(string userId);

    Task<List<Folder>> GetFolderChildrenAsync(FolderDto? folder);

    Task<Folder?> GetUserRootFolder(string userId);
}
