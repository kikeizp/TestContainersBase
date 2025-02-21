using Amazon.SQS;
using IntegrationTestingBase.Containers.AWS;

namespace Tests.Containers.AWS
{
    [CollectionDefinition("AwsContainer collection")]
    public class AwsContainerCollection : ICollectionFixture<AwsContainerFixture> { }

    public class AwsContainerFixture : IDisposable
    {
        public AwsContainer Container { get; private set; }

        public AwsContainerFixture()
        {
            Container = new AwsContainer(new AwsConfig
            {
                Credentials = new AwsCredentials { AccessKey = "key", AccessSecret = "secret" },
                Services = [AwsServices.SQS]
            });
            Container.Start().GetAwaiter().GetResult();
        }

        public void Dispose()
        {
            Container?.Stop().GetAwaiter().GetResult();
            GC.SuppressFinalize(this);
        }
    }

    [Collection("AwsContainer collection")]
    public class AwsContainerTests(AwsContainerFixture fixture)
    {
        private readonly AwsContainerFixture Fixture = fixture;

        [Fact]
        public void ShouldReturnCorrectRegion()
        {
            var region = Fixture.Container.GetRegion();
            Assert.Equal("us-east-1", region);
        }

        [Fact]
        public void ShouldReturnValidSqsClient()
        {
            var sqsClient = Fixture.Container.GetClient<AmazonSQSClient>(new AmazonSQSConfig 
            {
                ServiceURL = Fixture.Container.GetUrl(),
                AuthenticationRegion = Fixture.Container.GetRegion()
            });
            Assert.NotNull(sqsClient);
            Assert.IsType<AmazonSQSClient>(sqsClient);
        }
    }

}