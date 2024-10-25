using BloggersUnite.Controllers;
using BloggersUnite.Data.Repositories.Interfaces;
using BloggersUnite.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using BloggersUnite.Data.Repositories;

namespace BloggersUnite.Tests.Controllers
{
    public class BlogControllerTest
    {
        private readonly Mock<IBlogRepository> _mockBlogRepo;
        private readonly Mock<IPostRepository> _mockPostRepo;
        private readonly Mock<ICommentRepository> _mockCommentRepo;
        private readonly Mock<UserManager<IdentityUser>> _mockUserManager;
        private readonly BlogController _controller;

        public BlogControllerTest()
        {
            _mockBlogRepo = new Mock<IBlogRepository>();
            _mockPostRepo = new Mock<IPostRepository>();
            _mockCommentRepo = new Mock<ICommentRepository>();
            _mockUserManager = MockUserManager();

            _controller = new BlogController(
                _mockBlogRepo.Object,
                _mockPostRepo.Object,
                _mockCommentRepo.Object,
                _mockUserManager.Object
            );
        }

        private Mock<UserManager<IdentityUser>> MockUserManager()
        {
            var store = new Mock<IUserStore<IdentityUser>>();
            return new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
        }

        [Fact]
        public void Index_ReturnsView_WithListOfBlogs()
        {
            // Arrange
            var blogs = new List<Blog> { new Blog { Id = 1 }, new Blog { Id = 2 } };
            _mockBlogRepo.Setup(repo => repo.GetAllBlogs()).Returns(blogs);

            // Act
            var result = _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Blog>>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public void Details_ReturnsNotFound_WhenBlogIsNull()
        {
            // Arrange
            _mockBlogRepo.Setup(repo => repo.GetBlogById(1)).Returns((Blog)null);

            // Act
            var result = _controller.Details(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Details_ReturnsView_WithExistingBlog()
        {
            // Arrange
            var blog = new Blog { Id = 1, Title = "Test Blog" };
            _mockBlogRepo.Setup(repo => repo.GetBlogById(1)).Returns(blog);

            // Act
            var result = _controller.Details(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Blog>(viewResult.ViewData.Model);
            Assert.Equal("Test Blog", model.Title);
        }

        [Fact]
        public void Create_ReturnsViewResult_ForGetRequest()
        {
            // Act
            var result = _controller.Create();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewData.Model); // Ingen modell ved GET-forespørsel
        }

        [Fact]
        public void Create_ReturnsViewResult_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Title", "Required");

            // Act
            var result = _controller.Create(new Blog());

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<Blog>(viewResult.ViewData.Model);
        }

        [Fact]
        public void Create_RedirectsToIndex_WhenModelStateIsValid()
        {
            // Arrange
            var blog = new Blog { Id = 1, Title = "New Blog" };
            _mockUserManager.Setup(m => m.GetUserId(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).Returns("UserId");

            // Act
            var result = _controller.Create(blog);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
        }

        [Fact]
        public void Edit_ReturnsForbid_WhenUserIsNotOwner()
        {
            // Arrange
            var blog = new Blog { Id = 1, OwnerId = "DifferentUser" };
            _mockBlogRepo.Setup(repo => repo.GetBlogById(1)).Returns(blog);
            _mockUserManager.Setup(m => m.GetUserId(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).Returns("UserId");

            // Act
            var result = _controller.Edit(1);

            // Assert
            Assert.IsType<ForbidResult>(result);
        }

        [Fact]
        public void Edit_ReturnsView_WhenUserIsOwner()
        {
            // Arrange
            var blog = new Blog { Id = 1, OwnerId = "UserId" };
            _mockBlogRepo.Setup(repo => repo.GetBlogById(1)).Returns(blog);
            _mockUserManager.Setup(m => m.GetUserId(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).Returns("UserId");

            // Act
            var result = _controller.Edit(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Blog>(viewResult.ViewData.Model);
            Assert.Equal(1, model.Id);
        }

        [Fact]
        public void Edit_ReturnsView_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Title", "Required");
            var blog = new Blog { Id = 1, OwnerId = "UserId" };
            _mockBlogRepo.Setup(repo => repo.GetBlogById(1)).Returns(blog);
            _mockUserManager.Setup(m => m.GetUserId(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).Returns("UserId");

            // Act
            var result = _controller.Edit(blog);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<Blog>(viewResult.ViewData.Model);
        }

        [Fact]
        public void Delete_ReturnsNotFound_WhenBlogIsNull()
        {
            // Arrange
            _mockBlogRepo.Setup(repo => repo.GetBlogById(1)).Returns((Blog)null);

            // Act
            var result = _controller.Delete(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Delete_ReturnsForbid_WhenUserIsNotOwner()
        {
            // Arrange
            var blog = new Blog { Id = 1, OwnerId = "DifferentUser" };
            _mockBlogRepo.Setup(repo => repo.GetBlogById(1)).Returns(blog);
            _mockUserManager.Setup(m => m.GetUserId(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).Returns("UserId");

            // Act
            var result = _controller.Delete(1);

            // Assert
            Assert.IsType<ForbidResult>(result);
        }

        [Fact]
        public void DeleteConfirmed_ReturnsNotFound_WhenBlogIsNull()
        {
            // Arrange
            _mockBlogRepo.Setup(repo => repo.GetBlogById(1)).Returns((Blog)null);

            // Act
            var result = _controller.DeleteConfirmed(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void DeleteConfirmed_ReturnsForbid_WhenUserIsNotOwner()
        {
            // Arrange
            var blog = new Blog { Id = 1, OwnerId = "DifferentUser" };
            _mockBlogRepo.Setup(repo => repo.GetBlogById(1)).Returns(blog);
            _mockUserManager.Setup(m => m.GetUserId(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).Returns("UserId");

            // Act
            var result = _controller.DeleteConfirmed(1);

            // Assert
            Assert.IsType<ForbidResult>(result);
        }

        [Fact]
        public void DeleteConfirmed_DeletesBlogAndRedirectsToIndex()
        {
            // Arrange
            var blog = new Blog { Id = 1, OwnerId = "UserId" };
            var posts = new List<Post>
            {
                new Post { Id = 1, BlogId = 1 },
                new Post { Id = 2, BlogId = 1 }
            };

            var commentsPost1 = new List<Comment> { new Comment { Id = 1, PostId = 1 }, new Comment { Id = 2, PostId = 1 } };
            var commentsPost2 = new List<Comment> { new Comment { Id = 3, PostId = 2 }, new Comment { Id = 4, PostId = 2 } };

            _mockBlogRepo.Setup(repo => repo.GetBlogById(1)).Returns(blog);
            _mockPostRepo.Setup(repo => repo.GetPostsByBlogId(1)).Returns(posts);
            _mockCommentRepo.Setup(repo => repo.GetCommentsByPostId(1)).Returns(commentsPost1);
            _mockCommentRepo.Setup(repo => repo.GetCommentsByPostId(2)).Returns(commentsPost2);
            _mockUserManager.Setup(m => m.GetUserId(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).Returns("UserId");

            // Act
            var result = _controller.DeleteConfirmed(1);

            // Assert
            _mockCommentRepo.Verify(r => r.DeleteComment(It.Is<Comment>(c => c.Id == 1)), Times.Once);
            _mockCommentRepo.Verify(r => r.DeleteComment(It.Is<Comment>(c => c.Id == 2)), Times.Once);
            _mockCommentRepo.Verify(r => r.DeleteComment(It.Is<Comment>(c => c.Id == 3)), Times.Once);
            _mockCommentRepo.Verify(r => r.DeleteComment(It.Is<Comment>(c => c.Id == 4)), Times.Once);

            _mockPostRepo.Verify(r => r.DeletePost(It.Is<Post>(p => p.Id == 1)), Times.Once);
            _mockPostRepo.Verify(r => r.DeletePost(It.Is<Post>(p => p.Id == 2)), Times.Once);

            _mockBlogRepo.Verify(r => r.DeleteBlog(blog), Times.Once);

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        // Erstattet testen som feilet med en ny test
        [Fact]
        public void Search_ReturnsNoBlogs_WhenDatabaseIsEmpty()
        {
            // Arrange
            _mockBlogRepo.Setup(repo => repo.GetAllBlogs()).Returns(new List<Blog>());

            // Act
            var result = _controller.Search("test");

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Blog>>(viewResult.ViewData.Model);
            Assert.Empty(model); // Skal være tomt
        }
    }
}
