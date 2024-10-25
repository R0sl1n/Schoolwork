using BloggersUnite.Controllers;
using BloggersUnite.Data.Repositories;
using BloggersUnite.Data.Repositories.Interfaces;
using BloggersUnite.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace BloggersUnite.Tests.Controllers
{
    public class CommentControllerTest
    {
        private readonly Mock<ICommentRepository> _mockCommentRepo;
        private readonly Mock<IPostRepository> _mockPostRepo;
        private readonly Mock<UserManager<IdentityUser>> _mockUserManager;
        private readonly CommentController _controller;

        public CommentControllerTest()
        {
            _mockCommentRepo = new Mock<ICommentRepository>();
            _mockPostRepo = new Mock<IPostRepository>();
            _mockUserManager = MockUserManager();

            _controller = new CommentController(
                _mockCommentRepo.Object,
                _mockPostRepo.Object,
                _mockUserManager.Object
            );
        }

        private Mock<UserManager<IdentityUser>> MockUserManager()
        {
            var store = new Mock<IUserStore<IdentityUser>>();
            return new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
        }

        // 1. Create metode test
        [Fact]
        public void Create_Get_ReturnsView_WithComment()
        {
            var post = new Post { Id = 1 };
            _mockPostRepo.Setup(repo => repo.GetPostById(1)).Returns(post);

            var result = _controller.Create(1);

            var viewResult = result as ViewResult;
            Assert.NotNull(viewResult);
            var model = viewResult.ViewData.Model as Comment;
            Assert.NotNull(model);
            Assert.Equal(1, model.PostId);
        }

        // 2. Test når post ikke finnes
        [Fact]
        public void Create_Get_ReturnsNotFound_WhenPostIsNull()
        {
            _mockPostRepo.Setup(repo => repo.GetPostById(1)).Returns((Post)null);

            var result = _controller.Create(1);

            Assert.Null(result as ViewResult);
        }

        // 3. Oppretting av kommentar når ModelState er ugyldig
        [Fact]
        public void Create_Post_ReturnsViewResult_WhenModelStateIsInvalid()
        {
            _controller.ModelState.AddModelError("Content", "Required");

            var result = _controller.Create(new Comment { PostId = 1 });

            var viewResult = result as ViewResult;
            Assert.NotNull(viewResult);
            Assert.IsAssignableFrom<Comment>(viewResult.ViewData.Model);
        }

        // 4. Redirect når kommentaren opprettes
        [Fact]
        public void Create_Post_RedirectsToPostDetails_WhenCommentIsCreated()
        {
            var post = new Post { Id = 1 };
            _mockPostRepo.Setup(repo => repo.GetPostById(1)).Returns(post);
            _mockUserManager.Setup(m => m.GetUserId(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).Returns("UserId");

            var result = _controller.Create(new Comment { PostId = 1, Content = "Test Comment" });

            var redirectResult = result as RedirectToActionResult;
            Assert.NotNull(redirectResult);
            Assert.Equal("Details", redirectResult.ActionName);
        }

        // 5. Oppdatering av kommentar
        [Fact]
        public void Create_Post_AddsComment_AndRedirects()
        {
            var comment = new Comment { PostId = 1, Content = "Test Comment" };
            var post = new Post { Id = 1 };
            _mockPostRepo.Setup(repo => repo.GetPostById(1)).Returns(post);
            _mockUserManager.Setup(m => m.GetUserId(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).Returns("UserId");

            var result = _controller.Create(comment);

            _mockCommentRepo.Verify(repo => repo.CreateComment(comment), Times.Once);
            var redirectResult = result as RedirectToActionResult;
            Assert.NotNull(redirectResult);
            Assert.Equal("Details", redirectResult.ActionName);
        }

        // 6. Sletting av kommentar og redirect
        [Fact]
        public void DeleteConfirmed_RemovesCommentAndRedirectsToPostDetails()
        {
            var comment = new Comment { Id = 1, PostId = 1, OwnerId = "UserId" };
            _mockCommentRepo.Setup(repo => repo.GetCommentById(1)).Returns(comment);
            _mockUserManager.Setup(m => m.GetUserId(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).Returns("UserId");

            var result = _controller.DeleteConfirmed(1);

            _mockCommentRepo.Verify(r => r.DeleteComment(comment), Times.Once);
            var redirectResult = result as RedirectToActionResult;
            Assert.NotNull(redirectResult);
        }

        // 7. Enkel slettingstest
        [Fact]
        public void Delete_Post_RemovesCommentAndSaves()
        {
            var comment = new Comment { Id = 1, PostId = 1, OwnerId = "UserId" };
            _mockCommentRepo.Setup(repo => repo.GetCommentById(1)).Returns(comment);
            _mockUserManager.Setup(m => m.GetUserId(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).Returns("UserId");

            var result = _controller.DeleteConfirmed(1);

            _mockCommentRepo.Verify(repo => repo.DeleteComment(comment), Times.Once);
            _mockCommentRepo.Verify(repo => repo.Save(), Times.Once);
        }

        // 8. Oppretting når post ikke finnes
        [Fact]
        public void Create_Post_ReturnsError_WhenPostIsNull()
        {
            var comment = new Comment { PostId = 1, Content = "Test Comment" };
            _mockPostRepo.Setup(repo => repo.GetPostById(1)).Returns((Post)null);
            _mockUserManager.Setup(m => m.GetUserId(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).Returns("UserId");

            var result = _controller.Create(comment);

            var viewResult = result as ViewResult;
            Assert.Null(viewResult);
            Assert.IsType<NotFoundResult>(result);
        }

        

        // 9. Sletting av kommentar som ikke finnes
        [Fact]
        public void DeleteConfirmed_ReturnsNotFound_WhenCommentDoesNotExist()
        {
            _mockCommentRepo.Setup(repo => repo.GetCommentById(1)).Returns((Comment)null);

            var result = _controller.DeleteConfirmed(1);

            Assert.IsType<NotFoundResult>(result);
        }

        // 10. Enkel test for sletting av kommentar når bruker ikke er eier
        [Fact]
        public void Delete_ReturnsForbid_WhenUserIsNotOwner()
        {
            var comment = new Comment { Id = 1, PostId = 1, OwnerId = "AnotherUser" };
            _mockCommentRepo.Setup(repo => repo.GetCommentById(1)).Returns(comment);
            _mockUserManager.Setup(m => m.GetUserId(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).Returns("UserId");

            var result = _controller.DeleteConfirmed(1);

            Assert.IsType<ForbidResult>(result);
        }

       
        // 11. Test for opprettelse av kommentar når ModelState er gyldig
        [Fact]
        public void Create_Post_Success_WhenModelStateIsValid()
        {
            // Arrange
            var post = new Post { Id = 1 };
            var comment = new Comment { PostId = 1, Content = "New Comment" };
            _mockPostRepo.Setup(repo => repo.GetPostById(1)).Returns(post);
            _mockUserManager.Setup(m => m.GetUserId(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).Returns("UserId");

            // Act
            var result = _controller.Create(comment);

            // Assert
            _mockCommentRepo.Verify(repo => repo.CreateComment(comment), Times.Once);
            Assert.IsType<RedirectToActionResult>(result);
        }

        // 12. Test for å håndtere ugyldig ModelState under oppretting av kommentar
        [Fact]
        public void Create_Post_ReturnsView_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Content", "Content is required");
            var comment = new Comment { PostId = 1 };

            // Act
            var result = _controller.Create(comment);

            // Assert
            var viewResult = result as ViewResult;
            Assert.NotNull(viewResult);
            Assert.IsAssignableFrom<Comment>(viewResult.Model);
        }

        // 13. Test for sletting når bruker er eier av kommentaren
        [Fact]
        public void DeleteConfirmed_RemovesComment_WhenUserIsOwner()
        {
            // Arrange
            var comment = new Comment { Id = 1, PostId = 1, OwnerId = "UserId" };
            _mockCommentRepo.Setup(repo => repo.GetCommentById(1)).Returns(comment);
            _mockUserManager.Setup(m => m.GetUserId(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).Returns("UserId");

            // Act
            var result = _controller.DeleteConfirmed(1);

            // Assert
            _mockCommentRepo.Verify(repo => repo.DeleteComment(comment), Times.Once);
            Assert.IsType<RedirectToActionResult>(result);
        }

        // 14. Test for oppretting av kommentar uten post
        [Fact]
        public void Create_Post_ReturnsError_WhenPostDoesNotExist()
        {
            // Arrange
            var comment = new Comment { PostId = 1, Content = "Test Comment" };
            _mockPostRepo.Setup(repo => repo.GetPostById(1)).Returns((Post)null);

            // Act
            var result = _controller.Create(comment);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }


        // 15. Test for å oppdatere kommentarens innhold
        [Fact]
        public void Edit_Post_UpdatesContent_WhenModelStateIsValid()
        {
            // Arrange
            var existingComment = new Comment { Id = 1, PostId = 1, Content = "Old Content", OwnerId = "UserId" };
            _mockCommentRepo.Setup(repo => repo.GetCommentByIdAsNoTracking(1)).Returns(existingComment);
            _mockUserManager.Setup(m => m.GetUserId(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).Returns("UserId");

            var updatedComment = new Comment { Id = 1, PostId = 1, Content = "Updated Content" };

            // Act
            var result = _controller.Edit(updatedComment);

            // Assert
            _mockCommentRepo.Verify(repo => repo.UpdateComment(It.Is<Comment>(c => c.Content == "Updated Content")), Times.Once);
            Assert.IsType<RedirectToActionResult>(result);
        }



    }
}
