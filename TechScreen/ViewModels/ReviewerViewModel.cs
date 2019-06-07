using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechScreen.Models;

namespace TechScreen.ViewModels
{
    public class ReviewerViewModel
    {
        public ScreeningModel screeningModel { get; set; }
        public ScreeningCandidateModel screeningCandidateModel { get; set; }

        public DetailedCandidateScreeningModel detailedCandidateScreeningModel { get; set; }
    }
}
