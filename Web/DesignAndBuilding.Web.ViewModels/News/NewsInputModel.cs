namespace DesignAndBuilding.Web.ViewModels.News
{
    using System.ComponentModel.DataAnnotations;

    public class NewsInputModel
    {
        [Required(ErrorMessage = "Заглавието е задължително.")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Заглавието трябва да е между 5 и 200 символа.")]
        [Display(Name = "Заглавие")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Съдържанието е задължително.")]
        [StringLength(5000, MinimumLength = 10, ErrorMessage = "Съдържанието трябва да е между 10 и 5000 символа.")]
        [Display(Name = "Съдържание")]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        [Display(Name = "Активна")]
        public bool IsActive { get; set; }
    }
}
