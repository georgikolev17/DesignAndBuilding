namespace DesignAndBuilding.Services
{
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    public interface IObjectStorageService
    {
        Task UploadAsync(string bucket, string key, Stream data, string contentType);

        Task<Stream> DownloadAsync(string bucket, string key);

        Task DeleteAsync(string bucket, string key);
    }
}
