namespace IntegrationTestingBase.Containers.AWS
{
    public record AwsCredentials(string AccessKey, string AccessSecret);
    public enum AwsServices { S3, DynamoDB, SQS, SNS, Lambda, APIGateway, IAM, StepFunctions, Kinesis, SecretsManager }
    public record AwsConfig(IEnumerable<AwsServices> Services, AwsCredentials Credentials) : BaseConfig;
}