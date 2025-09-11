namespace DesignAndBuilding.Services.DTOs
{
    using System.IO;

    public class NewDescriptionFileDto
    {
        public NewDescriptionFileDto(byte[] data, string name, string contentType, int assignmentId)
        {
            this.Data = data;
            this.Name = name;
            this.ContentType = contentType;
            this.AssignmentId = assignmentId;
        }

        public byte[] Data { get; set; }

        public string Name { get; set; }

        public string ContentType { get; set; }

        public int AssignmentId { get; set; }
    }
}
