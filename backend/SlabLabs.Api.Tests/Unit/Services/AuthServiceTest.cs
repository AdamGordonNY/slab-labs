
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

using NUnit.Framework;
using SlabLabs.Api.Services;
using SlabLabs.Api.Data;
using Microsoft.EntityFrameworkCore;

using SlabLabs.Api.Tests.Fixtures;
using Moq;
using Slablabs.Api.Services;
using Slablabs.Api.Models;
using Microsoft.Extensions.Configuration;
using System.Linq.Expressions;

namespace SlabLabs.Api.Tests.Unit.Services
{
    [TestFixture]
    public class AuthServiceTests
    {
        private readonly Mock<AppDbContext> _mockDbContext;
        private readonly Mock<JWTService> _mockJwtService;
        private readonly Mock<IPasswordHasher<ApplicationUser>> _mockPasswordHasher;
        private readonly Mock<ILogger<AuthService>> _mockLogger;
        private readonly AuthService _authService;

        [Test]
        public AuthServiceTests()
        {
            _mockDbContext = new Mock<AppDbContext>();
            _mockJwtService = new Mock<JWTService>(new Mock<IConfiguration>().Object, new Mock<ILogger<JWTService>>().Object);
            _mockPasswordHasher = new Mock<IPasswordHasher<ApplicationUser>>();
            _mockLogger = new Mock<ILogger<AuthService>>();

            _authService = new AuthService(
                _mockDbContext.Object,
                _mockJwtService.Object,
                _mockPasswordHasher.Object,
                _mockLogger.Object
            );
        }

        #region RegisterAsync Tests

        [Fact]
        public async Task RegisterAsync_WithValidRequest_ShouldCreateUserAndReturnAuthResponse()
        {
            // Arrange
            var request = TokenFixtures.CreateValidRegisterRequest();
            var dbContextMock = new Mock<AppDbContext>();
            var userDbSetMock = new Mock<DbSet<User>>();

            userDbSetMock.Setup(x => x.FirstOrDefaultAsync(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync((ApplicationUser)null!);

            dbContextMock.Setup(x => x.Users).Returns(userDbSetMock.Object);
            dbContextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            _mockPasswordHasher
                .Setup(x => x.HashPassword(It.IsAny<User>(), request.Password))
                .Returns("hashed_password");

            _mockJwtService
                .Setup(x => x.GenerateToken(It.IsAny<User>()))
                .Returns("jwt_token_123");

            // Act
            var result = await _authService.RegisterAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.AccessToken.Should().Be("jwt_token_123");
            result.User.Should().NotBeNull();
            result.User.Email.Should().Be(request.Email);
            result.User.FirstName.Should().Be(request.FirstName);
        }

        [Fact]
        public async Task RegisterAsync_WithDuplicateEmail_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var request = TokenFixtures.CreateValidRegisterRequest();
            var existingUser = UserFixtures.CreateValidUser(email: request.Email);

            var dbContextMock = new Mock<AppDbContext>();
            var userDbSetMock = new Mock<DbSet<User>>();

            userDbSetMock.Setup(x => x.FirstOrDefaultAsync(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingUser);

            dbContextMock.Setup(x => x.Users).Returns(userDbSetMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _authService.RegisterAsync(request)
            );
        }

        #endregion

        #region LoginAsync Tests

        [Fact]
        public async Task LoginAsync_WithValidCredentials_ShouldReturnAuthResponse()
        {
            // Arrange
            var request = TokenFixtures.CreateValidLoginRequest();
            var user = UserFixtures.CreateValidUser(email: request.Email);

            var dbContextMock = new Mock<AppDbContext>();
            var userDbSetMock = new Mock<DbSet<User>>();

            userDbSetMock.Setup(x => x.FirstOrDefaultAsync(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            dbContextMock.Setup(x => x.Users).Returns(userDbSetMock.Object);
            dbContextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            _mockPasswordHasher
                .Setup(x => x.VerifyHashedPassword(user, user.PasswordHash, request.Password))
                .Returns(PasswordVerificationResult.Success);

            _mockJwtService
                .Setup(x => x.GenerateToken(user))
                .Returns("jwt_token_123");

            // Act
            var result = await _authService.LoginAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.AccessToken.Should().Be("jwt_token_123");
            result.User.Email.Should().Be(request.Email);
        }

        [Fact]
        public async Task LoginAsync_WithInvalidPassword_ShouldThrowUnauthorizedAccessException()
        {
            // Arrange
            var request = TokenFixtures.CreateValidLoginRequest();
            var user = UserFixtures.CreateValidUser(email: request.Email);

            var dbContextMock = new Mock<AppDbContext>();
            var userDbSetMock = new Mock<DbSet<User>>();

            userDbSetMock.Setup(x => x.FirstOrDefaultAsync(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            dbContextMock.Setup(x => x.Users).Returns(userDbSetMock.Object);

            _mockPasswordHasher
                .Setup(x => x.VerifyHashedPassword(user, user.PasswordHash, request.Password))
                .Returns(PasswordVerificationResult.Failed);

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(
                () => _authService.LoginAsync(request)
            );
        }

        [Fact]
        public async Task LoginAsync_WithUnverifiedEmail_ShouldThrowUnauthorizedAccessException()
        {
            // Arrange
            var request = TokenFixtures.CreateValidLoginRequest();
            var user = UserFixtures.CreateUnverifiedUser();

            var dbContextMock = new Mock<AppDbContext>();
            var userDbSetMock = new Mock<DbSet<User>>();

            userDbSetMock.Setup(x => x.FirstOrDefaultAsync(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            dbContextMock.Setup(x => x.Users).Returns(userDbSetMock.Object);

            _mockPasswordHasher
                .Setup(x => x.VerifyHashedPassword(user, user.PasswordHash, request.Password))
                .Returns(PasswordVerificationResult.Success);

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(
                () => _authService.LoginAsync(request)
            );
        }

        #endregion

        #region RefreshTokenAsync Tests

        [Fact]
        public async Task RefreshTokenAsync_WithValidToken_ShouldReturnNewAuthResponse()
        {
            // Arrange
            var refreshToken = "valid_refresh_token";
            var user = UserFixtures.CreateValidUser();
            var token = new RefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Token = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow,
                User = user
            };

            var dbContextMock = new Mock<AppDbContext>();
            var refreshTokenDbSetMock = new Mock<DbSet<RefreshToken>>();

            refreshTokenDbSetMock.Setup(x => x.FirstOrDefaultAsync(
                It.IsAny<Expression<Func<RefreshToken, bool>>>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(token);

            dbContextMock.Setup(x => x.RefreshTokens).Returns(refreshTokenDbSetMock.Object);
            dbContextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(2);

            _mockJwtService
                .Setup(x => x.GenerateToken(user))
                .Returns("new_jwt_token");

            // Act
            var result = await _authService.RefreshTokenAsync(refreshToken);

            // Assert
            result.Should().NotBeNull();
            result.AccessToken.Should().Be("new_jwt_token");
            result.User.Should().NotBeNull();
        }

        [Fact]
        public async Task RefreshTokenAsync_WithExpiredToken_ShouldThrowUnauthorizedAccessException()
        {
            // Arrange
            var refreshToken = "expired_refresh_token";

            var dbContextMock = new Mock<AppDbContext>();
            var refreshTokenDbSetMock = new Mock<DbSet<RefreshToken>>();

            refreshTokenDbSetMock.Setup(x => x.FirstOrDefaultAsync(
                It.IsAny<Expression<Func<RefreshToken, bool>>>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync((RefreshToken)null!);

            dbContextMock.Setup(x => x.RefreshTokens).Returns(refreshTokenDbSetMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(
                () => _authService.RefreshTokenAsync(refreshToken)
            );
        }

        #endregion
    }
}
