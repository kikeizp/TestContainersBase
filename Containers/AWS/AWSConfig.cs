namespace IntegrationTestingBase.Containers.AWS
{
    public enum AwsServices { S3, DynamoDB, SQS, SNS, Lambda, APIGateway, IAM, StepFunctions, Kinesis, SecretsManager }
    public record AwsCredentials
    {
        public required string AccessKey { get; set; }
        public required string AccessSecret { get; set; }
    }

    public record AwsConfig : BaseConfig
    {
        public required IEnumerable<AwsServices> Services { get; set; }
        public required AwsCredentials Credentials { get; set; }
    }
}