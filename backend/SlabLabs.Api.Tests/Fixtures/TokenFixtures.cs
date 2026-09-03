// Tests/Fixtures/TokenFixtures.cs


namespace SlabLabs.Api.Tests.Fixtures
{
    public static class TokenFixtures
    {
        public static RegisterRequest CreateValidRegisterRequest(
            string email = "newuser@example.com",
            string password = "Password@123")
        {
            return new RegisterRequest
            {
                Email = email,
                Password = password,
                FirstName = "Jane",
                LastName = "Smith"
            };
        }

        public static LoginRequest CreateValidLoginRequest(
            string email = "test@example.com",
            string password = "Password@123")
        {
            return new LoginRequest
            {
                Email = email,
                Password = password
            };
        }
    }
}
