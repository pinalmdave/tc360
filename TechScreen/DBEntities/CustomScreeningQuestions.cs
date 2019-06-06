using System;
using System.Collections.Generic;

namespace TechScreen.DBEntities
{
    public partial class CustomScreeningQuestions
    {
        public int Id { get; set; }
        public int? ScreeningId { get; set; }
        public int? TechStackId { get; set; }
        public int? TechId { get; set; }
        public string QuestionText { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }

        public virtual Screening Screening { get; set; }
        public virtual Technologies Tech { get; set; }
        public virtual TechnologyStack TechStack { get; set; }
    }
}
