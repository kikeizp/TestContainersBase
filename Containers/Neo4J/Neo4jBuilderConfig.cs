namespace IntegrationTestingBase.Containers.Neo4j
{
    public record Neo4jContainerConfig(string Auth, string HeapInitialSize, string HeapMaxSize) : BaseConfig;
}