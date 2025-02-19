using IntegrationTestingBase.Containers;
using IntegrationTestingBase.Containers.Registry;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTestingBase.Core.Factories
{
    public abstract class TestAppFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected TestContainerRegistry Registry { get; } = TestContainerRegistry.GetInstance;

        public TestAppFactory(Dictionary<string, BaseConfig> configs)
        {
            Registry.InitializeContainers(configs).GetAwaiter().GetResult();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder) =>
            builder.ConfigureServices(ConfigureServicesWithContainers);

        protected abstract void ConfigureServicesWithContainers(IServiceCollection services);

        protected static void RegisterHttpClient<TInterface, TImplementation>(IServiceCollection services, string url)
            where TInterface : class
            where TImplementation : class, TInterface
        {
            services.AddHttpClient<TInterface, TImplementation>(client =>
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });
        }

        protected static void SetEnvironments(Dictionary<string, string> variables)
        {
            foreach (var (name, value) in variables)
            {
                Environment.SetEnvironmentVariable(name, value);
            }
        }

        protected static void RegisterSingleton<TService>(IServiceCollection services, TService instance)
            where TService : class
        {
            services.AddSingleton(instance);
        }
    }
}