using DesignAndBuilding.Data.Models;
using DesignAndBuilding.Services;
using DesignAndBuilding.Web.ViewModels.Answer;
using DesignAndBuilding.Web.ViewModels.Question;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DesignAndBuilding.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class QandAApiController : ControllerBase
    {
        private readonly IQandAService qandAService;

        public QandAApiController(IQandAService qandAService)
        {
            this.qandAService = qandAService;
        }

        [HttpPost("ask")]
        public async Task<IActionResult> Ask(QuestionInputViewModel question)
        {
            AssignmentQuestion quest;
            try
            {
                quest = await this.qandAService.AskQuestionAsync(
                    question.AssignmentId,
                    question.EngineerId,
                    question.Text);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }

            return this.Ok(quest);
        }

        [HttpPost("answer")]
        public async Task<IActionResult> Answer(AnswerInputViewModel answer)
        {
            AssignmentAnswer ans;
            try
            {
                ans = await this.qandAService.AnswerQuestionAsync(
                    answer.QuestionId,
                    answer.ArchitectId,
                    answer.Text);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }

            return this.Ok(ans);
        }

    }
}
