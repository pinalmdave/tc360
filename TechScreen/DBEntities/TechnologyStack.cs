using System;
using System.Collections.Generic;

namespace TechScreen.DBEntities
{
    public partial class TechnologyStack
    {
        public TechnologyStack()
        {
            CustomScreeningQuestions = new HashSet<CustomScreeningQuestions>();
            Screening = new HashSet<Screening>();
            Technologies = new HashSet<Technologies>();
            TechnologyScreeningQuestions = new HashSet<TechnologyScreeningQuestions>();
        }

        public int TechStackId { get; set; }
        public string TechSuiteName { get; set; }

        public virtual ICollection<CustomScreeningQuestions> CustomScreeningQuestions { get; set; }
        public virtual ICollection<Screening> Screening { get; set; }
        public virtual ICollection<Technologies> Technologies { get; set; }
        public virtual ICollection<TechnologyScreeningQuestions> TechnologyScreeningQuestions { get; set; }
    }
}
