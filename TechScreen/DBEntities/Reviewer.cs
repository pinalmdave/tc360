using System;
using System.Collections.Generic;

namespace TechScreen.DBEntities
{
    public partial class Reviewer
    {
        public Reviewer()
        {
            ReviewerTechnologies = new HashSet<ReviewerTechnologies>();
            ScreeningCandidate = new HashSet<ScreeningCandidate>();
        }

        public int ReviewerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public string IsActive { get; set; }
        public string Rate { get; set; }
        public string ResumeLink { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }

        public virtual ICollection<ReviewerTechnologies> ReviewerTechnologies { get; set; }
        public virtual ICollection<ScreeningCandidate> ScreeningCandidate { get; set; }
    }
}
