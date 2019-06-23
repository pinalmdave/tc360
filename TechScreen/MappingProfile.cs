using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TechScreen.DBEntities;
using TechScreen.Models;

namespace TechScreen
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //DBEntity to Model
            CreateMap<Client, ClientModel>();
            CreateMap<CustomScreeningQuestions, CustomScreeningQuestionsModel>();
            CreateMap<DetailedCandidateScreening, DetailedCandidateScreeningModel>();
            CreateMap<JobCategories, JobCategoriesModel>();
            CreateMap<Reviewer, ReviewerModel>();
            CreateMap<ReviewerTechnologies, ReviewerTechnologiesModel>();
            CreateMap<Screening, ScreeningModel>();
            CreateMap<ScreeningCandidate, ScreeningCandidateModel>();
            CreateMap<ScreeningDetail, ScreeningDetailModel>();
            CreateMap<ScreeningQuestions, ScreeningQuestionsModel>();
            CreateMap<Technologies, TechnologiesModel>();
            CreateMap<TechnologyStack, TechnologyStackModel>();
            CreateMap<User, UserModel>();
            CreateMap<Transaction, TransactionModel>();

            //Model to DBEntities
            CreateMap<ScreeningModel, Screening>();
            CreateMap<TransactionModel, Transaction>();

            CreateMap<ScreeningQuestionsModel, ScreeningQuestions>();

            CreateMap<ScreeningCandidateModel, ScreeningCandidate>()
               .ForMember(dest => dest.Reviewer, opt => opt.Ignore())
               .ForMember(dest => dest.Screening, opt => opt.Ignore())
               .ForMember(dest => dest.DetailedCandidateScreening, opt => opt.Ignore());
        }
    }
}



