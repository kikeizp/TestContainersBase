using IntegrationTestingBase.Containers.MySQL;
using MySql.Data.MySqlClient;

namespace Tests.Containers.MySQL
{
    [CollectionDefinition("MySQLContainer collection")]
    public class MySQLContainerCollection : ICollectionFixture<MySQLContainerFixture> { }

    public class MySQLContainerFixture : IDisposable
    {
        public MySQLContainer Container { get; private set; }

        public MySQLContainerFixture()
        {
            Container = new MySQLContainer(new MySQLConfig
            {
                Credentials = new MySQLCredentials { DbName = "testdb", Username = "user", Password = "pass" }
            });
            Container.Start().GetAwaiter().GetResult();
        }

        public void Dispose()
        {
            Container?.Stop().GetAwaiter().GetResult();
            GC.SuppressFinalize(this);
        }
    }

    [Collection("MySQLContainer collection")]
    public class MySQLContainerTests(MySQLContainerFixture fixture)
    {
        private readonly MySQLContainerFixture Fixture = fixture;

        [Fact]
        public void ShouldReturnValidClient()
        {
            var client = Fixture.Container.GetClient();
            Assert.NotNull(client);
            Assert.IsType<MySqlConnection>(client);
        }
    }

}