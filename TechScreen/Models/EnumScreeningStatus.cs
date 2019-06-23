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
        SubmitQuestions,

        [Display(Name = "Awaiting Candidate Response")]
        AwaitingCandidateResponse,

        [Display(Name = "Awaiting Screening Invitation")]
        AwaitingScreeningInvitation,

        [Display(Name = "Pending Review")]
        PendingReview,

        [Display(Name = "Candidate Response Received")]
        CandidateResponseReceived,

        [Display(Name = "Review Completed")]
        ReviewCompleted
    }
}
