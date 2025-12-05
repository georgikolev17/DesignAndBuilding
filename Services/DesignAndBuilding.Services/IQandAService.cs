using DesignAndBuilding.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignAndBuilding.Services
{
    public interface IQandAService
    {
        // -------- QUESTIONS --------

        /// <summary>
        /// Creates a new question for an assignment, asked by an engineer.
        /// Validates that the user is allowed to ask questions on this assignment.
        /// </summary>
        Task<AssignmentQuestion> AskQuestionAsync(
            int assignmentId,
            string engineerId,
            string text);

        /// <summary>
        /// Returns all questions (with optional answers) for an assignment,
        /// ordered by creation time.
        /// </summary>
        Task<IReadOnlyList<AssignmentQuestion>> GetQuestionsForAssignmentAsync(
            int assignmentId);

        /// <summary>
        /// Gets a single question by id (including its answer).
        /// Returns null if not found or not visible to this user (optional).
        /// </summary>
        Task<AssignmentQuestion?> GetQuestionAsync(int questionId);


        // -------- ANSWERS --------

        /// <summary>
        /// Creates an answer for a question, or replaces the existing one,
        /// enforcing that the author is the assignment's architect.
        /// </summary>
        Task<AssignmentAnswer> AnswerQuestionAsync(
            int questionId,
            string architectId,
            string text);

        /// <summary>
        /// Edits an existing answer (e.g. to correct or clarify).
        /// Only the architect (or same author) is allowed to edit.
        /// </summary>
        Task<AssignmentAnswer> EditAnswerAsync(
            int answerId,
            string architectId,
            string newText);


        // -------- AUTHORIZATION HELPERS (optional but useful) --------

        /// <summary>
        /// Can this user ask questions on this assignment?
        /// (e.g. engineer with correct discipline, not blocked, etc.)
        /// </summary>
        Task<bool> CanUserAskQuestionAsync(int assignmentId, string userId);

        /// <summary>
        /// Can this user answer questions for this assignment?
        /// (e.g. the architect of this assignment).
        /// </summary>
        Task<bool> CanUserAnswerQuestionAsync(int questionId, string userId);
    }

}
