// Core/Interfaces/IAuthService.cs
namespace SlabLabs.Api.Core.Interfaces
{
    public interface IAuthService
    {
        /// <summary>
        /// Registers a new user.
        /// </summary>
        Task<AuthResponse> RegisterAsync(RegisterRequest request);

        /// <summary>
        /// Authenticates a user and returns tokens.
        /// </summary>
        Task<AuthResponse> LoginAsync(LoginRequest request);

        /// <summary>
        /// Refreshes an expired access token using a refresh token.
        /// </summary>
        Task<AuthResponse> RefreshTokenAsync(string refreshToken);

        /// <summary>
        /// Revokes a refresh token (logout).
        /// </summary>
        Task RevokeTokenAsync(string refreshToken);
    }
}
