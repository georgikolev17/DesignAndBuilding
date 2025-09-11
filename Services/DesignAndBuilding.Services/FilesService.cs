namespace DesignAndBuilding.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using DesignAndBuilding.Data.Common.Repositories;
    using DesignAndBuilding.Data.Models;
    using DesignAndBuilding.Services.DTOs;
    using DesignAndBuilding.Services.Storage;

    // TODO: Add synchronization between database and object storage.

    /// <summary>
    /// Manages adding files' metadata to the database and files' actual data in the object storage.
    /// </summary>
    public class FilesService : IFilesService
    {
        private readonly IObjectStorageService objectStorageService;
        private readonly IDescriptionFilesService descriptionFilesService;

        public FilesService(IObjectStorageService objectStorageService, IDescriptionFilesService descriptionFilesService)
        {
            this.objectStorageService = objectStorageService;
            this.descriptionFilesService = descriptionFilesService;
        }

        public async Task AddDescriptionFileAsync(NewDescriptionFileDto descriptionDto)
        {
            using var stream = new MemoryStream(descriptionDto.Data);
            // Save the file to the object storage
            // The file key in the bucket is structured as "{assignmentId}/{fileName}"
            await this.objectStorageService.UploadAsync(R2CloudflareConfig.Bucket, $"{descriptionDto.AssignmentId}/{descriptionDto.Name}", stream, descriptionDto.ContentType);

            // Save the file metadata to the database
            await this.descriptionFilesService.CreateDescriptionFileAsync(descriptionDto.Name, (int)descriptionDto.Data.Length, descriptionDto.ContentType, descriptionDto.AssignmentId);
        }

        public async Task AddDescriptionFilesAsync(IEnumerable<NewDescriptionFileDto> descriptionDtos)
        {
            foreach (var desc in descriptionDtos)
            {
                await this.AddDescriptionFileAsync(desc);
            }
        }

        public async Task DeleteDescriptionFileAsync(string name, int assignmentId)
        {
            // Delete the file from the object storage
            await this.objectStorageService.DeleteAsync(R2CloudflareConfig.Bucket, $"{assignmentId}/{name}");

            // Delete the file metadata from the database
            await this.descriptionFilesService.DeleteDescriptionFileAsync(name, assignmentId);
        }

        public async Task<IDictionary<string, Stream>> GetDescriptionFilesForAssignmentAsync(int assignmentId)
        {
            var result = new Dictionary<string, Stream>();

            var descriptions = await this.descriptionFilesService.GetAllDescriptionFilesForAssignmentAsync(assignmentId);

            foreach (var desc in descriptions)
            {
                var descData = await this.objectStorageService.DownloadAsync(R2CloudflareConfig.Bucket, $"{assignmentId}/{desc.Name}");
                result.Add(desc.Name, descData);
            }

            return result;
        }
    }
}
