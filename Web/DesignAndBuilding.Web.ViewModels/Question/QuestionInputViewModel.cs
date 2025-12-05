using System;
using System.Collections.Generic;
using System.Text;

namespace DesignAndBuilding.Web.ViewModels.Question
{
    public class QuestionInputViewModel
    {
        public int AssignmentId { get; set; }

        public string Text { get; set; }

        public string EngineerId { get; set; }
    }
}
