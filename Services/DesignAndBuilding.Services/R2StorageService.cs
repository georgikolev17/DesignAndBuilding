namespace DesignAndBuilding.Services
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    using Amazon.S3;
    using Amazon.S3.Model;

    public class R2StorageService : IObjectStorageService
    {
        private readonly IAmazonS3 s3;

        public R2StorageService(IAmazonS3 s3)
        {
            this.s3 = s3;
        }

        public async Task UploadAsync(string bucket, string key, Stream data, string contentType)
        {
            var request = new PutObjectRequest
            {
                BucketName = bucket,
                Key = key,
                InputStream = data,
                ContentType = contentType,
                AutoCloseStream = true,
                DisablePayloadSigning = true,
                DisableDefaultChecksumValidation = true,
            };

            await this.s3.PutObjectAsync(request);
        }

        public async Task<Stream> DownloadAsync(string bucket, string key)
        {
            var response = await this.s3.GetObjectAsync(bucket, key);
            return response.ResponseStream;
        }

        public async Task DeleteAsync(string bucket, string key)
        {
            await this.s3.DeleteObjectAsync(bucket, key);
        }
    }
}
