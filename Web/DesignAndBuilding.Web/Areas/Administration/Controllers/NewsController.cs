namespace DesignAndBuilding.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using DesignAndBuilding.Common;
    using DesignAndBuilding.Services;
    using DesignAndBuilding.Web.Controllers;
    using DesignAndBuilding.Web.ViewModels.News;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administration")]
    public class NewsController : BaseController
    {
        private readonly INewsService newsService;
        private readonly IMapper mapper;

        public NewsController(INewsService newsService, IMapper mapper)
        {
            this.newsService = newsService;
            this.mapper = mapper;
        }

        // GET: Administration/News
        public async Task<IActionResult> Index()
        {
            var news = await this.newsService.GetAllNewsAsync();
            var viewModel = this.mapper.Map<NewsListViewModel[]>(news);

            return this.View(viewModel);
        }

        // GET: Administration/News/Create
        public IActionResult Create()
        {
            return this.View();
        }

        // POST: Administration/News/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NewsInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.newsService.CreateNewsAsync(input.Title, input.Content, input.IsActive);

            this.TempData["SuccessMessage"] = "Новината е създадена успешно.";

            return this.RedirectToAction(nameof(Index));
        }

        // GET: Administration/News/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var news = await this.newsService.GetNewsByIdAsync(id);

            if (news == null)
            {
                return this.NotFound();
            }

            var viewModel = this.mapper.Map<NewsViewModel>(news);

            return this.View(viewModel);
        }

        // POST: Administration/News/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, NewsViewModel model)
        {
            if (id != model.Id)
            {
                return this.NotFound();
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var result = await this.newsService.UpdateNewsAsync(id, model.Title, model.Content, model.IsActive);

            if (!result)
            {
                return this.NotFound();
            }

            this.TempData["SuccessMessage"] = "Новината е редактирана успешно.";

            return this.RedirectToAction(nameof(Index));
        }

        // GET: Administration/News/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var news = await this.newsService.GetNewsByIdAsync(id);

            if (news == null)
            {
                return this.NotFound();
            }

            var viewModel = this.mapper.Map<NewsViewModel>(news);

            return this.View(viewModel);
        }

        // POST: Administration/News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await this.newsService.DeleteNewsAsync(id);

            if (!result)
            {
                return this.NotFound();
            }

            this.TempData["SuccessMessage"] = "Новината е изтрита успешно.";

            return this.RedirectToAction(nameof(Index));
        }
    }
}
