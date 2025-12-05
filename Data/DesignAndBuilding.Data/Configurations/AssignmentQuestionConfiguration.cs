namespace DesignAndBuilding.Data.Configurations
{
    using DesignAndBuilding.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class AssignmentQuestionConfiguration : IEntityTypeConfiguration<AssignmentQuestion>
    {
        public void Configure(EntityTypeBuilder<AssignmentQuestion> builder)
        {
            builder
                .HasOne(q => q.Answer)
                    .WithOne(a => a.Question)
                    .HasForeignKey<AssignmentAnswer>(a => a.QuestionId);
        }
    }
}
