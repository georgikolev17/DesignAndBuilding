using DesignAndBuilding.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignAndBuilding.Services
{
    public interface IConversationsService
    {
        // ----- Get a conversation -----

        /// <summary>
        /// Gets a conversation by ID including optional navigation properties.
        /// </summary>
        Task<Conversation> GetConversationAsync(int conversationId);

        /// <summary>
        /// Gets or creates the public assignment conversation for an assignment.
        /// </summary>
        Task<Conversation> GetOrCreatePublicAssignmentConversationAsync(int assignmentId);

        // ----- Private conversations -----

        /// <summary>
        /// Gets or creates a private 1-to-1 chat between architect and specific engineer for an assignment.
        /// </summary>
        Task<Conversation> GetOrCreatePrivateConversationAsync(int assignmentId, string architectId, string engineerId);

        // ----- Authorization checks -----

        /// <summary>
        /// Checks if a user is allowed to *participate* (send messages) in a conversation.
        /// </summary>
        Task<bool> CanUserParticipateAsync(int conversationId, string userId);

        // ----- Listing conversations -----

        /// <summary>
        /// Gets all conversations visible to the user (public and private).
        /// </summary>
        Task<IReadOnlyList<Conversation>> GetUserPrivateConversationsAsync(string userId);
    }

}
