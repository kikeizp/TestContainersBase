using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;

namespace IntegrationTestingBase.Containers.PGSQL
{
    public class PostgreSQLContainer(PostgreSQLConfig config) : BaseContainer
    {
        protected override string ImageName => "postgres:latest";
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
    }
}
