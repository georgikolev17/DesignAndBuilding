namespace DesignAndBuilding.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using DesignAndBuilding.Data.Common.Models;

    public class AssignmentQuestion : BaseDeletableModel<int>
    {
        public AssignmentQuestion()
        {
        }

        public AssignmentQuestion(int assignmentId, string engineerId, string text)
        {
            this.AssignmentId = assignmentId;
            this.EngineerId = engineerId;
            this.Text = text;
        }

        public int AssignmentId { get; set; }

        public virtual Assignment Assignment { get; set; }

        public string EngineerId { get; set; }

        public virtual ApplicationUser Engineer { get; set; }

        public string Text { get; set; }

        // Architect answer
        public int? AnswerId { get; set; }

        public virtual AssignmentAnswer? Answer { get; set; }
    }
}
