using IntegrationTestingBase.Containers.PGSQL;
using Npgsql;

namespace Tests.Containers.PGSQL
{
    [CollectionDefinition("PostgreSQLContainer collection")]
    public class PostgreSQLContainerCollection : ICollectionFixture<PostgreSQLContainerFixture> { }

    public class PostgreSQLContainerFixture : IDisposable
    {
        public PostgreSQLContainer Container { get; private set; }

        public PostgreSQLContainerFixture()
        {
            Container = new PostgreSQLContainer(new PostgreSQLConfig
            {
                Credentials = new PostgreSQLCredentials { DbName = "testdb", Username = "user", Password = "pass" }
            });
            Container.Start().GetAwaiter().GetResult();
        }

        public void Dispose()
        {
            Container?.Stop().GetAwaiter().GetResult();
            GC.SuppressFinalize(this);
        }
    }

    [Collection("PostgreSQLContainer collection")]
    public class PostgreSQLContainerTests(PostgreSQLContainerFixture fixture)
    {
        private readonly PostgreSQLContainerFixture Fixture = fixture;

        [Fact]
        public void ShouldReturnValidClient()
        {
            var client = Fixture.Container.GetClient();
            Assert.NotNull(client);
            Assert.IsType<NpgsqlConnection>(client);
        }
    }
}