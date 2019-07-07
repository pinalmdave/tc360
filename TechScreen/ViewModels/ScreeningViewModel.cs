using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechScreen.DBEntities;
using TechScreen.Models;

namespace TechScreen.ViewModels
{
    public class ScreeningViewModel
    {
        public ScreeningModel Screening { get; set; }

        public ScreeningCandidateModel ScreeningCandidateModel { get; set; }
        public JobCategoriesModel JobCategories { get; set; }
        public ScreeningQuestionsModel ScreeningQuestions { get; set; }
        public TechnologyStackModel TechnologyStack { get; set; }
        public TechnologiesModel Technologies { get; set; }

        public List<ScreeningCandidateModel> lstScreeningCandidates { get; set; }
        public List<JobCategoriesModel> lstJobCategories { get; set; }
        public List<ScreeningQuestionsModel> lstScreeningQuestions{ get; set; }
        public List<TechnologyStackModel> lstTechnologyStack { get; set; } 
        public List<TechnologiesModel> lstTechnologies { get; set; }

    }
}
