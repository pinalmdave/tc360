using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TechScreen.Models
{
    public class ScreeningCandidateModel
    {
        public int CandidateId { get; set; }
        public int? ScreeningId { get; set; }
        public int? ReviewerId { get; set; }

        [Required]
        [Display(Name = "Candidate First Name")]
        public string CandidateFirstName { get; set; }

        [Required]
        [Display(Name = "Candidate Last Name")]
        public string CandidateLastName { get; set; }

        [Required]
        [Display(Name = "Candidate Email")]
        public string CandidateEmail { get; set; }
        public string CandidatePhone { get; set; }
        public string ScreeningStatus { get; set; }
        public string CandidateSignInCode { get; set; }

        [Required]
        [Display(Name = "Overall Score")]
        public int? OverallScore { get; set; }

        [Required]
        [Display(Name = "Technical Communication")]
        public int? TechnicalCommunication { get; set; }

        [Required]
        [Display(Name = "Verbal Communication")]
        public int? VerbalCommunication { get; set; }

        [Required]
        [Display(Name = "Candidate Enthusiasm")]
        public int? CandidateEnthusiasm { get; set; }

        [Required]
        [Display(Name = "Overall impression and Recommendation")]
        public string ReviewerComments { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }

        public  ReviewerModel ReviewerModel { get; set; }
        public  ScreeningModel ScreeningModel { get; set; }
        public  ICollection<DetailedCandidateScreeningModel> DetailedCandidateScreeningModel { get; set; }
    }
}
