using IntegrationTestingBase.Containers;
using IntegrationTestingBase.Core.Interfaces;

namespace IntegrationTestingBase.Core.Factories
{
    public abstract class ConfigurableTestFactory<TProgram> : TestAppFactory<TProgram>, IConfigurableTestFactory<TProgram>
    where TProgram : class
    {
        protected ConfigurableTestFactory(Dictionary<string, BaseConfig> configs) : base(configs) =>
            ConfigureClients().GetAwaiter().GetResult();

        public abstract Task ConfigureClients();
    }
}