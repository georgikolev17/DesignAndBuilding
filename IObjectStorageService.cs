using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace DesignAndBuilding.Services
{
    public interface IObjectStorageService
    {
        void UploadAsync(string bucket, string key, Stream data, string contentType);
        Stream DownloadAsync(string bucket, string key);
        Task DeleteAsync(string bucket, string key);
    }

}
