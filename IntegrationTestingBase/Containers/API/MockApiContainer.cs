using System.Text.Json;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;

namespace IntegrationTestingBase.Containers.API
{

    public class MockApiContainer(MockApiConfig config) : BaseContainer
    {
        protected override string ImageName => config.Image ?? "wiremock/wiremock:latest";
        protected override ushort Port => 8080;

        private static Mapping DefaultMapping => new ()
        {
            Request = new Request 
            {
                Method = "GET",
                Url = "/"
            },
            Response = new Response
            {
                Status = 200,
                Body = "{ \"message\": \"SUCCESS\" }",
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            }
        };

        private string MappingPath => SaveMappingsToFile([..config.Mappings, DefaultMapping ]);
        private static string ContainerMappingPath => "/home/wiremock/mappings/mapping.json";

        protected override IWaitForContainerOS DefineWaitStrategy(IWaitForContainerOS strategy)
        {
            return strategy.UntilHttpRequestIsSucceeded(r => r.ForPort(Port));
        }

        private static string SaveMappingsToFile(List<Mapping> mappings)
        {
            string tempFilePath = Path.Combine(Path.GetTempPath(), $"wiremock-mappings-{Guid.NewGuid()}.json");

            string jsonContent = JsonSerializer.Serialize(new MockApiConfig {
                Mappings = mappings
            });

            File.WriteAllText(tempFilePath, jsonContent);

            return tempFilePath; 
        }

        protected override ContainerBuilder CustomizeContainer(ContainerBuilder container)
        {
            Console.Write($"Path is: {MappingPath}");
            return container
            .WithBindMount(MappingPath, ContainerMappingPath);
        }
    }
}
