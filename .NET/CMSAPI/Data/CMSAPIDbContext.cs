using CMSAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CMSAPI.Data {
    // DbContext class for the CMS API, extending IdentityDbContext to support IdentityUser
    public class CMSAPIDbContext : IdentityDbContext<IdentityUser> {
        public CMSAPIDbContext(DbContextOptions<CMSAPIDbContext> options) : base(options) { }

        // DbSet for the CMS-specific User table, separate from Identity's default users table
        public DbSet<User> CMSUsers { get; set; }

        // DbSet for documents, representing the documents table in the database
        public DbSet<Document> Documents { get; set; }

        // DbSet for folders, representing the folders table in the database
        public DbSet<Folder> Folders { get; set; }

        // DbSet for content types, representing the content types table in the database
        public DbSet<ContentType> ContentTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            // Folder relationship with IdentityUser
            modelBuilder.Entity<Folder>()
                .HasOne<IdentityUser>()
                .WithMany() // No navigation collection on IdentityUser
                .HasForeignKey(f => f.IdentityUserId).OnDelete(DeleteBehavior.Cascade);

            // Document relationship with IdentityUser
            modelBuilder.Entity<Document>()
                .HasOne(d => d.IdentityUser)
                .WithMany() // No navigation collection on IdentityUser
                .HasForeignKey(d => d.IdentityUserId);
        }

    }
}
