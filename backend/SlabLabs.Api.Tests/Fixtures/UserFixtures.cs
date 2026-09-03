// Tests/Fixtures/UserFixtures.cs

using Slablabs.Api.Models;
using Slablabs.Api.Models.Enums;

namespace SlabLabs.Api.Tests.Fixtures
{
    public static class UserFixtures
    {
        public static ApplicationUser CreateValidUser(
            string email = "test@example.com",
            string firstName = "John",
            string lastName = "Doe")
        {
            return new ApplicationUser
            {
                Id = 1,
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                PasswordHash = "hashed_password_123",
                Role = UserRole.Customer,
                Status = UserStatus.Active,
                EmailVerifiedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }

        public static ApplicationUser CreateUnverifiedUser()
        {
            var user = CreateValidUser();
            user.EmailVerifiedAt = null;
            return user;
        }

        public static ApplicationUser CreateInactiveUser()
        {
            var user = CreateValidUser();
            user.Status = UserStatus.Inactive;
            return user;
        }
    }
}
