using System;
using System.Collections.Generic;

namespace TechScreen.DBEntities
{
    public partial class ReviewerTechnologies
    {
        public int Id { get; set; }
        public int? ReviewerId { get; set; }
        public int? TechStackId { get; set; }
        public int? TechId { get; set; }

        public virtual Reviewer Reviewer { get; set; }
    }
}
