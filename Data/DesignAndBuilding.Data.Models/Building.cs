namespace DesignAndBuilding.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using DesignAndBuilding.Data.Common.Models;

    public class Building : BaseDeletableModel<int>
    {
        public Building()
        {
            this.Assignments = new HashSet<Assignment>();
        }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Town { get; set; }

        [Required]
        public string TotalBuildUpArea { get; set; }

        [Required]
        public int ArchitectId { get; set; }

        public ApplicationRole Architect { get; set; }

        public virtual ICollection<Assignment> Assignments { get; set; }
    }
}
