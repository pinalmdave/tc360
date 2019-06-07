using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TechScreen.DBEntities;
using TechScreen.Models;
using AutoMapper;

namespace TechScreen.Services
{
    public class ScreeningRepository : IScreeningRepository
    {
        private TechscreenDBContext context;
        private readonly IMapper _mapper;


        public ScreeningRepository(TechscreenDBContext _context, IMapper mapper)
        {
            this.context = _context;
            this._mapper = mapper;
        }

        public List<ScreeningModel> GetReviewerScreenings(int reviewerId)
        {
            try
            {
                var result = context.Screening.Where(s => s.ScreeningCandidate.Any(sc => sc.ReviewerId == reviewerId)).Include(dc => dc.DetailedCandidateScreening).ToList();
                                 
                return this._mapper.Map(result, new List<ScreeningModel>());
            }

            catch(Exception ex)
            {
                return null;
            }
        }

        public List<ReviewerModel> GetReviewers()
        {
            var reviewers = this.context.Reviewer
                            .Include(rt => rt.ReviewerTechnologies).ToList();

            return this._mapper.Map(reviewers, new List<ReviewerModel>());
        }

        public void AddNewScreening(Screening screening)
        {
            this.context.Screening.Add(screening);
            Save();
        }

        public List<Screening> GetUserScreening(string userEmail)
        {
            var userId = GetUserId(userEmail);
            var lstUserScreening = context.Screening.Include(s=> s.ScreeningCandidate).Where(x => x.UserId == userId).ToList();
            return lstUserScreening;
        }

        public int GetUserId(string userEmail)
        {
            var userId = this.context.User.Where(x => x.UserEmail == userEmail).FirstOrDefault().UserId;
            return userId;
        }

        public bool AssignReviewer(int screeningId, int candidateId, int reviewerId, string userName)
        {
            try
            {
                var screeningCandidate = this.context.ScreeningCandidate.Where(x => x.ScreeningId == screeningId && x.CandidateId == candidateId).FirstOrDefault();
                screeningCandidate.ReviewerId = reviewerId;

                screeningCandidate.LastUpdated = DateTime.Now;
                screeningCandidate.LastUpdatedBy = userName;
                this.context.ScreeningCandidate.Update(screeningCandidate);
                Save();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }

        }

        public List<ScreeningQuestions> GetScreeningQuestions(string candidateCode)
        {
            //var screening = context.Screening.Where(x => x.CandidateSignInCode == candidateCode.Trim()).FirstOrDefault();

            //if (screening == null) return null;

            //var lstScreeningQuestions = context.ScreeningQuestions.Where(x => x.ScreeningId == screening.ScreeningId).ToList();

            //return lstScreeningQuestions;
            return null;

        }

        public void CancelScreening(int screeningId, string userId)
        {
            var screening = this.context.Screening.Where(x => x.ScreeningId == screeningId).FirstOrDefault();
            //screening.ScreeningStatus = "Cancelled";
            screening.LastUpdated = DateTime.Now;
            screening.LastUpdatedBy = userId;
            this.context.Screening.Add(screening);
            Save();
        }

        public bool IsNewClient(string email)
        {
            var client = this.context.Client.Where(x => x.Email == email).FirstOrDefault();
            if (client == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int AddNewClient(Client client)
        {
            this.context.Add(client);
            context.SaveChanges();
            var clientId = client.ClientId;
            return clientId;
        }

        public int AddNewUser(User user)
        {
            this.context.Add(user);
            context.SaveChanges();
            var userId = user.UserId;
            return userId;
        }

        public bool Save()
        {
            return (this.context.SaveChanges() >= 0);
        }

    }
}
