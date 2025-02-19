namespace IntegrationTestingBase.Containers.Custom
{
    public record CustomContainerConfig : BaseConfig
    {

        public new required string Image { get; set; }
        public required ushort Port { get; set; }
        public required Dictionary<string, string> Variables { get; set; }
    }

}