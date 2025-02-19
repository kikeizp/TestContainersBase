using IntegrationTestingBase.Containers.API;
using IntegrationTestingBase.Containers.AWS;
using IntegrationTestingBase.Containers.Custom;
using IntegrationTestingBase.Containers.MySQL;
using IntegrationTestingBase.Containers.Neo4j;
using IntegrationTestingBase.Containers.PGSQL;
using IntegrationTestingBase.Core.Interfaces;

namespace IntegrationTestingBase.Containers.Registry
{
    public sealed class TestContainerRegistry : IContainerRegistry
    {
        private static readonly TestContainerRegistry Instance = new();
        public static TestContainerRegistry GetInstance => Instance;

        private readonly Dictionary<string, BaseContainer> Containers = [];

        public async Task InitializeContainers(Dictionary<string, BaseConfig> configs)
        {
            Console.WriteLine("🔄 Initializing required containers...");

            foreach (var (name, config) in configs)
            {
                if (IsContainerRegistered(name))
                {
                    Console.WriteLine($"✅ Container named '{name}' is already registered.");
                    continue;
                }

                BaseContainer container = config switch
                {
                    AwsConfig awsConfig => new AwsContainer(awsConfig),
                    MockApiConfig mockApiConfig => new MockApiContainer(mockApiConfig),
                    Neo4jConfig neo4jConfig => new Neo4jContainer(neo4jConfig),
                    PostgreSQLConfig postgreConfig => new PostgreSQLContainer(postgreConfig),
                    MySQLConfig mySqlConfig => new MySQLContainer(mySqlConfig),
                    CustomContainerConfig customConfig => new CustomContainer(customConfig),
                    _ => throw new Exception($"Unknown config type: {config.GetType()}")
                };

                await container.Start();
                Console.WriteLine($"✅ Container named '{name}' is running successfully on URL: {container.GetUrl()}");

                AddContainer(name, container);
            }

            Console.WriteLine("✅ All containers initialized.");
        }

        private void AddContainer(string name, BaseContainer container)
        {
            Containers[name] = container;
        }

        public async Task StartContainer(string name)
        {
            if (Containers.TryGetValue(name, out var container))
            {
                await container.Start();
            }
            else
            {
                throw new KeyNotFoundException($"Container with name '{name}' not found.");
            }
        }

        public async Task StopContainer(string name)
        {
            if (Containers.TryGetValue(name, out var container))
            {
                await container.Stop();
            }
            else
            {
                throw new KeyNotFoundException($"Container with name '{name}' not found.");
            }
        }

        public T GetContainer<T>(string name) where T : class
        {
            return Containers.TryGetValue(name, out var container) && container is T result
                ? result
                : throw new InvalidOperationException($"Container '{name}' is not of type {typeof(T).Name}");
        }

        public async Task StopContainers()
        {
            foreach (var (_, container) in Containers)
            {
                await container.Stop();
            }
        }

        public Dictionary<string, BaseContainer> GetContainers() => Containers;

        public bool IsContainerRegistered(string name) => Containers.ContainsKey(name);
    }
}
