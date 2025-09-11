using DesignAndBuilding.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignAndBuilding.Services
{
    public interface IDescriptionFilesService
    {
        Task CreateDescriptionFileAsync(string name, int size, string contentType, int assignmentId);

        Task DeleteDescriptionFileAsync(string name, int assignmentId);

        Task<IEnumerable<DescriptionFile>> GetAllDescriptionFilesForAssignmentAsync(int assignmentId);

        Task<DescriptionFile> GetDescriptionFileAsync(string name, int assignmentId);
    }
}
