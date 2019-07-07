using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechScreen.DBEntities;

namespace TechScreen.ViewModels
{
    public class ScoreCardVM
    {
        public int CandidateId { get; set; }

        public string CandidateName { get; set; }
        public string CandidateEmail { get; set; }
        public string CandidatePhone { get; set; }
        public string FeedbackDate { get; set; }

        public int? OverallScore { get; set; }
        public int? TechnicalCommunication { get; set; }
        public int? VerbalCommunication { get; set; }
        public int? CandidateEnthusiasm { get; set; }

        public string ReviewerComments { get; set; }

        public List<DetailedCandidateScreening> lstDetailedCandidateScreening { get; set; }

    }
}
