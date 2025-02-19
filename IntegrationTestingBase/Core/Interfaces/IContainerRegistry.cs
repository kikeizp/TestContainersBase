using IntegrationTestingBase.Containers;

namespace IntegrationTestingBase.Core.Interfaces
{
    public interface IContainerRegistry
    {
        Task InitializeContainers(Dictionary<string, BaseConfig> configs);
        T GetContainer<T>(string name) where T : class;
        bool IsContainerRegistered(string name);
        Dictionary<string, BaseContainer> GetContainers();
    }
}