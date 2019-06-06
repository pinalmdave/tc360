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
        public int? OverAllScore { get; set; }
        public int? CommunicationSkills { get; set; }
        public int? Attitude { get; set; }
        public int? FacialGesture { get; set; }
        public string VideoUrl { get; set; }
        public string ReviewerComments { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }
    }
}
