using BloggersUnite.Controllers;
using BloggersUnite.Data.Repositories.Interfaces;
using BloggersUnite.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace BloggersUnite.Tests.Controllers
{
    public class HomeControllerTest
    {
        private readonly Mock<IBlogRepository> _mockBlogRepo;
        private readonly Mock<UserManager<IdentityUser>> _mockUserManager;
        private readonly HomeController _controller;

        public HomeControllerTest()
        {
            _mockBlogRepo = new Mock<IBlogRepository>();

            // Mocking UserManager for testing 
            var store = new Mock<IUserStore<IdentityUser>>();
            _mockUserManager = new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);

            _controller = new HomeController(_mockBlogRepo.Object, _mockUserManager.Object);

            // Mocking user identity
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, "testuser") };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var user = new ClaimsPrincipal(identity);
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }


        //returnerer visningen "Welcome" dersom brukeren ikke er autentisert (ikke innlogget).
        [Fact]
        public void Index_ReturnsWelcomeView_WhenUserNotAuthenticated()
        {
            // Arrange
            _controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity());

            // Act
            var result = _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Welcome", viewResult.ViewName);
        }

        //Sjekker at Index-metoden returnerer en liste med blogger hvis en bruker er autentisert.
        [Fact]
        public void Index_ReturnsView_WithListOfBlogs_WhenUserAuthenticated()
        {
            // Arrange
            var blogs = new List<Blog> { new Blog { Id = 1, Title = "Blog1" }, new Blog { Id = 2, Title = "Blog2" } };
            _mockBlogRepo.Setup(repo => repo.GetAllBlogs()).Returns(blogs);

            // Act
            var result = _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Blog>>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Count());
        }

        //Sjekker at Index-metoden returnerer en tom liste hvis ingen blogger finnes i databasen.
        [Fact]
        public void Index_ReturnsEmptyList_WhenNoBlogsExist()
        {
            // Arrange
            var blogs = new List<Blog>(); // Empty list
            _mockBlogRepo.Setup(repo => repo.GetAllBlogs()).Returns(blogs);

            // Act
            var result = _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Blog>>(viewResult.ViewData.Model);
            Assert.Empty(model);
        }
    }
}
