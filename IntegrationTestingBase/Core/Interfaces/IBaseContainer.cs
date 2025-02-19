namespace IntegrationTestingBase.Core.Interfaces
{
    public interface IBaseTestContainer
    {
        Task Start();
        Task Stop();
        ushort GetPort();
        string GetUrl();
    }
}