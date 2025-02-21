using IntegrationTestingBase.Containers.Custom;

namespace Tests.Containers.Custom
{
    [CollectionDefinition("CustomContainer collection")]
    public class CustomContainerCollection : ICollectionFixture<CustomContainerFixture> { }

    public class CustomContainerFixture : IDisposable
    {
        public CustomContainer Container { get; private set; }

        public CustomContainerFixture()
        {
            Container = new CustomContainer(new CustomContainerConfig
            {
                Image = "nginx:latest",
                Port = 80,
                Variables = new Dictionary<string, string> { { "ENV_VAR", "value" } }
            });
            Container.Start().GetAwaiter().GetResult();
        }

        public void Dispose()
        {
            Container?.Stop().GetAwaiter().GetResult();
            GC.SuppressFinalize(this);
        }
    }

    [Collection("CustomContainer collection")]
    public class CustomContainerTests(CustomContainerFixture fixture)
    {
        private readonly CustomContainerFixture Fixture = fixture;

        [Fact]
        public async Task ShouldReturnDefaultNginxPage()
        {
            var response = await new HttpClient().GetAsync($"http://localhost:{Fixture.Container.GetPort()}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Welcome to nginx!", content);
        }
    }
}