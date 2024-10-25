using BloggersUnite.Controllers;
using BloggersUnite.Data.Repositories.Interfaces;
using BloggersUnite.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using System.Collections.Generic;
using BloggersUnite.Data.Repositories;

namespace BloggersUnite.Tests.Controllers
{
    public class PostControllerTest
    {
        private readonly Mock<IPostRepository> _mockPostRepo;
        private readonly Mock<IBlogRepository> _mockBlogRepo;
        private readonly Mock<ICommentRepository> _mockCommentRepo;
        private readonly Mock<UserManager<IdentityUser>> _mockUserManager;
        private readonly PostController _controller;

        public PostControllerTest()
        {
            _mockPostRepo = new Mock<IPostRepository>();
            _mockBlogRepo = new Mock<IBlogRepository>();
            _mockCommentRepo = new Mock<ICommentRepository>();
            _mockUserManager = MockUserManager();

            _controller = new PostController(
                _mockPostRepo.Object,
                _mockBlogRepo.Object,
                _mockCommentRepo.Object,
                _mockUserManager.Object
            );
        }

        private Mock<UserManager<IdentityUser>> MockUserManager()
        {
            var store = new Mock<IUserStore<IdentityUser>>();
            return new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
        }

        // Details metode Tests
        [Fact]
        public void Details_ReturnsNotFound_WhenPostIsNull()
        {
            // Arrange
            _mockPostRepo.Setup(repo => repo.GetPostByIdWithBlog(1)).Returns((Post)null);

            // Act
            var result = _controller.Details(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Details_ReturnsViewResult_WithPostAndComments()
        {
            
            var post = new Post
            {
                Id = 1,
                Comments = new List<Comment>
                {
                    new Comment { Id = 1, OwnerId = "User1", Owner = new IdentityUser { Email = "user1@example.com" } },
                    new Comment { Id = 2, OwnerId = "User2", Owner = new IdentityUser { Email = "user2@example.com" } }
                }
            };
            var user1 = new IdentityUser { Id = "User1", Email = "user1@example.com" };
            var user2 = new IdentityUser { Id = "User2", Email = "user2@example.com" };

            _mockPostRepo.Setup(repo => repo.GetPostByIdWithBlog(1)).Returns(post);
            _mockUserManager.Setup(m => m.FindByIdAsync("User1")).ReturnsAsync(user1);
            _mockUserManager.Setup(m => m.FindByIdAsync("User2")).ReturnsAsync(user2);

            // Act
            var result = _controller.Details(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Post>(viewResult.ViewData.Model);
            Assert.Equal(1, model.Id);
            Assert.Equal(2, model.Comments.Count);
            Assert.Equal("user1@example.com", model.Comments[0].Owner.Email);
        }

        // Create Method Tests
        [Fact]
        public void Create_Post_InvalidModel_ReturnsView()
        {
            // Arrange
            _controller.ModelState.AddModelError("Title", "Required");

            // Act
            var result = _controller.Create(new Post());

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<Post>(viewResult.ViewData.Model);
        }

        [Fact]
        public void Create_Forbids_WhenBlogNotAllowsPosts()
        {
            // Arrange
            var post = new Post { BlogId = 1, Title = "New Post" };
            var blog = new Blog { Id = 1, AllowPosts = false };
            _mockBlogRepo.Setup(repo => repo.GetBlogById(1)).Returns(blog);

            // Act
            var result = _controller.Create(post);

            // Assert
            Assert.IsType<ForbidResult>(result);
        }

        // New Test: Ensuring Comment Creation is Forbidden for Closed Blogs
        [Fact]
        public void CreateComment_Forbids_WhenBlogIsClosed()
        {
            // Arrange
            var post = new Post { Id = 1, BlogId = 1 };
            var blog = new Blog { Id = 1, IsOpen = false };
            _mockPostRepo.Setup(repo => repo.GetPostById(1)).Returns(post);
            _mockBlogRepo.Setup(repo => repo.GetBlogById(1)).Returns(blog);

            // Act
            var result = _controller.CreateComment(1, "New Comment");

            // Assert
            Assert.IsType<ForbidResult>(result);
        }

        // New Test: Ensuring ModelState is invalid in Create method
        [Fact]
        public void Create_ReturnsView_WhenModelStateIsInvalid()
        {
            // Arrange
            var post = new Post { BlogId = 1, Title = "New Post" };
            _mockBlogRepo.Setup(repo => repo.GetBlogById(1)).Returns(new Blog { Id = 1, AllowPosts = true });
            _controller.ModelState.AddModelError("Content", "Required");

            // Act
            var result = _controller.Create(post);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<Post>(viewResult.ViewData.Model);
        }

        // Edit Method Tests
        [Fact]
        public void Edit_ReturnsForbid_WhenUserIsNotOwner()
        {
            // Arrange
            var post = new Post { Id = 1, OwnerId = "DifferentUser" };
            _mockPostRepo.Setup(repo => repo.GetPostById(1)).Returns(post);
            _mockUserManager.Setup(m => m.GetUserId(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).Returns("UserId");

            // Act
            var result = _controller.Edit(1);

            // Assert
            Assert.IsType<ForbidResult>(result);
        }

        [Fact]
        public void Edit_ReturnsViewResult_WhenUserIsOwner()
        {
            // Arrange
            var post = new Post { Id = 1, OwnerId = "UserId" };
            _mockPostRepo.Setup(repo => repo.GetPostById(1)).Returns(post);
            _mockUserManager.Setup(m => m.GetUserId(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).Returns("UserId");

            // Act
            var result = _controller.Edit(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Post>(viewResult.ViewData.Model);
            Assert.Equal(1, model.Id);
        }

        [Fact]
        public void Edit_Post_ReturnsForbid_WhenUserIsNotOwner()
        {
            // Arrange
            var post = new Post { Id = 1, OwnerId = "DifferentUser" };
            var updatedPost = new Post { Id = 1, Title = "Updated Post" };

            _mockPostRepo.Setup(repo => repo.GetPostById(1)).Returns(post);
            _mockUserManager.Setup(m => m.GetUserId(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).Returns("UserId");

            // Act
            var result = _controller.Edit(updatedPost);

            // Assert
            Assert.IsType<ForbidResult>(result);
        }

        [Fact]
        public void Edit_Post_RedirectsToBlogDetails_WhenModelStateIsValidAndUserIsOwner()
        {
            // Arrange
            var post = new Post { Id = 1, OwnerId = "UserId" };
            var updatedPost = new Post { Id = 1, Title = "Updated Post", BlogId = 1 };

            _mockPostRepo.Setup(repo => repo.GetPostById(1)).Returns(post);
            _mockUserManager.Setup(m => m.GetUserId(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).Returns("UserId");

            // Act
            var result = _controller.Edit(updatedPost);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Details", redirectToActionResult.ActionName);
            Assert.Equal("Blog", redirectToActionResult.ControllerName);
        }

        // Delete Method Tests
        [Fact]
        public void Delete_ReturnsNotFound_WhenPostIsNull()
        {
            // Arrange
            _mockPostRepo.Setup(repo => repo.GetPostById(1)).Returns((Post)null);

            // Act
            var result = _controller.Delete(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void DeleteConfirmed_RemovesPostAndRedirectsToBlogDetails()
        {
            // Arrange
            var post = new Post { Id = 1, BlogId = 1, OwnerId = "UserId" };
            _mockPostRepo.Setup(repo => repo.GetPostById(1)).Returns(post);
            _mockUserManager.Setup(m => m.GetUserId(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).Returns("UserId");

            // Act
            var result = _controller.DeleteConfirmed(1);

            // Assert
            _mockPostRepo.Verify(r => r.DeletePost(post), Times.Once);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Details", redirectToActionResult.ActionName);
            Assert.Equal("Blog", redirectToActionResult.ControllerName);
        }

        [Fact]
        public void Delete_ReturnsForbid_WhenUserIsNotOwner()
        {
            // Arrange
            var post = new Post { Id = 1, OwnerId = "DifferentUser" };
            _mockPostRepo.Setup(repo => repo.GetPostById(1)).Returns(post);
            _mockUserManager.Setup(m => m.GetUserId(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).Returns("UserId");

            // Act
            var result = _controller.Delete(1);

            // Assert
            Assert.IsType<ForbidResult>(result);
        }
    }
}
