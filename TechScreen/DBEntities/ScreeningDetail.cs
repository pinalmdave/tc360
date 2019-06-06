using System;
using System.Collections.Generic;

namespace TechScreen.DBEntities
{
    public partial class ScreeningDetail
    {
        public int Id { get; set; }
        public int? ScreeningId { get; set; }
        public int? ReviewerId { get; set; }
        public int? TechStackId { get; set; }
        public int? OverallScore { get; set; }
        public int? CommunicationSkills { get; set; }
        public int? Attitude { get; set; }
        public int? ConfidenceLevel { get; set; }
        public int? FacialGesture { get; set; }
        public string VideoUrl { get; set; }
        public int? TechId { get; set; }
        public int? Score { get; set; }
        public string ReviewerComments { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }

        public virtual Reviewer Reviewer { get; set; }
        public virtual Screening Screening { get; set; }
        public virtual Technologies Tech { get; set; }
    }
}
