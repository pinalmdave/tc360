using System;
using System.Collections.Generic;

namespace TechScreen.DBEntities
{
    public partial class Technologies
    {
        public Technologies()
        {
            CustomScreeningQuestions = new HashSet<CustomScreeningQuestions>();
            TechnologyScreeningQuestions = new HashSet<TechnologyScreeningQuestions>();
        }

        public int TechId { get; set; }
        public int? TechStackId { get; set; }
        public string TechName { get; set; }

        public virtual TechnologyStack TechStack { get; set; }
        public virtual ICollection<CustomScreeningQuestions> CustomScreeningQuestions { get; set; }
        public virtual ICollection<TechnologyScreeningQuestions> TechnologyScreeningQuestions { get; set; }
    }
}
