using DesignAndBuilding.Data.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DesignAndBuilding.Data.Models
{
    public class Conversation : BaseDeletableModel<int>
    {
        public Conversation()
        {
            this.Participants = new HashSet<ConversationParticipant>();
            this.Messages = new HashSet<ChatMessage>();
        }

        public Conversation(ConversationType type)
        {
            this.Type = type;
            this.Participants = new HashSet<ConversationParticipant>();
            this.Messages = new HashSet<ChatMessage>();
        }

        public Conversation(ConversationType type, int assignmentId)
        {
            this.Type = type;
            this.AssignmentId = assignmentId;
            this.Participants = new HashSet<ConversationParticipant>();
            this.Messages = new HashSet<ChatMessage>();
        }

        public ConversationType Type { get; set; }

        // Optional link to an assignment (for public Q&A or assignment-specific private chats)
        public int? AssignmentId { get; set; }

        public virtual Assignment? Assignment { get; set; }

        // For PublicAssignment:
        //   - Participants collection should be empty (access is rule-based).
        // For Private1To1:
        //   - Participants must contain exactly architect + engineer.
        public virtual ICollection<ConversationParticipant> Participants { get; set; }

        public virtual ICollection<ChatMessage> Messages { get; set; }
    }

}
