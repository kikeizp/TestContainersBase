namespace IntegrationTestingBase.Containers.MySQL
{
    public record MySQLCredentials {
        public required string DbName { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
    public record MySQLConfig : BaseConfig {
        public required MySQLCredentials Credentials { get; set; }
    }
}


