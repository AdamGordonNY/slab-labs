// Services/AuthService.cs
using Slablabs.Api.Services;
using SlabLabs.Api.Data;
using Microsoft.AspNetCore.Identity;

using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Slablabs.Api.Models;
using SlabLabs.Api.Core.Interfaces;
using Slablabs.Api.Models.Enums;




namespace SlabLabs.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _dbContext;
        private readonly JWTService _jwtService;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            AppDbContext dbContext,
            JWTService jwtService,
            IPasswordHasher<ApplicationUser> passwordHasher,
            ILogger<AuthService> logger)
        {
            _dbContext = dbContext;
            _jwtService = jwtService;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            var existingUser = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (existingUser != null)
                throw new InvalidOperationException("User with this email already exists");

            var user = new ApplicationUser
            {
                Email = request.Email ?? throw new ArgumentException("Email is required", nameof(request.Email)),
                FirstName = request.FirstName ?? "",
                LastName = request.LastName ?? "",
                Status = UserStatus.Active,
                Role = UserRole.Customer,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            // Create email verification token
            var verificationToken = GenerateRandomToken();
            var emailToken = new EmailVerificationToken
            {
                UserId = user.Id,
                Token = HashToken(verificationToken),
                ExpiresAt = DateTime.UtcNow.AddHours(24),
                CreatedAt = DateTime.UtcNow
            };

            _dbContext.EmailVerificationTokens.Add(emailToken);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"User registered: {user.Email}");

            return new AuthResponse
            {
                // ✅ Use JWTService to generate token
                AccessToken = _jwtService.GenerateToken(user),
                RefreshToken = "",
                User = MapUserToDto(user)
            };
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
                throw new UnauthorizedAccessException("Invalid email or password");

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
            if (result == PasswordVerificationResult.Failed)
                throw new UnauthorizedAccessException("Invalid email or password");

            if (user.EmailVerifiedAt == null)
                throw new UnauthorizedAccessException("Please verify your email before logging in");

            user.LastLoginAt = DateTime.UtcNow;
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();

            var accessToken = _jwtService.GenerateToken(user);  // ✅ Use JWTService
            var refreshToken = GenerateRefreshToken(user);

            return new AuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                User = MapUserToDto(user)
            };
        }

        public async Task<AuthResponse> RefreshTokenAsync(string refreshToken)
        {
            var token = await _dbContext.RefreshTokens
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Token == refreshToken && !t.IsRevoked && !t.IsExpired);

            if (token == null)
                throw new UnauthorizedAccessException("Invalid or expired refresh token");

            var user = token.User;

            token.RevokedAt = DateTime.UtcNow;
            var newRefreshToken = GenerateRefreshToken(user);

            _dbContext.RefreshTokens.Update(token);
            _dbContext.Set<RefreshToken>().Add(newRefreshToken);
            await _dbContext.SaveChangesAsync();

            return new AuthResponse
            {

                AccessToken = _jwtService.GenerateToken(user),
                RefreshToken = newRefreshToken.Token,
                User = MapUserToDto(user)
            };
        }

        public async Task RevokeTokenAsync(string refreshToken)
        {
            var token = await _dbContext.RefreshTokens
                .FirstOrDefaultAsync(t => t.Token == refreshToken);

            if (token != null)
            {
                token.RevokedAt = DateTime.UtcNow;
                _dbContext.RefreshTokens.Update(token);
                await _dbContext.SaveChangesAsync();
            }
        }

        private RefreshToken GenerateRefreshToken(ApplicationUser user)
        {
            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var token = Convert.ToBase64String(tokenBytes);

            return new RefreshToken
            {
                UserId = user.Id,
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow
            };
        }
        private string GenerateRandomToken()
        {
            var tokenBytes = RandomNumberGenerator.GetBytes(32);
            return Convert.ToBase64String(tokenBytes);
        }

        private string HashToken(string token)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(token));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        private UserDto MapUserToDto(ApplicationUser user)
        {
            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role.ToString(),
                EmailVerified = user.EmailVerifiedAt.HasValue,
                Status = user.Status.ToString()
            };
        }
    }
}
