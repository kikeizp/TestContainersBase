namespace IntegrationTestingBase.Containers.PGSQL
{
    public record PostgreSQLCredentials {
        public required string DbName { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
    public record PostgreSQLConfig : BaseConfig {
        public required PostgreSQLCredentials Credentials { get; set; }
    }
}


