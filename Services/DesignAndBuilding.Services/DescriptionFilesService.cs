namespace DesignAndBuilding.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using DesignAndBuilding.Data.Common.Repositories;
    using DesignAndBuilding.Data.Models;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Provides functionality for managing description files in the database.
    /// </summary>
    public class DescriptionFilesService : IDescriptionFilesService
    {
        private readonly IDeletableEntityRepository<DescriptionFile> descriptionFiles;

        public DescriptionFilesService(IDeletableEntityRepository<DescriptionFile> descriptionFiles)
        {
            this.descriptionFiles = descriptionFiles;
        }

        /// <summary>
        /// Creates a description file entry in the database. If the file with the same name and assignmentId exists, it gets replaced by the new one.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <param name="contentType"></param>
        /// <param name="assignmentId"></param>
        /// <returns></returns>
        public async Task CreateDescriptionFileAsync(string name, int size, string contentType, int assignmentId)
        {
            var description = await this.GetDescriptionFileAsync(name, assignmentId);
            if (description == null)
            {
                await this.descriptionFiles.AddAsync(new DescriptionFile(name, size, contentType, assignmentId));
            }
            else
            {
                description.CreatedOn = DateTime.UtcNow;
                description.Size = size;
                description.ContentType = contentType;
                this.descriptionFiles.Update(description);
            }

            await this.descriptionFiles.SaveChangesAsync();
        }

        public async Task DeleteDescriptionFileAsync(string name, int assignmentId)
        {
            var description = await this.GetDescriptionFileAsync(name, assignmentId);
            if (description != null)
            { 
                description.IsDeleted = true;
                try
                {
                    await descriptionFiles.SaveChangesAsync(); // DbContext
                }
                catch (Exception ex)
                {
                    throw; // rethrow so you see it in dev
                }
            }
        }

        /// <summary>
        /// Returns the metadata for all description files for a specific assignment.
        /// </summary>
        /// <param name="assignmentId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<DescriptionFile>> GetAllDescriptionFilesForAssignmentAsync(int assignmentId)
        {
            var descriptions = await Task.FromResult(this.descriptionFiles.All().Where(x => x.AssignmentId == assignmentId).AsEnumerable());
            return descriptions;
        }

        public async Task<DescriptionFile> GetDescriptionFileAsync(string name, int assignmentId)
        {
            var description = await this.descriptionFiles.All().FirstOrDefaultAsync(x => x.AssignmentId == assignmentId && x.Name == name);
            return description;
        }
    }
}
