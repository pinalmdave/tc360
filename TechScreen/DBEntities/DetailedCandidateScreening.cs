using System;
using System.Collections.Generic;

namespace TechScreen.DBEntities
{
    public partial class DetailedCandidateScreening
    {
        public int DetailScreeningId { get; set; }
        public int? ScreeningId { get; set; }
        public int? CandidateId { get; set; }
        public int? ReviewerId { get; set; }
        public string SkillName { get; set; }
        public int? SkillScore { get; set; }
        public string ReviwerComments { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }

        public virtual ScreeningCandidate Candidate { get; set; }
        public virtual Reviewer Reviewer { get; set; }
        public virtual Screening Screening { get; set; }
    }
}
