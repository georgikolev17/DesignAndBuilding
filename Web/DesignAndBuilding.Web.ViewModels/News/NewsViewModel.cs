namespace DesignAndBuilding.Web.ViewModels.News
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class NewsViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Заглавието трябва да е между 5 и 200 символа.")]
        [Display(Name = "Заглавие")]
        public string Title { get; set; }

        [Required]
        [StringLength(5000, MinimumLength = 10, ErrorMessage = "Съдържанието трябва да е между 10 и 5000 символа.")]
        [Display(Name = "Съдържание")]
        public string Content { get; set; }

        [Display(Name = "Активна")]
        public bool IsActive { get; set; }

        [Display(Name = "Дата на създаване")]
        public DateTime CreatedOn { get; set; }

        [Display(Name = "Последна промяна")]
        public DateTime? ModifiedOn { get; set; }
    }
}
