using DotNet.Testcontainers.Configurations;
using MySql.Data.MySqlClient;

namespace IntegrationTestingBase.Containers.MySQL
{
    public class MySQLContainer(MySQLConfig config) : BaseContainer
    {
        protected override string ImageName => config.Image ?? "mysql:latest";
        protected override ushort Port => 3306;
        protected override Dictionary<string, string> EnvVariables => new()
        {
            {"MYSQL_DATABASE", config.Credentials.DbName},
            {"MYSQL_USER", config.Credentials.Username},
            {"MYSQL_PASSWORD", config.Credentials.Password},
            {"MYSQL_ROOT_PASSWORD", config.Credentials.Password}
        };

        protected override IWaitForContainerOS DefineWaitStrategy(IWaitForContainerOS strategy)
        {
            return strategy.UntilPortIsAvailable(Port);
        }
        public MySqlConnection GetClient()
        {
            var connectionString = new MySqlConnectionStringBuilder
            {
                Server = GetUrl(),
                Port = Port,
                Database = config.Credentials.DbName,
                UserID = config.Credentials.Username,
                Password = config.Credentials.Password,
                SslMode = MySqlSslMode.Disabled
            }.ToString();

            return new MySqlConnection(connectionString);
        }
    }
}
