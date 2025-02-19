using IntegrationTestingBase.Core.Factories;

namespace IntegrationTestingBase.Core.Fixtures
{
    public abstract class SharedTestFixture<TFactory, TProgram> : IDisposable
        where TFactory : ConfigurableTestFactory<TProgram>, new()
        where TProgram : class
    {
        public TFactory Factory { get; }
        public IServiceProvider Services => Factory.Services;

        public SharedTestFixture()
        {
            Factory = new TFactory();
            Factory.Server.CreateClient();
        }

        public void Dispose()
        {
            Factory.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}