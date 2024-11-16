using Microsoft.EntityFrameworkCore;
using CMSAPI.Models;
using CMSAPI.Services.AuthServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace CMSAPI.Data {
    public static class SeedData {
        public static async Task Initialize(IServiceProvider serviceProvider) {
            var context = serviceProvider.GetRequiredService<CMSAPIDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var authService = serviceProvider.GetRequiredService<IAuthService>();

            // Apply pending migrations to the database
            context.Database.Migrate();

            // Check if there are any CMS users in the database
            if (!context.CMSUsers.Any()) {
                // Create an Identity user first
                var identityUser = new IdentityUser {
                    UserName = "testuser",
                    Email = "test@example.com",
                    EmailConfirmed = true
                };

                // Add the IdentityUser to the database with a password
                var result = await userManager.CreateAsync(identityUser, "Password123!");
                if (result.Succeeded) {
                    // Seed ContentTypes
                    var contentTypes = new[]
                    {
                        new ContentType { Type = "Text" },
                        new ContentType { Type = "Url" }
                    };
                    context.ContentTypes.AddRange(contentTypes);

                    // Create a CMS-specific User linked to the IdentityUser
                    var cmsUser = new User {
                        Username = identityUser.UserName,
                        Email = identityUser.Email,
                        IdentityUserId = identityUser.Id // Link CMS user to IdentityUser
                    };

                    // Create a root folder associated with the IdentityUser
                    var folder = new Folder {
                        Name = "Root",
                        IdentityUserId = identityUser.Id // Associate folder with IdentityUserId
                    };

                    // Create a sample document associated with the IdentityUser and folder
                    var document = new Document {
                        Title = "Sample Document",
                        Content = "This is a sample document.",
                        ContentType = contentTypes[0].Type,
                        CreatedDate = DateTime.Now,
                        IdentityUserId = identityUser.Id, // Associate document with IdentityUserId
                        Folder = folder
                    };

                    // Add the CMS-specific User, folder, and document to the database
                    context.CMSUsers.Add(cmsUser);
                    context.Folders.Add(folder);
                    context.Documents.Add(document);

                    // Save changes to the database
                    await context.SaveChangesAsync();

                    // Generate JWT token for the seeded user
                    var token = authService.GenerateTokenString(identityUser);

                    // Log the token to the console for easy testing in Swagger
                    Console.WriteLine($"Token for 'testuser': Bearer {token}");
                }
            }
        }
    }
}
