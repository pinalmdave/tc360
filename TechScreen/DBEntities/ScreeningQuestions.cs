using System;
using System.Collections.Generic;

namespace TechScreen.DBEntities
{
    public partial class ScreeningQuestions
    {
        public int Id { get; set; }
        public int? ScreeningId { get; set; }
        public int? QuestionId { get; set; }
        public string QuestionText { get; set; }

        public virtual Screening Screening { get; set; }
    }
}
