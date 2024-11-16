using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMSAPI.Data;
using CMSAPI.DTOs;
using CMSAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CMSAPI.Services.DocumentServices
{
    public class DocumentService : IDocumentService
    {
        private readonly CMSAPIDbContext _context;

        public DocumentService(CMSAPIDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DocumentDto>> GetAllDocumentsAsync(string userId)
        {
            // Retrieve all documents for the user
            var documents = await _context.Documents
                .Where(d => d.IdentityUserId == userId)
                .Select(d => new DocumentDto
                {
                    Id = d.Id,
                    Title = d.Title,
                    Content = d.Content,
                    ContentType = d.ContentType,
                    CreatedDate = d.CreatedDate,
                    IdentityUserId = d.IdentityUserId,
                    FolderId = d.FolderId,
                    FolderName = d.Folder != null ? d.Folder.Name : null
                })
                .ToListAsync();

            return documents;
        }

        public async Task<DocumentDto> GetDocumentByIdAsync(int id, string userId)
        {
            // Retrieve a specific document by ID for the user
            var document = await _context.Documents
                .Include(d => d.Folder) // Include folder details
                .FirstOrDefaultAsync(d => d.Id == id && d.IdentityUserId == userId);

            if (document == null)
                return null;

            return new DocumentDto
            {
                Id = document.Id,
                Title = document.Title,
                Content = document.Content,
                ContentType = document.ContentType,
                CreatedDate = document.CreatedDate,
                IdentityUserId = document.IdentityUserId,
                FolderId = document.FolderId,
                FolderName = document.Folder?.Name
            };
        }

        public async Task<DocumentDto> CreateDocumentAsync(CreateDocumentDto createDocumentDto)
        {
            // Create a new document
            var document = new Document
            {
                Title = createDocumentDto.Title,
                Content = createDocumentDto.Content,
                ContentType = createDocumentDto.ContentType,
                CreatedDate = DateTime.UtcNow,
                IdentityUserId = createDocumentDto.IdentityUserId,
                FolderId = createDocumentDto.FolderId
            };

            _context.Documents.Add(document);
            await _context.SaveChangesAsync();

            return new DocumentDto
            {
                Id = document.Id,
                Title = document.Title,
                Content = document.Content,
                ContentType = document.ContentType,
                CreatedDate = document.CreatedDate,
                IdentityUserId = document.IdentityUserId,
                FolderId = document.FolderId,
                FolderName = document.Folder?.Name
            };
        }

        public async Task<bool> UpdateDocumentAsync(int id, UpdateDocumentDto updateDocumentDto, string userId)
        {
            // Update an existing document
            var document = await _context.Documents
                .FirstOrDefaultAsync(d => d.Id == id && d.IdentityUserId == userId);

            if (document == null)
                return false;

            document.Title = updateDocumentDto.Title;
            document.Content = updateDocumentDto.Content;
            document.ContentType = updateDocumentDto.ContentType;
            document.FolderId = updateDocumentDto.FolderId;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteDocumentAsync(int id, string userId)
        {
            // Delete a document
            var document = await _context.Documents
                .FirstOrDefaultAsync(d => d.Id == id && d.IdentityUserId == userId);

            if (document == null)
                return false;

            _context.Documents.Remove(document);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
