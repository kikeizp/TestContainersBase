using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;

namespace IntegrationTestingBase.Containers
{

    /// <summary>
    /// Interface representing the basic operations and configurations for a container.
    /// </summary>
    public interface IBaseContainer
    {
        /// <summary>
        /// Starts the container asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task Start();

        /// <summary>
        /// Stops the container asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task Stop();

        /// <summary>
        /// Retrieves the mapped public port of the container.
        /// </summary>
        /// <returns>The mapped port as an unsigned short.</returns>
        ushort GetPort();

        /// <summary>
        /// Retrieves the base URL of the running container.
        /// </summary>
        /// <returns>The URL as a string.</returns>
        string GetUrl();
    }

    public abstract class BaseContainer : IBaseContainer
    {
        private const string BaseUrl = "http://localhost";
        protected abstract ushort Port { get; }
        protected abstract string ImageName { get; }
        protected virtual Dictionary<string, string> EnvVariables { get; } = [];
        private IContainer Container { get; }

        protected ContainerBuilder AddEnvironments(ContainerBuilder container)
        {
            foreach (var variable in EnvVariables)
            {
                container = container.WithEnvironment(variable.Key, variable.Value);
            }

            return container;
        }

        protected BaseContainer()
        {
            Container = CustomizeContainer(AddEnvironments(CreateBaseContainer())).Build()
            ?? throw new Exception($"Failed to build container for image {ImageName}");
        }

        protected virtual ContainerBuilder CustomizeContainer(ContainerBuilder container)
        {
            return container;
        }

        protected abstract IWaitForContainerOS DefineWaitStrategy(IWaitForContainerOS strategy);

        private ContainerBuilder CreateBaseContainer()
        {
            Console.WriteLine($"Building container for image: {ImageName} and port {Port}");

            var builder = new ContainerBuilder()
                .WithName(Guid.NewGuid().ToString("D"))
                .WithImage(ImageName)
                .WithPortBinding(Port, true)
                .WithWaitStrategy(DefineWaitStrategy(Wait.ForUnixContainer()));

            return builder;
        }
        public Task Start(){
            return Container.StartAsync();
        }

        public Task Stop(){
            return Container.StopAsync();
        }

        public ushort GetPort(){
            return Container.GetMappedPublicPort(Port);
        }

        public string GetUrl(){
            return $"{BaseUrl}:{Container.GetMappedPublicPort(Port)}";
        }
    }

}
