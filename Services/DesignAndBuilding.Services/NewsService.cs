namespace DesignAndBuilding.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using DesignAndBuilding.Data.Common.Repositories;
    using DesignAndBuilding.Data.Models;
    using Microsoft.EntityFrameworkCore;

    public class NewsService : INewsService
    {
        private readonly IDeletableEntityRepository<News> newsRepository;

        public NewsService(IDeletableEntityRepository<News> newsRepository)
        {
            this.newsRepository = newsRepository;
        }

        public async Task<IEnumerable<News>> GetAllNewsAsync()
        {
            return await this.newsRepository
                .All()
                .OrderByDescending(n => n.CreatedOn)
                .ToListAsync();
        }

        public async Task<IEnumerable<News>> GetActiveNewsAsync()
        {
            return await this.newsRepository
                .All()
                .Where(n => n.IsActive)
                .OrderByDescending(n => n.CreatedOn)
                .ToListAsync();
        }

        public async Task<News> GetNewsByIdAsync(int id)
        {
            return await this.newsRepository
                .All()
                .FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task<int> CreateNewsAsync(string title, string content, bool isActive)
        {
            var news = new News
            {
                Title = title,
                Content = content,
                IsActive = isActive,
            };

            await this.newsRepository.AddAsync(news);
            await this.newsRepository.SaveChangesAsync();

            return news.Id;
        }

        public async Task<bool> UpdateNewsAsync(int id, string title, string content, bool isActive)
        {
            var news = await this.newsRepository
                .All()
                .FirstOrDefaultAsync(n => n.Id == id);

            if (news == null)
            {
                return false;
            }

            news.Title = title;
            news.Content = content;
            news.IsActive = isActive;

            this.newsRepository.Update(news);
            await this.newsRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteNewsAsync(int id)
        {
            var news = await this.newsRepository
                .All()
                .FirstOrDefaultAsync(n => n.Id == id);

            if (news == null)
            {
                return false;
            }

            this.newsRepository.Delete(news);
            await this.newsRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> NewsExistsAsync(int id)
        {
            return await this.newsRepository
                .All()
                .AnyAsync(n => n.Id == id);
        }
    }
}
