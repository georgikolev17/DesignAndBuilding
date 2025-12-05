using System;
using System.Collections.Generic;
using System.Text;

namespace DesignAndBuilding.Web.ViewModels.Question
{
    public class QuestionListViewModel
    {
        // Question ID
        public int Id { get; set; }

        // The question text
        public string Text { get; set; }

        // Name of the engineer who asked the question
        public string EngineerName { get; set; }

        // When the question was asked
        public DateTime CreatedOn { get; set; }

        // Answer text, or null if no answer
        public string? AnswerText { get; set; }

        // Name of the architect who answered
        public string? AnswerArchitectName { get; set; }

        // When the answer was posted
        public DateTime? AnswerCreatedOn { get; set; }
    }
}
