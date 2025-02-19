namespace IntegrationTestingBase.Containers.Neo4j
{
    public record Neo4jCredentials
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
    public record Neo4jConfig : BaseConfig
    {
        public required Neo4jCredentials Credentials { get; set; }
        public required string HeapInitialSize { get; set; }
        public required string HeapMaxSize { get; set; }
    }
}