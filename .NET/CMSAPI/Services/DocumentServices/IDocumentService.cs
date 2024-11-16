using System.Collections.Generic;
using System.Threading.Tasks;
using CMSAPI.DTOs;

namespace CMSAPI.Services.DocumentServices
{
    public interface IDocumentService
    {
        /// Retrieve all documents for a specific user
        Task<IEnumerable<DocumentDto>> GetAllDocumentsAsync(string userId);

        /// Retrieve a single document by its ID and userId
        Task<DocumentDto> GetDocumentByIdAsync(int id, string userId);

        /// Create a new document for the specified user
        Task<DocumentDto> CreateDocumentAsync(CreateDocumentDto createDocumentDto);

        /// Update an existing document by its ID and userId
        Task<bool> UpdateDocumentAsync(int id, UpdateDocumentDto updateDocumentDto, string userId);

        /// Delete a document by its ID and userId
        Task<bool> DeleteDocumentAsync(int id, string userId);
    }
}