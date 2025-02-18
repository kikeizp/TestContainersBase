namespace IntegrationTestingBase.Containers.Custom
{
    public record CustomContainerConfig(string ImageUrl, ushort Port, Dictionary<string, string> Variables) : BaseConfig;

}
