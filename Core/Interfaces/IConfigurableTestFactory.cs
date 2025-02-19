namespace IntegrationTestingBase.Core.Interfaces
{
    public interface IConfigurableTestFactory<TProgram> where TProgram : class
    {
        Task ConfigureClients();
    }
}