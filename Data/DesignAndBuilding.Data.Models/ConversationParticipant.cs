using DesignAndBuilding.Data.Common.Models;
using System;

namespace DesignAndBuilding.Data.Models
{
    public class ConversationParticipant : IDeletableEntity
    {
        public ConversationParticipant()
        {
        }

        public ConversationParticipant(string userId, int conversationId)
        {
            this.UserId = userId;
            this.ConversationId = conversationId;
        }

        public int ConversationId { get; set; }

        public virtual Conversation Conversation { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}