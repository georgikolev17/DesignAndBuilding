namespace DesignAndBuilding.Web.ViewModels.Notification
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class NotificationViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string Message { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public bool IsRead { get; set; }
    }
}
