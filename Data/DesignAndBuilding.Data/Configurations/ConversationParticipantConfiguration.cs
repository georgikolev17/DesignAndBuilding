namespace DesignAndBuilding.Data.Configurations
{
    using DesignAndBuilding.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ConversationParticipantConfiguration : IEntityTypeConfiguration<ConversationParticipant>
    {
        public void Configure(EntityTypeBuilder<ConversationParticipant> builder)
        {
            builder.HasKey(cp => new { cp.ConversationId, cp.UserId });
        }
    }
}
