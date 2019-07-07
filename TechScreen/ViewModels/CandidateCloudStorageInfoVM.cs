using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechScreen.Models;

namespace TechScreen.ViewModels
{
    public class CandidateScreeningInfoVM
    {
        public string CandidateSignInCode { get; set; }
        public string CandidateId { get; set; }
        public string ScreeningId { get; set; }
        
        public int OverallScore { get; set; }
        public int TechnicalCommunication { get; set; }
        public int VerbalCommunication { get; set; }
        public int CandidateEnthusiasm { get; set; }
        public string ReviewerComments { get; set; }

        public List<DBEntities.ScreeningQuestions> ScreeningQuestions { get; set; }

        public ScreeningCandidateModel ScreeningCandidateModel { get; set; }

        public List<DetailedCandidateScreeningModel> LstDetailedCandidateScreeningModel { get; set; }
    }
}
