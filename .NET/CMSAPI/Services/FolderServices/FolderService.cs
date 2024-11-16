using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMSAPI.Data;
using CMSAPI.DTOs;
using CMSAPI.Models;
using CMSAPI.Services.DocumentServices;
using Microsoft.EntityFrameworkCore;

namespace CMSAPI.Services.FolderServices;
public class FolderService : IFolderService {

    private readonly CMSAPIDbContext _context;
    private readonly IDocumentService _documentService;

    public FolderService(CMSAPIDbContext context, IDocumentService documentService) {
        _documentService = documentService;
        _context = context;

    }

    public async Task<List<FolderDto>> GetAllFoldersAsync(string userId) {
        try {
            var folders = await _context.Folders.Where(f => f.IdentityUserId == userId).ToListAsync();

            var documents = await _documentService.GetAllDocumentsAsync(userId);

            var foldersDto = folders.Select(f => {

                var parent = folders.FirstOrDefault(p => p.Id == f.ParentFolderId);
                var foldersDocuments = documents.Where(d => d.FolderId == f.Id).ToList();


                string url = getFullUrl(folders, f);

                return new FolderDto() {
                    Id = f.Id,
                    Name = f.Name,
                    ParentFolder = parent,
                    Documents = foldersDocuments,
                    Url = url
                };
            }).ToList();

            return foldersDto;
        }

        catch (Exception ex) {
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);

            return new List<FolderDto>();

        }
    }

    public async Task<FolderDto?> GetFolderByRouteAsync(string userId, string name) {
        try {
            var folders = await _context.Folders.Where(f => f.IdentityUserId == userId).ToListAsync();

            var splitName = name.Split("/").ToList();

            var folder = traverseToTarget(folders, splitName);

            if (folder == null) { return null; }

            return await createReturnFolderDto(userId, folder, folders);
        }
        catch (Exception e) {
            Console.WriteLine(e);
            throw;
        }

    }



    public async Task<FolderDto?> GetFolderByNameAndParentAsync(string userId, string name, int? ParentId) {
        try {
            var folders = await _context.Folders.Where(f => f.IdentityUserId == userId).ToListAsync();

            var namedFolder = folders.FirstOrDefault(f => f.Name == name && f.ParentFolderId == ParentId);

            if (namedFolder == null) { return null; }

            return await createReturnFolderDto(userId, namedFolder, folders);
        }
        catch (Exception e) {
            Console.WriteLine(e);
            throw;
        }

    }

    public async Task<FolderDto?> GetFolderByIdAsync(string userId, int id) {
        try {
            var folders = await _context.Folders.Where(f => f.IdentityUserId == userId).ToListAsync();

            var folder = folders
                .FirstOrDefault(f => f.Id == id);

            if (folder == null) { return null; }

            return await createReturnFolderDto(userId, folder, folders);
        }
        catch (Exception e) {
            Console.WriteLine(e);
            throw;
        }

    }


    public async Task<Folder> SaveAsync(string userId, Folder folder) {
        var existingFolder = await _context.Folders.FirstOrDefaultAsync(f => f.Id == folder.Id && f.IdentityUserId == userId);

        if (existingFolder != null) {
            _context.Entry(existingFolder).State = EntityState.Detached;
        }

        _context.Folders.Update(folder);
        await _context.SaveChangesAsync();

        return await _context.Folders.FirstAsync(f => f.IdentityUserId == userId && f.Name == folder.Name && f.ParentFolderId == folder.ParentFolderId);

    }

    public async Task DeleteAsync(string userId, int id) {

        var folders = await _context.Folders.Where(f => f.IdentityUserId == userId).ToListAsync();
        var folder = folders
            .FirstOrDefault(f => f.Id == id);

        _context.Folders.Remove(folder);
        await _context.SaveChangesAsync();
    }

    private async Task<FolderDto> createReturnFolderDto(string userId, Folder folder, List<Folder> folders) {

        var documents = await _documentService.GetAllDocumentsAsync(userId);
        var parent = folder.ParentFolderId != null ? folders.FirstOrDefault(p => p.Id == folder.ParentFolderId) : null;

        var foldersDocuments = documents.Where(d => d.FolderId == folder.Id).ToList();

        var url = getFullUrl(folders, folder);

        var folderDto = new FolderDto() {
            Id = folder.Id,
            Name = folder.Name,
            ParentFolder = parent,
            Documents = foldersDocuments,
            Url = url
        };

        return folderDto;
    }

    private string getFullUrl(List<Folder> folders, Folder folder) {
        var url = "/" + folder.Name;
        var ancestors = getAncestors(folders, folder);

        if (ancestors.Count < 1) return url;

        foreach (var ancestor in ancestors) {

            var delimitor = ancestor.Name == "Root" ? "" : "/";

            url = delimitor + ancestor.Name + url;
        }

        return url;
    }

    private List<Folder> getAncestors(List<Folder> folders, Folder folder) {
        var ancestors = new List<Folder>() { };

        Folder? parent = folders.FirstOrDefault(p => p.Id == folder.ParentFolderId);

        while (parent != null) {
            ancestors.Add(parent);

            parent = folders.FirstOrDefault(p => p.Id == parent.ParentFolderId);

        }

        return ancestors;

    }

    private Folder? traverseToTarget(List<Folder> folders, List<String> splitFolders) {
        splitFolders.Reverse();

        var finalTarget = folders.FirstOrDefault(t => t.Name == splitFolders[0]);

        if (finalTarget == null) {
            return null;
        }

        for (var i = 1; i < splitFolders.Count; i++) {
            var nextFolder = folders.FirstOrDefault(nf => nf.Name == splitFolders[i]);

            if (nextFolder == null) {
                return null;
            }

            if (nextFolder.ParentFolderId != null) {
                continue;
            }
        }

        return finalTarget;

    }

    public async Task<Folder?> GetUserRootFolder(string userId) {
        return await _context.Folders.FirstOrDefaultAsync(rf => rf.IdentityUserId == userId && rf.ParentFolderId == null);
    }

    public async Task CreateRootFolder(string userId) {
        var folder = new Folder() {
            Name = "Root",
            IdentityUserId = userId,
            ParentFolderId = null
        };
        _context.Folders.Update(folder);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Folder>> GetFolderChildrenAsync(FolderDto? folder) {

        if (folder == null) {
            return new List<Folder>();
        }

        var children = await _context.Folders.Where(f => f.ParentFolderId == folder.Id).ToListAsync();

        return children;
    }

}
