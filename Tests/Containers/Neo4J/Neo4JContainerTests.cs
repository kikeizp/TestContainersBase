using IntegrationTestingBase.Containers.Neo4j;
using Neo4j.Driver;

namespace Tests.Containers.Neo4J
{
    [CollectionDefinition("Neo4jContainer collection")]
    public class Neo4jContainerCollection : ICollectionFixture<Neo4jContainerFixture> { }

    public class Neo4jContainerFixture : IDisposable
    {
        public Neo4jContainer Container { get; private set; }

        public Neo4jContainerFixture()
        {
            Container = new Neo4jContainer(new Neo4jConfig
            {
                Credentials = new Neo4jCredentials { Username = "neo4j", Password = "password" },
                HeapInitialSize = "512m",
                HeapMaxSize = "4G"
            });
            
                Console.WriteLine("I HAVE NOT STARTED NEO4J");

                try
                {
                    Container.Start().GetAwaiter().GetResult();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"FAILED TO START NEO4J: {ex.Message}");
                    throw;
                }
        }

        public void Dispose()
        {
            Container?.Stop().GetAwaiter().GetResult();
            GC.SuppressFinalize(this);
        }
    }

    [Collection("Neo4jContainer collection")]
    public class Neo4jContainerTests(Neo4jContainerFixture fixture)
    {
        private readonly Neo4jContainerFixture Fixture = fixture;

        [Fact]
        public void ShouldReturnValidClient()
        {
            var client = Fixture.Container.GetClient();
            Assert.NotNull(client);
            Assert.IsAssignableFrom<IDriver>(client);
        }
    }
}