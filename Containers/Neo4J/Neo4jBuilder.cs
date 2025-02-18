using DotNet.Testcontainers.Configurations;

namespace IntegrationTestingBase.Containers.Neo4j
{
    public class Neo4jContainer(Neo4jContainerConfig config) : BaseContainer
    {
        protected override string ImageName => "neo4j:latest";
        protected override ushort Port => 7687; 

        protected override Dictionary<string, string> EnvVariables => new()
        {
            { "NEO4J_AUTH", config.Auth },
            { "NEO4J_dbms_memory_heap_initial__size", config.HeapInitialSize },
            { "NEO4J_dbms_memory_heap_max__size", config.HeapMaxSize },
            { "NEO4JLABS_PLUGINS", "[\"apoc\"]" }
        };

        protected override IWaitForContainerOS DefineWaitStrategy(IWaitForContainerOS strategy)
        {
            return strategy.UntilPortIsAvailable(Port);
        }
    }
}