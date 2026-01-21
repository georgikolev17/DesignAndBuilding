namespace DesignAndBuilding.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using DesignAndBuilding.Data.Common.Models;

    public class News : BaseDeletableModel<int>
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public bool IsActive { get; set; }
    }
}
