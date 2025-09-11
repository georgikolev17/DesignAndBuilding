namespace DesignAndBuilding.Services.Storage
{
    using Amazon.Runtime;
    using Amazon.S3;

    public static class R2Client
    {
        public static IAmazonS3 Create()
        {
            var s3cfg = new Amazon.S3.AmazonS3Config
            {
                ServiceURL = $"https://{R2CloudflareConfig.AccountId}.r2.cloudflarestorage.com",
                ForcePathStyle = true,
            };
            return new AmazonS3Client(
                new BasicAWSCredentials(R2CloudflareConfig.AccessKeyId, R2CloudflareConfig.SecretAccessKey),
                s3cfg
            );
        }
    }
}
