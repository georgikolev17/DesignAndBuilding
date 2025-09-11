using Amazon.Runtime;
using Amazon.S3;
using DesignAndBuilding.Services;
using DesignAndBuilding.Services.Storage;
using System.Text;

var s3cfg = new Amazon.S3.AmazonS3Config
{
    ServiceURL = $"https://{R2CloudflareConfig.AccountId}.r2.cloudflarestorage.com",
    ForcePathStyle = true,
};
var client = new AmazonS3Client(
    new BasicAWSCredentials(R2CloudflareConfig.AccessKeyId, R2CloudflareConfig.SecretAccessKey),
    s3cfg
);

var storage = new R2StorageService(client);

byte[] bytes = Encoding.UTF8.GetBytes("Testing2");

// Create a MemoryStream containing the data
using var stream = new MemoryStream(bytes);
stream.Position = 0;

var file = await storage.DownloadAsync(R2CloudflareConfig.Bucket, "15/tatko original 2.pdf");
Console.WriteLine();

using (var fileStream = File.Create(@"D:\TUe\Year-2\Q1\Algebra for Security\tatko original 2.pdf"))
{
    await file.CopyToAsync(fileStream);
}

// await storage.UploadAsync(R2CloudflareConfig.Bucket, "dasdas/test.txt", stream, "text/plain");

