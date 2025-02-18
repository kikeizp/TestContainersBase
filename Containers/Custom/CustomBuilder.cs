
using DotNet.Testcontainers.Configurations;

namespace IntegrationTestingBase.Containers.Custom
{
    public class CustomContainer(CustomContainerConfig config) : BaseContainer
    {
        protected override string ImageName => config.ImageUrl;
        protected override ushort Port => config.Port;

        protected override Dictionary<string, string> EnvVariables => config.Variables;

        protected override IWaitForContainerOS DefineWaitStrategy(IWaitForContainerOS strategy)
        {
            return strategy.UntilPortIsAvailable(Port);
        }

    }
}

