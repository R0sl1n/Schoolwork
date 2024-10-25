using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Moq;
using LibraryApi.Services;
using LibraryApi.Models;
using LibraryApi.Dtos;
using System.Threading.Tasks;
using LibraryApi.Data;

namespace Tests
{
    public static class MockHelpers
    {
        // Mock AuthorService
        public static AuthorService MockAuthorService()
        {
            var context = DbContextHelper.GetInMemoryDbContext();
            return new AuthorService(context);
        }

        // Mock BookService
        public static BookService MockBookService()
        {
            var context = DbContextHelper.GetInMemoryDbContext();
            return new BookService(context);
        }

        // Mock CategoryService
        public static CategoryService MockCategoryService()
        {
            var context = DbContextHelper.GetInMemoryDbContext();
            return new CategoryService(context);
        }
    }
}