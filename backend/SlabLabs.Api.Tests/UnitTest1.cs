using NUnit.Framework;



namespace SlabLabs.Api.Tests
{
    [TestFixture]
    public class HealthCheckTests
    {
        [Test]
        public void HealthCheck_ShouldPass()
        {
            Assert.That(true, Is.True);
        }
    }
}
