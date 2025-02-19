using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using Npgsql;


namespace IntegrationTestingBase.Containers.PGSQL
{
    public class PostgreSQLContainer(PostgreSQLConfig config) : BaseContainer
    {
        protected override string ImageName => config.Image ?? "postgres:latest";
        protected override ushort Port => 5432;
        protected override Dictionary<string, string> EnvVariables => new ()
        {
            {"POSTGRES_DB", config.Credentials.DbName},
            {"POSTGRES_USER", config.Credentials.Username},
            {"POSTGRES_PASSWORD", config.Credentials.Password},
        };

        protected override IWaitForContainerOS DefineWaitStrategy(IWaitForContainerOS strategy)
        {
            return strategy.UntilPortIsAvailable(Port);
        }
        public NpgsqlConnection GetClient()
        {
            var connectionString = new NpgsqlConnectionStringBuilder
            {
                Host = GetUrl(),
                Port = Port,
                Database = config.Credentials.DbName,
                Username = config.Credentials.Username,
                Password = config.Credentials.Password,
                SslMode = SslMode.Disable
            }.ToString();

            return new NpgsqlConnection(connectionString);
        }
    }
}
