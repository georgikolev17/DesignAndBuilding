using DesignAndBuilding.Data.Common.Models;
using System;

namespace DesignAndBuilding.Data.Models
{
    public class AssignmentAnswer : BaseDeletableModel<int>
    {
        public AssignmentAnswer()
        {
        }

        public AssignmentAnswer(int questionId, string architectId, string text)
        {
            this.QuestionId = questionId;
            this.ArchitectId = architectId;
            this.Text = text;
        }

        public int QuestionId { get; set; }

        public virtual AssignmentQuestion Question { get; set; }

        public string ArchitectId { get; set; }

        public virtual ApplicationUser Architect { get; set; }

        public string Text { get; set; }
    }
}