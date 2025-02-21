using System.Text.Json;
using IntegrationTestingBase.Containers.API;
using IntegrationTestingBase.Containers;

namespace Tests.Containers.API
{
    [CollectionDefinition("Wiremock collection")]
    public class ContainerCollection : ICollectionFixture<WiremockLifecycleFixture>{}
    public class WiremockLifecycleFixture : IDisposable
    {
        public BaseContainer Container { get; private set; }

        public WiremockLifecycleFixture()
        {
            Container = new MockApiContainer(new MockApiConfig
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
                            Status = 201,
                            Body = "{ \"message\": \"CREATED\" }",
                            Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
                        }
                    },
                    new Mapping
                    {
                        Request = new Request
                        {
                            Method = "DELETE",
                            UrlPattern = "/delete"
                        },
                        Response = new Response
                        {
                            Status = 204,
                            Body = "",
                            Headers = []
                        }
                    },
                    new Mapping
                    {
                        Request = new Request
                        {
                            Method = "PUT",
                            UrlPattern = "/update"
                        },
                        Response = new Response
                        {
                            Status = 200,
                            Body = "{ \"message\": \"UPDATED\" }",
                            Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
                        }
                    }
                ]
            });
            Container.Start().GetAwaiter().GetResult();
        }

        public void Dispose()
        {
            Container?.Stop().GetAwaiter().GetResult();
            GC.SuppressFinalize(this);
        }
    }

    [Collection("Wiremock collection")]
    public class MockApiContainerTests(WiremockLifecycleFixture fixture)
    {
        private readonly WiremockLifecycleFixture Fixture = fixture;
        private readonly HttpClient Client = new();

        [Fact]
        public async Task ShouldRetrieveSuccessWhenGetTest()
        {
            HttpResponseMessage response = await Client.GetAsync(Fixture.Container.GetUrl());
            response.EnsureSuccessStatusCode();

            var responseBody = JsonSerializer.Deserialize<Dictionary<string, object>>(await response.Content.ReadAsStringAsync());

            Assert.NotNull(responseBody);
            Assert.True(responseBody.ContainsKey("message"));
            Assert.Equal("SUCCESS", responseBody["message"].ToString());
        }

        [Fact]
        public async Task ShouldCreateResourceWhenPostTest()
        {
            var content = new StringContent("{}", System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await Client.PostAsync(Fixture.Container.GetUrl(), content);

            Assert.Equal(201, (int)response.StatusCode);

            var responseBody = JsonSerializer.Deserialize<Dictionary<string, object>>(await response.Content.ReadAsStringAsync());

            Assert.NotNull(responseBody);
            Assert.Equal("CREATED", responseBody["message"].ToString());
        }

        [Fact]
        public async Task ShouldReturnNotFoundWhenInvalidEndpoint()
        {
            HttpResponseMessage response = await Client.GetAsync($"{Fixture.Container.GetUrl()}/invalid");

            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task ShouldReturnUpdatedWhenPutTest()
        {
            var content = new StringContent("{ \"data\": \"update\" }", System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await Client.PutAsync($"{Fixture.Container.GetUrl()}/update", content);

            Assert.Equal(200, (int)response.StatusCode);

            var responseBody = JsonSerializer.Deserialize<Dictionary<string, object>>(await response.Content.ReadAsStringAsync());

            Assert.NotNull(responseBody);
            Assert.Equal("UPDATED", responseBody["message"].ToString());
        }

        [Fact]
        public async Task ShouldReturnNoContentWhenDeleteTest()
        {
            HttpResponseMessage response = await Client.DeleteAsync($"{Fixture.Container.GetUrl()}/delete");

            Assert.Equal(204, (int)response.StatusCode);
        }

        [Fact]
        public async Task ShouldHandleConcurrentRequests()
        {
            var tasks = Enumerable.Range(0, 10).Select(async _ =>
            {
                HttpResponseMessage response = await Client.GetAsync(Fixture.Container.GetUrl());
                response.EnsureSuccessStatusCode();
                var responseBody = JsonSerializer.Deserialize<Dictionary<string, object>>(await response.Content.ReadAsStringAsync());
                Assert.NotNull(responseBody);
                Assert.Equal("SUCCESS", responseBody["message"].ToString());
            });

            await Task.WhenAll(tasks);
        }
    }
}
