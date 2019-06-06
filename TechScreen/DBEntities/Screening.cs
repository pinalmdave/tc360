using System;
using System.Collections.Generic;

namespace TechScreen.DBEntities
{
    public partial class Screening
    {
        public Screening()
        {
            CustomScreeningQuestions = new HashSet<CustomScreeningQuestions>();
            DetailedCandidateScreening = new HashSet<DetailedCandidateScreening>();
            ScreeningCandidate = new HashSet<ScreeningCandidate>();
            ScreeningQuestions = new HashSet<ScreeningQuestions>();
        }

        public int ScreeningId { get; set; }
        public int? UserId { get; set; }
        public string HiringCompanyName { get; set; }
        public string JobRequisitionNumber { get; set; }
        public string IsJobDescOptSelected { get; set; }
        public string IsCustomQuestionOptSelected { get; set; }
        public string JobTitle { get; set; }
        public string JobDesc { get; set; }
        public string JobScreeningQuestions { get; set; }
        public string JobScreeningQuestionsUrl { get; set; }
        public string SpecialRequest { get; set; }
        public int? JobCatId { get; set; }
        public int? TechStackId { get; set; }
        public int? TechId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }

        public virtual JobCategories JobCat { get; set; }
        public virtual Technologies Tech { get; set; }
        public virtual TechnologyStack TechStack { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<CustomScreeningQuestions> CustomScreeningQuestions { get; set; }
        public virtual ICollection<DetailedCandidateScreening> DetailedCandidateScreening { get; set; }
        public virtual ICollection<ScreeningCandidate> ScreeningCandidate { get; set; }
        public virtual ICollection<ScreeningQuestions> ScreeningQuestions { get; set; }
    }
}
