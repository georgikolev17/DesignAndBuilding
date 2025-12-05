namespace DesignAndBuilding.Web.Controllers.Api
{
    using System;
    using System.Threading.Tasks;

    using DesignAndBuilding.Data.Models;
    using DesignAndBuilding.Services;
    using DesignAndBuilding.Web.ViewModels.Answer;
    using DesignAndBuilding.Web.ViewModels.Question;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    [Route("api/qanda")]
    [ApiController]
    [IgnoreAntiforgeryToken]
    public class QandAApiController : ControllerBase
    {
        private readonly IQandAService qandAService;
        private readonly UserManager<ApplicationUser> userManager;

        public QandAApiController(IQandAService qandAService, UserManager<ApplicationUser> userManager)
        {
            this.qandAService = qandAService;
            this.userManager = userManager;
        }

        [HttpPost("ask")]
        public async Task<IActionResult> Ask(QuestionInputViewModel question)
        {
            var engineerId = this.userManager.GetUserId(this.User);
            AssignmentQuestion quest;
            try
            {
                quest = await this.qandAService.AskQuestionAsync(
                    question.AssignmentId,
                    engineerId,
                    question.Text);
            }
            catch (Exception ex)
            {
                return this.Forbid(ex.Message);
            }

            return this.Ok(new { quest.Text });
        }

        [HttpPost("answer")]
        public async Task<IActionResult> Answer(AnswerInputViewModel answer)
        {
            var architectId = this.userManager.GetUserId(this.User);

            AssignmentAnswer ans;
            try
            {
                ans = await this.qandAService.AnswerQuestionAsync(
                    answer.QuestionId,
                    architectId,
                    answer.Text);
            }
            catch (Exception ex)
            {
                return this.Forbid(ex.Message);
            }

            return this.Ok(new { ans.Text, ans.QuestionId });
        }

    }
}
