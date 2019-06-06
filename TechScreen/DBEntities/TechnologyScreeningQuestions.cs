using System;
using System.Collections.Generic;

namespace TechScreen.DBEntities
{
    public partial class TechnologyScreeningQuestions
    {
        public int Id { get; set; }
        public int? TechStackId { get; set; }
        public int? TechId { get; set; }
        public string ScreeningQuestion { get; set; }

        public virtual Technologies Tech { get; set; }
        public virtual TechnologyStack TechStack { get; set; }
    }
}
