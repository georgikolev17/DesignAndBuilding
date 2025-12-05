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

        public Conversation(int assignmentId)
        {
            this.AssignmentId = assignmentId;
            this.Participants = new HashSet<ConversationParticipant>();
            this.Messages = new HashSet<ChatMessage>();
        }

        // Optional link to an assignment (for public Q&A or assignment-specific private chats)
        public int AssignmentId { get; set; }

        public virtual Assignment Assignment { get; set; }

        public virtual ICollection<ConversationParticipant> Participants { get; set; }

        public virtual ICollection<ChatMessage> Messages { get; set; }
    }

}
