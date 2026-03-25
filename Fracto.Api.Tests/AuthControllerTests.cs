using Fracto.Api.Controllers;
using Fracto.Api.Data;
using Fracto.Api.Dtos.Auth;
using Fracto.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Fracto.Api.Tests
{
    public class AuthControllerTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var context = new AppDbContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        private IOptions<JwtOptions> GetJwtOptions()
        {
            return Options.Create(new JwtOptions
            {
                Key = "SuperSecretKeyForTestingPurposeOnly1234567890",
                Issuer = "TestIssuer",
                Audience = "TestAudience",
                ExpiryMinutes = 60
            });
        }

        [Fact]
        public async Task Register_ValidUser_ReturnsOk()
        {
            // Arrange
            var dbContext = GetDbContext();
            var jwtOptions = GetJwtOptions();
            var passwordHasherMock = new Mock<IPasswordHasher<User>>();
            passwordHasherMock.Setup(x => x.HashPassword(It.IsAny<User>(), It.IsAny<string>())).Returns("hashed_pwd");

            var controller = new AuthController(dbContext, jwtOptions, passwordHasherMock.Object);

            var request = new RegisterRequest
            {
                Username = "testuser",
                Email = "test@test.com",
                FullName = "Test User",
                Password = "Password123"
            };

            // Act
            var result = await controller.Register(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var authResponse = Assert.IsType<AuthResponse>(okResult.Value);
            Assert.Equal("testuser", authResponse.Username);
            Assert.NotNull(authResponse.Token);
        }

        [Fact]
        public async Task Register_DuplicateUsername_ReturnsBadRequest()
        {
            // Arrange
            var dbContext = GetDbContext();
            dbContext.Users.Add(new User { Username = "testuser", Email = "other@test.com", FullName = "Other User", PasswordHash = "hash" });
            await dbContext.SaveChangesAsync();

            var jwtOptions = GetJwtOptions();
            var passwordHasherMock = new Mock<IPasswordHasher<User>>();
            var controller = new AuthController(dbContext, jwtOptions, passwordHasherMock.Object);

            var request = new RegisterRequest { Username = "testuser", Email = "test@test.com", FullName = "Test User", Password = "Password123" };

            // Act
            var result = await controller.Register(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Contains("Username already exists", badRequestResult.Value.ToString());
        }
    }
}
