namespace DesignAndBuilding.Services
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    using DesignAndBuilding.Services.DTOs;

    public interface IFilesService
    {
        Task AddDescriptionFileAsync(NewDescriptionFileDto descriptionDto);

        Task AddDescriptionFilesAsync(IEnumerable<NewDescriptionFileDto> descriptionDtos);

        /// <summary>
        /// The method returns key-value pairs, where the key is the file name and the value is a stream containing the file data.
        /// </summary>
        /// <param name="assignmentId"></param>
        /// <returns></returns>
        Task<IDictionary<string, Stream>> GetDescriptionFilesForAssignmentAsync(int assignmentId);

        Task DeleteDescriptionFileAsync(string name, int assignmentId);
    }
}
