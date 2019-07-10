using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TechScreen.Models
{
    public enum EnumScreeningStatus
    {
        [Display(Name ="Submit Questions")]
        Submit_Questions,

        [Display(Name = "Awaiting Candidate Response")]
        Awaiting_Candidate_Response,

        [Display(Name = "Awaiting Screening Invitation")]
        Awaiting_Screening_Invitation,

        [Display(Name = "Pending Review")]
        Pending_Review,

        [Display(Name = "Candidate Response Received")]
        Candidate_Response_Received,

        [Display(Name = "Completed")]
        Completed,

        Submitted,

        Cancelled,

        New,

        [Display(Name = "In Process")]
        In_Process,

        [Display(Name = "Awaiting Admin Response")]
        Awaiting_Reviewer_Response,

        Assigned,

        Screening_Invitation_Sent
    }
}
