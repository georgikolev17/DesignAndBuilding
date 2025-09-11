namespace DesignAndBuilding.Data.Models
{
    using DesignAndBuilding.Data.Common.Models;

    public class DescriptionFile : BaseDeletableModel<int>
    {
        public DescriptionFile(string name, int size, string contentType, int assignmentId)
        {
            this.Name = name;
            this.Size = size;
            this.ContentType = contentType;
            this.AssignmentId = assignmentId;
        }

        public string Name { get; set; }

        public int Size { get; set; }

        public string ContentType { get; set; }

        public int AssignmentId { get; set; }

        public virtual Assignment Assignment { get; set; }
    }
}
