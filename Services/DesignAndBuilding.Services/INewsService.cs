namespace DesignAndBuilding.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using DesignAndBuilding.Data.Models;

    public interface INewsService
    {
        Task<IEnumerable<News>> GetAllNewsAsync();

        Task<IEnumerable<News>> GetActiveNewsAsync();

        Task<News> GetNewsByIdAsync(int id);

        Task<int> CreateNewsAsync(string title, string content, bool isActive);

        Task<bool> UpdateNewsAsync(int id, string title, string content, bool isActive);

        Task<bool> DeleteNewsAsync(int id);

        Task<bool> NewsExistsAsync(int id);
    }
}
