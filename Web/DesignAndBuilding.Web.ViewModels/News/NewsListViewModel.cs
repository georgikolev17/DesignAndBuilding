namespace DesignAndBuilding.Web.ViewModels.News
{
    using System;

    public class NewsListViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
