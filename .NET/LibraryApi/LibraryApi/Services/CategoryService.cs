using LibraryApi.Data;
using LibraryApi.Models;
using LibraryApi.Dtos;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Services
{
    // Service class for managing category-related operations in the application
    public class CategoryService
    {
        private readonly LibraryDbContext _context;

        // Constructor that initializes the service with the database context
        public CategoryService(LibraryDbContext context)
        {
            _context = context;
        }

        // Retrieves all categories from the database, including their associated books, and maps them to DTOs
        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _context.Categories.Include(c => c.Books).ToListAsync();
            return categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                BookTitles = c.Books?.Select(b => b.Title).ToList() ?? new List<string>()
            });
        }

        // Retrieves a specific category by its ID and maps it to a DTO
        public async Task<CategoryDto?> GetCategoryByIdAsync(int id)
        {
            var category = await _context.Categories.Include(c => c.Books).FirstOrDefaultAsync(c => c.Id == id);
            if (category == null) return null;

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                BookTitles = category.Books?.Select(b => b.Title).ToList() ?? new List<string>()
            };
        }

        // Retrieves a category by its name
        public async Task<Category?> GetCategoryByNameAsync(string name)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower());
        }

        // Adds a new category to the database
        public async Task<Category> AddCategoryAsync(Category category)
        {
            // Check if exists and convert to lower case
            var existingCategory = await _context.Categories
                .FirstOrDefaultAsync(c => c.Name.ToLower() == category.Name.ToLower());

            if (existingCategory != null)
            {
                throw new InvalidOperationException("A category with the same name already exists.");
            }

            // Add if no existing category was found
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }


        // Updates an existing category based on the provided data
        public async Task<bool> UpdateCategoryAsync(Category category)
        {
            var existingCategory = await _context.Categories.FindAsync(category.Id);
            if (existingCategory == null)
            {
                return false;
            }

            // Update the name of the existing category
            existingCategory.Name = category.Name;
            await _context.SaveChangesAsync();
            return true;
        }

        // Deletes a category from the database based on its ID
        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return false;
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
