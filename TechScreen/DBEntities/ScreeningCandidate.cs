using System;
using System.Collections.Generic;

namespace TechScreen.DBEntities
{
    public partial class ScreeningCandidate
    {
        public ScreeningCandidate()
        {
            DetailedCandidateScreening = new HashSet<DetailedCandidateScreening>();
        }

        public int CandidateId { get; set; }
        public int? ScreeningId { get; set; }
        public int? ReviewerId { get; set; }
        public string CandidateFirstName { get; set; }
        public string CandidateLastName { get; set; }
        public string CandidateEmail { get; set; }
        public string CandidatePhone { get; set; }
        public string ScreeningStatus { get; set; }
        public string CandidateSignInCode { get; set; }
        public int? OverallScore { get; set; }
        public int? TechnicalCommunication { get; set; }
        public int? VerbalCommunication { get; set; }
        public int? CandidateEnthusiasm { get; set; }
        public string ReviewerComments { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }

        public virtual Reviewer Reviewer { get; set; }
        public virtual Screening Screening { get; set; }
        public virtual ICollection<DetailedCandidateScreening> DetailedCandidateScreening { get; set; }
    }
}
