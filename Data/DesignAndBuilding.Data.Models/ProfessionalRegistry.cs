namespace DesignAndBuilding.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using DesignAndBuilding.Data.Common.Models;

    public class ProfessionalRegistry
    {
        public ProfessionalRegistry()
        {
        }

        public ProfessionalRegistry(string registrationNumber, string fullName, string discipline)
        {
            this.RegistrationNumber = registrationNumber;
            this.FullName = fullName;
            this.Discipline = discipline;
        }

        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string RegistrationNumber { get; set; } // Unique registration number

        [Required]
        [MaxLength(200)]
        public string FullName { get; set; }

        [Required]
        [MaxLength(20)]
        public string Discipline { get; set; } // Temporarily string - will convert to UserType enum later
    }
}
