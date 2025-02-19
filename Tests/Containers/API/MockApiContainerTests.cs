using System.Text.Json;
using IntegrationTestingBase.Containers.API;

namespace Tests.Containers.API
{
    public class MockApiContainerTests : IAsyncLifetime
    {
        private readonly MockApiContainer MockApiContainer;
        private readonly HttpClient Client = new();

        private readonly MockApiConfig Config = new()
        {
            Image = "wiremock/wiremock:latest",
            Mappings = [
                    new Mapping
                    {
                        Request = new Request
                        {
                            Method = "GET",
                            UrlPattern = "/"
                        },
                        Response = new Response
                        {
                            Status = 200,
                            Body = "{ \"message\": \"SUCCESS\" }",
                            Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
                        }
                    },
                    new Mapping
                    {
                        Request = new Request
                        {
                            Method = "POST",
                            UrlPattern = "/"
                        },
                        Response = new Response
                        {
                            Status = 200,
                            Body = "{ \"message\": \"SUCCESS\" }",
                            Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
                        }
                    }
                ]
        };

        public MockApiContainerTests()
        {
            MockApiContainer = new(Config);
        }

        public async Task InitializeAsync()
        {
            await MockApiContainer.Start();
        }

        public async Task DisposeAsync()
        {
            await MockApiContainer.Stop();
        }

        [Fact]
        public async Task ShouldRetrieveSuccessWhenGetTest()
        {
            // Arrange
            HttpResponseMessage response = await Client.GetAsync(MockApiContainer.GetUrl());
            response.EnsureSuccessStatusCode();

            // Assert
            var responseBody = JsonSerializer.Deserialize<Dictionary<string, object>>(await response.Content.ReadAsStringAsync());

            // Assert
            Assert.NotNull(responseBody);
            Assert.True(responseBody.ContainsKey("message"));
            Assert.Equal("SUCCESS", responseBody["message"].ToString());
        }


        [Fact]
        public async Task ShouldCreate()
        {
            // Arrange
            HttpResponseMessage response = await Client.GetAsync(MockApiContainer.GetUrl());
            response.EnsureSuccessStatusCode();

            // Assert
            var responseBody = JsonSerializer.Deserialize<Dictionary<string, object>>(await response.Content.ReadAsStringAsync());

            // Assert
            Assert.NotNull(responseBody);
            Assert.True(responseBody.ContainsKey("message"));
            Assert.Equal("SUCCESS", responseBody["message"].ToString());

        }

    }

}
