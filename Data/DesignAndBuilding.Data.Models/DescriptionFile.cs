namespace DesignAndBuilding.Data.Models
{
    using DesignAndBuilding.Data.Common.Models;

    public class DescriptionFile : BaseDeletableModel<int>
    {
        public string Name { get; set; }

        public byte[] Content { get; set; }

        public string ContentType { get; set; }

        public int AssignmentId { get; set; }

        public virtual Assignment Assignment { get; set; }
    }
}
