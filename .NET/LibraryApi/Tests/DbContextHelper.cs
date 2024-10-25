using LibraryApi.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace Tests
{
    public static class DbContextHelper
    {
        public static LibraryDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique database pr test
                .Options;

            var context = new LibraryDbContext(options);
            context.Database.EnsureCreated();
            return context;
        }
    }
}