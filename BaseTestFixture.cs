namespace IntegrationTestingBase
{
    public abstract class BaseTestFixture<TFactory, TProgram> : IDisposable
        where TFactory : BaseConfigurableFactory<TProgram>, new()
        where TProgram : class
    {
        public TFactory Factory { get; }
        public IServiceProvider Services => Factory.Services;

        public BaseTestFixture()
        {
            Console.WriteLine("🔄 Initializing Test Environment...");
            Factory = new TFactory();
            Factory.Server.CreateClient();
        }

        public void Dispose()
        {
            Console.WriteLine("🛑 Disposing Test Environment...");
            Factory.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
