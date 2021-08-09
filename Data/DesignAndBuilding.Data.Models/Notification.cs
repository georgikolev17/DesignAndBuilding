namespace DesignAndBuilding.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using DesignAndBuilding.Data.Common.Models;

    public class Notification : IDeletableEntity, IAuditInfo
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Message { get; set; }

        [Required]
        public bool IsRead { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        // Deletable Entity
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        // Audit Info
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
