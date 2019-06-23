﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TechScreen.Models
{
    public class ScreeningModel
    {
        public int ScreeningId { get; set; }
        public int? UserId { get; set; }
        public int? ReviewerId { get; set; }
        public string Status { get; set; }


        [Required]
        [Display(Name = "Hiring Company Name")]
        public string HiringCompanyName { get; set; }

        public string JobRequisitionNumber { get; set; }

        public string IsJobDescOptSelected { get; set; }

        public string IsCustomQuestionOptSelected { get; set; }

        [Required]
        [Display(Name = "Job Title")]
        [StringLength(100, MinimumLength = 3)]
        public string JobTitle { get; set; }


        public string JobDesc { get; set; }
        public  string SpecialRequest { get; set; }
        public string JobScreeningQuestions { get; set; }
        public string JobScreeningQuestionsUrl { get; set; }
        public int? JobCatId { get; set; }
        public int? TechStackId { get; set; }
        public int? TechId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }


        public UserModel User { get; set; }
        public  JobCategoriesModel JobCat { get; set; }
        public TechnologiesModel Tech { get; set; }
        public TechnologyStackModel TechStack { get; set; }
        public  ICollection<CustomScreeningQuestionsModel> CustomScreeningQuestions { get; set; }
        public ICollection<DetailedCandidateScreeningModel> DetailedCandidateScreening { get; set; }
        public ICollection<ScreeningCandidateModel> ScreeningCandidate { get; set; }
        public ICollection<ScreeningQuestionsModel> ScreeningQuestions { get; set; }
        public ICollection<TransactionModel> Transaction { get; set; }
    }
}
