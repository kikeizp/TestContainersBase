namespace IntegrationTestingBase.Containers.PGSQL
{
    public record PostgreSQLCredentials(string DbName, string Username, string Password);
    public record PostgreSQLConfig(PostgreSQLCredentials Credentials) : BaseConfig;
}

