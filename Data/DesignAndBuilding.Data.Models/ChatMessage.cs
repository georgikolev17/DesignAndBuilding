namespace DesignAndBuilding.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using DesignAndBuilding.Data.Common.Models;

    public class ChatMessage : BaseDeletableModel<int>
    {
        public ChatMessage()
        {
        }

        public ChatMessage(int conversationId, string senderId, string text)
        {
            this.ConversationId = conversationId;
            this.SenderId = senderId;
            this.Text = text;
            this.CreatedOn = DateTime.UtcNow;
        }

        public int ConversationId { get; set; }

        public virtual Conversation Conversation { get; set; }

        public string SenderId { get; set; }

        public virtual ApplicationUser Sender { get; set; }

        public string Text { get; set; }
    }
}
