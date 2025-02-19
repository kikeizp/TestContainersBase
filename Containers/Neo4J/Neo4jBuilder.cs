using DotNet.Testcontainers.Configurations;
using Neo4j.Driver;

namespace IntegrationTestingBase.Containers.Neo4j
{
    public class Neo4jContainer(Neo4jContainerConfig config) : BaseContainer
    {
        protected override string ImageName => config.Image ?? "neo4j:latest";
        protected override ushort Port => 7687; 

        protected override Dictionary<string, string> EnvVariables => new()
        {
            { "NEO4J_AUTH", $"{config.Credentials.Username}/{config.Credentials.Password}" },
            { "NEO4J_dbms_memory_heap_initial__size", config.HeapInitialSize },
            { "NEO4J_dbms_memory_heap_max__size", config.HeapMaxSize },
            { "NEO4JLABS_PLUGINS", "[\"apoc\"]" }
        };

        protected override IWaitForContainerOS DefineWaitStrategy(IWaitForContainerOS strategy)
        {
            return strategy.UntilPortIsAvailable(Port);
        }

        public IDriver GetClient()
        {
            string uri = $"bolt://{GetUrl()}:{Port}";
            return GraphDatabase.Driver(uri, AuthTokens.Basic(config.Credentials.Username, config.Credentials.Password));
        }
    }
}