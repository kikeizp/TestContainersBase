
using IntegrationTestingBase.Containers;

namespace IntegrationTestingBase
{
    public abstract class BaseConfigurableFactory<TProgram> : ContainerBasedAppFactory<TProgram> where TProgram : class
    {
        protected BaseConfigurableFactory(Dictionary<string, BaseConfig> configs) : base(configs)
        {
            Console.WriteLine("🔄 Configuring Clients and Containers...");
            ConfigureClients().GetAwaiter().GetResult();
        }

        protected abstract Task ConfigureClients();
    }
}
