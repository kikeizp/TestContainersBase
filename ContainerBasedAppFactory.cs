using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using IntegrationTestingBase.Containers;

namespace IntegrationTestingBase
{
    public abstract class ContainerBasedAppFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected ContainerRegistry ContainerRegistry { get; private set; } = ContainerRegistry.GetInstance;

        public ContainerBasedAppFactory(Dictionary<string, BaseConfig> configs)
        {
            Console.WriteLine("🔄 Initializing Containers for Integration Tests...");
            ContainerRegistry.GetInstance.InitializeContainers(configs).GetAwaiter().GetResult();
        }

        protected static void SetEnvironments(Dictionary<string, string> variables)
        {
            foreach (var (name, value) in variables)
            {
                Environment.SetEnvironmentVariable(name, value);
            }
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                Console.WriteLine("Configuring DI services...");
                ConfigureServicesWithContainers(services);
            });
        }

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

        protected static void RegisterSingleton<TService>(IServiceCollection services, TService instance)
            where TService : class
        {
            services.AddSingleton(instance);
        }

        protected abstract void ConfigureServicesWithContainers(IServiceCollection services);
    }
}
