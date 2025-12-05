namespace DesignAndBuilding.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using DesignAndBuilding.Data.Common.Models;
    using DesignAndBuilding.Data.Common.Repositories;
    using DesignAndBuilding.Data.Models;
    using Microsoft.EntityFrameworkCore;

    public class QandAService : IQandAService
    {
        private readonly IDeletableEntityRepository<AssignmentQuestion> questionsRepo;
        private readonly IDeletableEntityRepository<AssignmentAnswer> answersRepo;
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepo;
        private readonly IDeletableEntityRepository<Assignment> assignmentRepo;

        public QandAService(IDeletableEntityRepository<AssignmentQuestion> questionsRepo, IDeletableEntityRepository<AssignmentAnswer> answersRepo, IDeletableEntityRepository<ApplicationUser> usersRepo, IDeletableEntityRepository<Assignment> assignmentRepo)
        {
            this.questionsRepo = questionsRepo;
            this.answersRepo = answersRepo;
            this.usersRepo = usersRepo;
            this.assignmentRepo = assignmentRepo;
        }

        public async Task<AssignmentAnswer> AnswerQuestionAsync(int questionId, string architectId, string text)
        {
            if (!await this.CanUserAnswerQuestionAsync(questionId, architectId))
            {
                throw new Exception("User is not allowed to answer this question.");
            }

            var ans = new AssignmentAnswer(questionId, architectId, text);
            await this.answersRepo.AddAsync(ans);
            await this.answersRepo.SaveChangesAsync();

            return ans;
        }

        public async Task<AssignmentQuestion> AskQuestionAsync(int assignmentId, string engineerId, string text)
        {
            if (!await this.CanUserAskQuestionAsync(assignmentId, engineerId))
            {
                throw new Exception("User is not allowed to ask a question on this assignment.");
            }

            var question = new AssignmentQuestion(assignmentId, engineerId, text);
            await this.questionsRepo.AddAsync(question);
            await this.questionsRepo.SaveChangesAsync();

            return question;
        }

        public async Task<bool> CanUserAnswerQuestionAsync(int questionId, string userId)
        {
            var question = await this.questionsRepo.AllAsNoTracking()
                .Include(q => q.Assignment)
                .ThenInclude(a => a.Building)
                .FirstOrDefaultAsync(q => q.Id == questionId);
            if (question == null)
            {
                throw new Exception("Question not found.");
            }

            return question.Assignment.Building.ArchitectId == userId;
        }

        public async Task<bool> CanUserAskQuestionAsync(int assignmentId, string userId)
        {
            var user = await this.usersRepo.AllAsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);
            var assignment = await this.assignmentRepo.AllAsNoTracking().FirstOrDefaultAsync(a => a.Id == assignmentId);
            if (assignment == null || user == null)
            {
                return false;
            }

            // User can ask question if their user type matches the assignment's user type and the assignment is not finished
            return assignment.UserType == user.UserType && !assignment.IsFinished;
        }

        public async Task<AssignmentAnswer> EditAnswerAsync(int answerId, string architectId, string newText)
        {
            var answer = this.answersRepo.All()
                .FirstOrDefault(a => a.Id == answerId);
            if (answer == null)
            {
                throw new Exception("Answer not found.");
            }

            if (!await this.CanUserAnswerQuestionAsync(answer.QuestionId, architectId))
            {
                throw new Exception("User is not allowed to edit this answer.");
            }

            answer.Text = newText;
            await this.answersRepo.SaveChangesAsync();
            return answer;
        }

        public async Task<AssignmentQuestion?> GetQuestionAsync(int questionId)
        {
            return await this.questionsRepo.AllAsNoTracking()
                .Include(q => q.Answer)
                .FirstOrDefaultAsync(q => q.Id == questionId);
        }

        public async Task<IReadOnlyList<AssignmentQuestion>> GetQuestionsForAssignmentAsync(int assignmentId)
        {
            return await this.questionsRepo.AllAsNoTracking()
                .Where(q => q.AssignmentId == assignmentId)
                .Include(q => q.Answer)
                .OrderByDescending(q => q.CreatedOn)
                .ToListAsync();
        }
    }
}
