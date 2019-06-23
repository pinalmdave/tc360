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

        public bool CreateScreeningQuestions(int screeningId, string userEmail, List<ScreeningQuestionsModel> lstScreeningQuestionsModel)
        {
            try
            {
                var screening = this.context.Screening.Include(sc=> sc.ScreeningCandidate).Include(sq=> sq.ScreeningQuestions).Where(x => x.ScreeningId == screeningId).FirstOrDefault();

                screening.LastUpdated = DateTime.Now;
                screening.LastUpdatedBy = userEmail;
                screening.Status = EnumScreeningStatus.AwaitingCandidateResponse.ToString();

                foreach (var candidate in screening.ScreeningCandidate)
                {
                    candidate.ScreeningStatus = EnumScreeningStatus.AwaitingCandidateResponse.ToString();
                    candidate.LastUpdated = DateTime.Now;
                    candidate.LastUpdatedBy = userEmail;
                    candidate.CandidateSignInCode = Guid.NewGuid().ToString();
                }

                screening.ScreeningQuestions = this._mapper.Map(lstScreeningQuestionsModel , new List<ScreeningQuestions>());

                this.context.Screening.Update(screening);
                Save();
                return true;
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public ScreeningModel GetScreeningDetailToCreateQuestions(int screeningId)
        {
            try
            {
                var result = context.Screening.Include(r => r.ScreeningQuestions).Where(x => x.ScreeningId == screeningId).FirstOrDefault();

                return this._mapper.Map(result, new ScreeningModel());
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ScreeningModel GetScreeningDetailForUpdate(int screeningId)
        {
            try
            {
                var result = context.Screening.Include(r => r.ScreeningQuestions).Include(sc=>sc.ScreeningCandidate).Where(x => x.ScreeningId == screeningId).FirstOrDefault();

                return this._mapper.Map(result, new ScreeningModel());
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<ScreeningModel> GetReviewerScreenings(int reviewerId)
        {
            try
            {
                var result = context.Screening.Include(r => r.ScreeningCandidate).Where(x => x.ReviewerId == reviewerId).ToList();
                                 
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

        public bool AssignCandidateToReviewer(int screeningId, int candidateId, int reviewerId, string userName)
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

        public bool AssignScreeningToReviewer(int screeningId, int reviewerId, string userName)
        {
            try
            {
                var screening = this.context.Screening.Where(x => x.ScreeningId == screeningId).Include(sc=> sc.ScreeningCandidate).FirstOrDefault();
                screening.ReviewerId = reviewerId;
                screening.LastUpdated = DateTime.Now;
                screening.LastUpdatedBy = userName;

                var screeningCandidates = screening.ScreeningCandidate.ToList(); 

                foreach(var candidate in screeningCandidates)
                {
                    candidate.ReviewerId = reviewerId;
                    candidate.LastUpdated = DateTime.Now;
                    candidate.LastUpdatedBy = userName;
                }

                this.context.Screening.Update(screening);

                Save();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool AssignScreening(int screeningId, int reviewerId, string userName)
        {
            try
            {
                var screeningCandidate = this.context.ScreeningCandidate.Where(x => x.ScreeningId == screeningId).FirstOrDefault();
                screeningCandidate.ReviewerId = reviewerId;

                screeningCandidate.LastUpdated = DateTime.Now;
                screeningCandidate.LastUpdatedBy = userName;
                this.context.ScreeningCandidate.Update(screeningCandidate);
                Save();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public TransactionModel GetTransaction(int transactionId)
        {
            var transaction = this.context.Transaction.Where(t => t.TransactionId == transactionId).FirstOrDefault();
            var transactionModel = this._mapper.Map(transaction, new TransactionModel());
            return transactionModel;
        }

        public bool UpdateTransaction(int transactionId, string status)
        {
            var transaction = this.context.Transaction.Where(t => t.TransactionId == transactionId).FirstOrDefault();
            transaction.PaymentStatus = status;
            transaction.LastUpdated = DateTime.Now;
            this.context.Transaction.Update(transaction);
            var isSaved = Save();
            return isSaved;
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
