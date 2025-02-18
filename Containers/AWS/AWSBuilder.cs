using Amazon.DynamoDBv2;
using Amazon.Runtime;
using Amazon.SimpleNotificationService.Model;
using Amazon.SQS;
using DotNet.Testcontainers.Configurations;

namespace IntegrationTestingBase.Containers.AWS
{
public class AwsContainer(AwsConfig config) : BaseContainer
    {
        private static string FormatServices(IEnumerable<AwsServices> services)
        {
            if (services == null || !services.Any())
            {
                throw new ArgumentException("At least one AWS service must be provided.");
            }

            return string.Join(",", services.Select(s => s.ToString().ToLowerInvariant()));
        }

        protected override string ImageName => "localstack/localstack:latest";
        protected override ushort Port => 4566;

        private string Region => "us-east-1";

        protected override Dictionary<string, string> EnvVariables => new ()
        {
            {"SERVICES", FormatServices(config.Services)},
            {"AWS_ACCESS_KEY_ID", config.Credentials.AccessKey},
            {"AWS_SECRET_ACCESS_KEY", config.Credentials.AccessSecret},
        };

        protected override IWaitForContainerOS DefineWaitStrategy(IWaitForContainerOS strategy)
        {
            return strategy.UntilHttpRequestIsSucceeded(r => r.ForPort(Port));
        }

        public string GetRegion()
        {
            return Region;
        }

        public TClient GetClient<TClient>(ClientConfig config) where TClient : AmazonServiceClient
        {
            var credentials = GetCredentials();

            AmazonServiceClient client =  config switch
            {
                AmazonDynamoDBConfig awsConfig => new AmazonDynamoDBClient(credentials, awsConfig),
                AmazonSQSConfig awsConfig => new AmazonSQSClient(credentials, awsConfig),
                _ => throw new NotFoundException("Client do not exist")
            };

            return (TClient) client;
        }

        private BasicAWSCredentials GetCredentials()
        {
            return new BasicAWSCredentials(config.Credentials.AccessKey, config.Credentials.AccessSecret);
        }
    }
}
