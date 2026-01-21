namespace DesignAndBuilding.Web.ViewModels.Home
{
    using System.Collections.Generic;
    using DesignAndBuilding.Web.ViewModels.News;

    public class HomeIndexViewModel
    {
        public IEnumerable<NewsListViewModel> News { get; set; }
    }
}
