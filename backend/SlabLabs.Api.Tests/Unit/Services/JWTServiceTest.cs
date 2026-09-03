using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Slablabs.Api.Services;
using SlabLabs.Api.Tests.Fixtures;
namespace Slablabs.Api.Tests.Unit.Services;


[TestFixture]
public class JWTServiceTests
{
    private IConfiguration? _config;
    private Mock<ILogger<JWTService>>? _mockLogger;
    private JWTService _jwtService;

    [SetUp]
    public void Setup()
    {
        _mockLogger = new Mock<ILogger<JWTService>>();

        var configDict = new Dictionary<string, string>
            {
                { "Jwt:Key", "your-super-secret-key-that-is-at-least-32-characters-long-change-in-production" },
                { "Jwt:Issuer", "YourAppName" },
                { "Jwt:Audience", "YourAppUsers" }
            };

        _config = new ConfigurationBuilder()
            .AddInMemoryCollection(configDict)
            .Build();

        _jwtService = new JWTService(_config, _mockLogger.Object);
    }

    [Test]
    public void GenerateToken_WithValidUser_ShouldReturnValidToken()
    {
        // Arrange
        var user = UserFixtures.CreateValidUser();

        // Act
        var token = _jwtService.GenerateToken(user);

        // Assert
        token.Should().NotBeNullOrEmpty();

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        jwtToken.Should().NotBeNull();
        jwtToken!.Issuer.Should().Be("YourAppName");
        jwtToken.Audiences.Should().Contain("YourAppUsers");
        jwtToken.ValidTo.Should().BeAfter(DateTime.UtcNow);
    }

    [Test]
    public void GenerateToken_ShouldContainRequiredClaims()
    {
        // Arrange
        var user = UserFixtures.CreateValidUser();

        // Act
        var token = _jwtService.GenerateToken(user);

        // Assert
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        jwtToken!.Claims.Should().Contain(c => c.Type == JwtRegisteredClaimNames.Sub && c.Value == user.Id.ToString());
        jwtToken.Claims.Should().Contain(c => c.Type == ClaimTypes.Email && c.Value == user.Email);
        jwtToken.Claims.Should().Contain(c => c.Type == ClaimTypes.Role && c.Value == user.Role.ToString());
        jwtToken.Claims.Should().Contain(c => c.Type == "firstName" && c.Value == user.FirstName);
        jwtToken.Claims.Should().Contain(c => c.Type == "lastName" && c.Value == user.LastName);
    }

    [Test]
    public void ValidateToken_WithValidToken_ShouldReturnClaimsPrincipal()
    {
        // Arrange
        var user = UserFixtures.CreateValidUser();
        var token = _jwtService.GenerateToken(user);

        // Act
        var principal = _jwtService.ValidateToken(token);

        // Assert
        principal.Should().NotBeNull();
        principal!.FindFirst(JwtRegisteredClaimNames.Sub)?.Value.Should().Be(user.Id.ToString());
        principal.FindFirst(ClaimTypes.Email)?.Value.Should().Be(user.Email);
    }

    [Test]
    public void ValidateToken_WithInvalidToken_ShouldThrowSecurityTokenException()
    {
        // Arrange
        var invalidToken = "invalid.token.here";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _jwtService.ValidateToken(invalidToken));
    }

    [Test]
    public void ValidateToken_WithExpiredToken_ShouldThrowSecurityTokenExpiredException()
    {
        // Arrange
        var expiredToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyLCJleHAiOjB9.invalid";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _jwtService.ValidateToken(expiredToken));
    }
}
